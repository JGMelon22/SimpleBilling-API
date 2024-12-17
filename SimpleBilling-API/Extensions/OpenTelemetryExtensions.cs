using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;

namespace SimpleBilling_API.Extensions;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection RegisterOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry().WithMetrics((options) =>
        {
            options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("SimpleBilling-API"))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    // .AddConsoleExporter()
                    .AddOtlpExporter(otel =>
                    {
                        otel.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                        otel.Endpoint = new Uri(configuration["OtlpExporter:Endpoint"]!);
                    });
        });

        services.AddOpenTelemetry().WithTracing((options) =>
        {
            options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("SimpleBilling-API"))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                // .AddConsoleExporter()
                .AddOtlpExporter(otel =>
                {
                    otel.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    otel.Endpoint = new Uri(configuration["OtlpExporter:Endpoint"]!);
                });
        });

        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddOpenTelemetry(options =>
            {
                options.IncludeScopes = true;
                options.AddConsoleExporter();
                options.AddOtlpExporter(otel =>
                {
                    otel.Endpoint = new Uri(configuration["OtlpExporter:Endpoint"]!);
                });
            });
        });

        return services;
    }
}
