using CustomerService.Api;
using CustomerService.Api.Extensions;
using CustomerService.Application;
using CustomerService.Infrastructure;
using CustomerService.Infrastructure.Identity;
using CustomerService.Infrastructure.Notifications;

var builder = WebApplication.CreateBuilder(args);

// ---- Services ----
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiLayer();
builder.Services.AddControllers();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsPolicies();
builder.Services.AddSwaggerWithAuth();

// ---- Build ----
var app = builder.Build();

// ---- Seed roles/test users (must run before serving requests) ----
using (var scope = app.Services.CreateScope())
{
    await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);

    if (app.Environment.IsDevelopment())
    {
        await TestUserSeeder.SeedAsync(scope.ServiceProvider);
    }
}

// ---- Middleware pipeline ----
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(CorsServiceExtensions.TestPolicyName);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<RequestHub>("/hubs/requests");

app.Run();