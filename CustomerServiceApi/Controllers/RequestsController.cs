using CustomerService.Application.Messages.Commands.RequestAdditionalInfo;
using CustomerService.Application.Requests.Commands.AssignRequest;
using CustomerService.Application.Requests.Commands.ChangeRequestCategory;
using CustomerService.Application.Requests.Commands.ChangeRequestStatus;
using CustomerService.Application.Requests.Commands.ChangeRequestUrgency;
using CustomerService.Application.Requests.Commands.SubmitRequest;
using CustomerService.Application.Requests.Commands.TakeRequest;
using CustomerService.Application.Requests.Queries.GetRequestById;
using CustomerService.Application.Requests.Queries.GetRequestLogs;
using CustomerService.Application.Requests.Queries.GetRequestsQuery;
using CustomerService.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomerService.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RequestsController : ApiController
    {
        private readonly ISender _mediator;

        public RequestsController(ISender mediator)
        {
            _mediator = mediator;
        }

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue("sub")!);

        private string CurrentUserRole =>
    User.FindFirstValue(ClaimTypes.Role)!;

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Submit(SubmitRequestRequest request)
        {
            var command = new SubmitRequestCommand(
                CurrentUserId, request.Urgency, request.Category, request.Description);

            var result = await _mediator.Send(command);

            return result.Match(response => Ok(response), Problem);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetRequestByIdQuery(id, CurrentUserId, CurrentUserRole));
            return result.Match(response => Ok(response), Problem);
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests(
       [FromQuery] Guid? customerId,
       [FromQuery] Guid? agentId,
       [FromQuery] bool? unassignedOnly,
       [FromQuery] string? status,
       [FromQuery] string? urgency,
       [FromQuery] string? category,
       [FromQuery] int pageNumber = 1,
       [FromQuery] int pageSize = 20)
        {
            var query = new GetRequestQuery(
                customerId, agentId, unassignedOnly, status, urgency, category,
                pageNumber, pageSize, CurrentUserId, CurrentUserRole);

            var result = await _mediator.Send(query);
            return result.Match(response => Ok(response), Problem);
        }
        [HttpGet("{id:guid}/logs")]
        public async Task<IActionResult> GetLogs(Guid id)
        {
            var result = await _mediator.Send(new GetRequestLogsQuery(id, CurrentUserId, CurrentUserRole));
            return result.Match(response => Ok(response), Problem);
        }

        [HttpPost("{id:guid}/take")]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Take(Guid id)
        {
            var command = new TakeRequestCommand(id, CurrentUserId);
            var result = await _mediator.Send(command);
            return result.Match(response => Ok(response), Problem);
        }

        [HttpPost("{id:guid}/assign")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Assign(Guid id, [FromBody] Guid agentId)
        {
            var command = new AssignRequestCommand(id, agentId, CurrentUserId);
            var result = await _mediator.Send(command);
            return result.Match(response => Ok(response), Problem);
        }

        [HttpPut("{id:guid}/status")]
        [Authorize(Roles = "Agent,Manager")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] string newStatus)
        {
            var command = new ChangeRequestStatusCommand(id, newStatus, CurrentUserId);
            var result = await _mediator.Send(command);
            return result.Match(response => Ok(response), Problem);
        }

        [HttpPut("{id:guid}/urgency")]
        [Authorize(Roles = "Agent,Manager")]
        public async Task<IActionResult> ChangeUrgency(Guid id, [FromBody] string newUrgency)
        {
            var command = new ChangeRequestUrgencyCommand(id, newUrgency, CurrentUserId);
            var result = await _mediator.Send(command);
            return result.Match(response => Ok(response), Problem);
        }

        [HttpPut("{id:guid}/category")]
        [Authorize(Roles = "Agent,Manager")]
        public async Task<IActionResult> ChangeCategory(Guid id, [FromBody] string newCategory)
        {
            var command = new ChangeRequestCategoryCommand(id, newCategory, CurrentUserId);
            var result = await _mediator.Send(command);
            return result.Match(response => Ok(response), Problem);
        }
        [HttpPost("{requestId:guid}/request-additional-info")]
        [Authorize(Roles = "Agent,Manager")]
        public async Task<IActionResult> RequestAdditionalInfo(Guid requestId, [FromBody] string content)
        {
            var command = new RequestAdditionalInfoCommand(requestId, CurrentUserId, content);
            var result = await _mediator.Send(command);
            return result.Match(response => Ok(response), Problem);
        }
    }
}
