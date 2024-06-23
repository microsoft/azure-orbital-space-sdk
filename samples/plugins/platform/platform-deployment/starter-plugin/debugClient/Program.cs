using Google.Protobuf.WellKnownTypes;
using Microsoft.Azure.SpaceFx;
using Microsoft.Azure.SpaceFx.MessageFormats.PlatformServices.Deployment;

namespace PayloadApp.DebugClient;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.{env:DOTNET_ENVIRONMENT}.json", optional: true, reloadOnChange: true);

        builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(50051, o => o.Protocols = HttpProtocols.Http2))
        .ConfigureServices((services) =>
        {
            services.AddAzureOrbitalFramework();
            services.AddHostedService<PayloadApp.DebugClient.MessageSender>();
            services.AddSingleton<Core.IMessageHandler<Microsoft.Azure.SpaceFx.MessageFormats.PlatformServices.Deployment.PositionRequest>, PayloadApp.DebugClient.MessageHandler<Microsoft.Azure.SpaceFx.MessageFormats.PlatformServices.Deployment.PositionRequest>>();
            services.AddSingleton<Core.IMessageHandler<Microsoft.Azure.SpaceFx.MessageFormats.PlatformServices.Deployment.PositionResponse>, PayloadApp.DebugClient.MessageHandler<Microsoft.Azure.SpaceFx.MessageFormats.PlatformServices.Deployment.PositionResponse>>();
            services.AddSingleton<Core.IMessageHandler<Microsoft.Azure.SpaceFx.MessageFormats.PlatformServices.Deployment.PositionUpdateRequest>, PayloadApp.DebugClient.MessageHandler<Microsoft.Azure.SpaceFx.MessageFormats.PlatformServices.Deployment.PositionUpdateRequest>>();
            services.AddSingleton<Core.IMessageHandler<Microsoft.Azure.SpaceFx.MessageFormats.PlatformServices.Deployment.PositionUpdateResponse>, PayloadApp.DebugClient.MessageHandler<Microsoft.Azure.SpaceFx.MessageFormats.PlatformServices.Deployment.PositionUpdateResponse>>();
            Core.APP_CONFIG appConfig = new()
            {
                HEARTBEAT_PULSE_TIMING_MS = 3000,
                HEARTBEAT_RECEIVED_TOLERANCE_MS = 10000
            };

            services.AddSingleton(appConfig);

        }).ConfigureLogging((logging) =>
        {
            logging.AddProvider(new Microsoft.Extensions.Logging.SpaceFX.Logger.HostSvcLoggerProvider());
            logging.AddConsole();
        });

        var app = builder.Build();

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<Core.Services.MessageReceiver>();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            });
        });
        app.Run();
    }
}