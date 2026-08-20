# Customer Support Request Management API

A backend system for receiving, tracking, and resolving customer support requests, built as a candidate take-home assignment. Implements Clean Architecture, Domain-Driven Design, and CQRS (via MediatR) on ASP.NET Core.

## Tech Stack

- **.NET 10** / ASP.NET Core Web API
- **Entity Framework Core** (SQL Server)
- **MediatR** — CQRS command/query dispatch
- **FluentValidation** — request validation pipeline
- **ErrorOr** — result/error handling without exceptions
- **ASP.NET Core Identity** — authentication store
- **JWT Bearer** — access/refresh token authentication
- **SignalR** — real-time message delivery
- **Swashbuckle (Swagger)** — API documentation

## Architecture

The solution follows Clean Architecture with four layers:

```
CustomerService.Domain          — entities, value objects, domain events, business rules
CustomerService.Application     — commands, queries, handlers, validators, interfaces (CQRS via MediatR)
CustomerService.Infrastructure  — EF Core, Identity, SignalR, email, repository implementations
CustomerService.Api             — controllers, JWT/CORS/Swagger configuration, composition root
CustomerService.Contracts       — request/response DTOs shared between Api and Application
```

Dependency direction: `Api → Application/Infrastructure/Contracts`, `Infrastructure → Application/Domain`, `Application → Domain`, `Domain → (nothing)`.

### Core domain model

- **User** (TPT inheritance) — base type with `Customer`, `Agent`, `Manager` subtypes. Kept separate from ASP.NET Identity's `ApplicationUser`, linked by a shared `Guid` id, so the domain model isn't coupled to the auth framework.
- **Request** (aggregate root) — the support ticket itself. Holds status, urgency, category, customer/agent ids, and its own `Logs` collection.
- **Log** (child entity of Request) — an audit trail entry recording every traceable change (status, urgency, category, assignment). Written via domain event handlers, not inline in the aggregate's methods, so log-writing and future notification handling stay decoupled from the core mutation logic.
- **Message** (separate aggregate root) — customer/agent communication tied to a Request by id. Broadcast live via SignalR when created.

### Request lifecycle

```
Submitted → Assigned → InProgress → WaitingOnCustomer → InProgress → Completed → Reopened → Assigned
```

Transitions are enforced by a dedicated `RequestStatusTransitionRules` class — illegal transitions are rejected before any data changes.

## Setup & Running Locally

### Prerequisites
- .NET 10 SDK
- SQL Server (local or LocalDB)
- A Gmail account with an [App Password](https://myaccount.google.com/apppasswords) generated (for sending real emails — confirmation and password reset)

### 1. Clone and restore

```bash
git clone <repo-url>
cd CustomerService
dotnet restore
```

### 2. Configure secrets

Sensitive values are kept out of `appsettings.json` via .NET User Secrets. From the `CustomerService.Api` project directory:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=CustomerServiceDb;Trusted_Connection=True;TrustServerCertificate=True"
dotnet user-secrets set "JwtSettings:Secret" "<a random string, 32+ characters>"
dotnet user-secrets set "EmailSettings:SmtpHost" "smtp.gmail.com"
dotnet user-secrets set "EmailSettings:SmtpPort" "587"
dotnet user-secrets set "EmailSettings:SenderEmail" "<your gmail address>"
dotnet user-secrets set "EmailSettings:SenderName" "Customer Support"
dotnet user-secrets set "EmailSettings:Username" "<your gmail address>"
dotnet user-secrets set "EmailSettings:Password" "<your 16-character Gmail App Password, no spaces>"
```

`JwtSettings:Issuer`, `JwtSettings:Audience`, and `JwtSettings:ExpiryMinutes` already have working defaults in `appsettings.json` and don't need to be secret.

### 3. Prepare the database

```bash
cd CustomerService.Api
dotnet ef database update --project ../CustomerService.Infrastructure --startup-project .
```

This creates all tables (Identity tables, Users/Customers/Agents/Managers, Requests, Logs, Messages) via the included EF Core migrations.

### 4. Run

```bash
dotnet run --project CustomerService.Api
```

On first startup, the app automatically:
- Seeds the `Customer`, `Agent`, and `Manager` roles.
- (Development only) Seeds two ready-to-use test accounts — see below.

Swagger UI is available at `/swagger` when running in Development.

## Test Users

Two non-customer accounts are seeded automatically in Development (self-registration only creates Customer accounts, by design):

| Role    | Email                | Password         |
|---------|-----------------------|------------------|
| Agent   | agent@example.com     | AgentPass123!    |
| Manager | manager@example.com   | ManagerPass123!  |

Customer accounts are created via `POST /api/auth/register`. New accounts must confirm their email (a real email is sent) before they can log in.

## Authentication

- `POST /api/auth/register` — creates a Customer account, sends a confirmation email.
- `POST /api/auth/login` — returns a JWT access token (15 min) + refresh token (7 days, rotates on use).
- `POST /api/auth/refresh-token` — exchanges an expired access token + valid refresh token for a new pair.
- `GET /api/auth/confirm-email` — confirms an account from the emailed link.
- `POST /api/auth/forgot-password` / `POST /api/auth/reset-password` — password reset via emailed token. (No frontend exists yet — the reset link points at the API directly; testers copy the `userId`/`token` query values from the email into a `reset-password` request manually.)
- `GET /api/auth/me` — the current authenticated user's profile.

In Swagger, use the **Authorize** button and paste the raw access token (no `Bearer ` prefix needed).

## Core Endpoints

| Resource | Endpoint |
|---|---|
| Requests | `POST /api/requests` (submit), `GET /api/requests/{id}`, `GET /api/requests` (filtered/paged — customer id, agent id, unassigned-only, status, urgency, category), `GET /api/requests/{id}/logs`, `POST /api/requests/{id}/take`, `POST /api/requests/{id}/assign`, `PUT /api/requests/{id}/status`, `PUT /api/requests/{id}/urgency`, `PUT /api/requests/{id}/category`, `POST /api/requests/{id}/request-additional-info` |
| Messages | `POST /api/requests/{requestId}/messages`, `GET /api/requests/{requestId}/messages` |

Access control is enforced both by role (`[Authorize(Roles = "...")]`) and by resource ownership — a Customer can only see their own requests/messages/logs; an Agent only what's assigned to them; a Manager sees everything.

## Real-Time Messaging

New messages broadcast live over SignalR to anyone viewing that request. Connect to:

```
wss://<host>/hubs/requests?access_token=<JWT access token>
```

then invoke `JoinRequestGroup(requestId)` on the hub connection. New messages arrive via the `ReceiveMessage` client event.

## Assumptions, Decisions & Known Limitations

**Assumptions**
- "Request" (the ticket) and the customer's underlying "issue" are the same concept — modeled as a single `Request` entity, not split into separate Issue/Request concepts.
- Self-registration always creates a Customer; Agent and Manager accounts are provisioned directly (seeded for this assignment; a real deployment would need an admin-only creation flow, not yet built).
- Category values (`Technical`, `Billing`, `AccountAccess`, `FeatureRequest`, `General`) and Urgency values (`Low`, `Medium`, `High`, `Critical`) are a reasonable default set — the PRD didn't specify exact values.

**Key design decisions**
- Log entries are written via domain event handlers (dispatched by an EF Core `SaveChangesInterceptor` *before* the actual database write) rather than inline in the aggregate's own methods — this keeps the field-change and its audit entry atomic in one transaction while keeping the aggregate's core methods free of logging concerns.
- The Domain `User` and Identity's `ApplicationUser` are two separate classes linked by a shared id, rather than one class inheriting `IdentityUser` — keeps the Domain layer free of any ASP.NET Core dependency.
- All repository interfaces live in the Application layer (not Domain), with implementations in Infrastructure — Domain has zero outward dependencies.

**Known limitations / incomplete items**
- The `Take` (claim an unassigned request) action guards against double-assignment only in memory (checking `AgentId == null` on the loaded aggregate before saving). A genuine concurrent race — two agents claiming the same request in the same instant — is not fully closed at the database level; a conditional `WHERE AgentId IS NULL` update or an EF Core concurrency token would be the next step.
- Internal support notes (visible to staff only, never to the customer) are not implemented — messaging currently only supports the shared customer/agent conversation.
- No frontend exists. Endpoints that would normally point to a web page (email confirmation, password reset) currently point directly at the API and return a plain response.
- Optional enhancements from the PRD (attachments, notifications beyond in-conversation messages, an overdue/inactive requests view, workload summaries, post-completion customer feedback, escalation) were deliberately left out of scope, per the assignment's own guidance to prioritize a complete core workflow.

## Commit History

Commits are structured to reflect implementation progress by feature area (domain modeling, application layer, infrastructure wiring, API endpoints) rather than as a single bulk commit — see the git log for the full sequence.
