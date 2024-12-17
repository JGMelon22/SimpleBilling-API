using FluentValidation;
using Serilog;
using SimpleBilling_API.DTOs;
using SimpleBilling_API.Infrastructure.Data;
using SimpleBilling_API.Infrastructure.Repository;
using SimpleBilling_API.Infrastructure.Validators;
using SimpleBilling_API.Interfaces;
using Wolverine;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;
using SimpleBilling_API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(options =>
{
    options.Version = "v1";
    options.Title = "SimpleBilling-API";
    options.Description = "A Simple Web API simulating a billing system.";
});

builder.Services.AddSingleton<DapperDbContext>();

builder.Services.AddScoped<IItemRepository, ItemRepository>();

builder.Services.AddTransient<IValidator<ItemRequest>, ItemValidator>();

# region [Otel Setup]

builder.Services.RegisterOpenTelemetry(builder.Configuration);

#endregion

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

builder.Host.UseWolverine(opts =>
{
    opts.Policies.MessageExecutionLogLevel(LogLevel.None);
    opts.Policies.MessageSuccessLogLevel(LogLevel.Debug);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
