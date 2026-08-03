using CustomerService.Application;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApplication();

    builder.Services.AddControllers();

    builder.Services.AddOpenApi();

}
// Add services to the container.

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
