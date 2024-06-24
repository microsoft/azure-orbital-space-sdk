using Microsoft.Azure.SpaceFx.MessageFormats.Common;
using Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor;

namespace DebugClient;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(50051, o => o.Protocols = HttpProtocols.Http2))
        .ConfigureServices((services) => {
            services.AddAzureOrbitalFramework();
            services.AddHostedService<MessageSender>();

            // Register with the messages we're expecting to receive from the service
            services.AddSingleton<Core.IMessageHandler<PluginHealthCheckMultiResponse>, MessageHandler<PluginHealthCheckMultiResponse>>();
            services.AddSingleton<Core.IMessageHandler<SensorData>, MessageHandler<SensorData>>();
            services.AddSingleton<Core.IMessageHandler<TaskingPreCheckResponse>, MessageHandler<TaskingPreCheckResponse>>();
            services.AddSingleton<Core.IMessageHandler<SensorsAvailableResponse>, MessageHandler<SensorsAvailableResponse>>();
            services.AddSingleton<Core.IMessageHandler<TaskingResponse>, MessageHandler<TaskingResponse>>();
            services.AddSingleton<Core.IMessageHandler<TaskingResponse>, MessageHandler<TaskingResponse>>();
            services.AddSingleton<Core.IMessageHandler<LinkResponse>, MessageHandler<LinkResponse>>();
            services.AddSingleton<Core.IMessageHandler<PositionResponse>, MessageHandler<PositionResponse>>();
            services.AddSingleton<Core.IMessageHandler<PositionUpdateResponse>, MessageHandler<PositionUpdateResponse>>();

        }).ConfigureLogging((logging) => {
            // Enable the Azure Orbital Space SDK Logging to route messages to the hostsvc-logging
            logging.AddProvider(new Microsoft.Extensions.Logging.SpaceFX.Logger.HostSvcLoggerProvider());
            logging.AddConsole();
        });

        var app = builder.Build();

        // Configure a global exception handler to log the exception and stop the application on any uncaught exceptions
        app.Use(async (context, next) => {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            var appLifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
            try {
                await next();
            } catch (Exception ex) {
                logger.LogError(ex, "Unhandled exception occurred. Stopping the application.");
                appLifetime.StopApplication();
                throw; // Re-throwing the exception is optional and depends on how you want to handle the error response.
            }
        });

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapGrpcService<Core.Services.MessageReceiver>());
        app.Run();
    }
}
