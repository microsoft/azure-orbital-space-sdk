using Google.Protobuf.WellKnownTypes;

namespace Microsoft.Azure.SpaceFx.PlatformServices.Deployment.Plugins;
public class StarterPlugin : Microsoft.Azure.SpaceFx.PlatformServices.Deployment.Plugins.PluginBase {
    public StarterPlugin() {
        LoggerFactory loggerFactory = new();
        this.Logger = loggerFactory.CreateLogger<StarterPlugin>();
    }

    public override void ConfigureLogging(ILoggerFactory loggerFactory) => this.Logger = loggerFactory.CreateLogger<StarterPlugin>();

    public override ILogger Logger { get; set; }

    public override Task BackgroundTask() => Task.Run(() => {
        Logger.LogInformation("Hello from the background task!");
    });

    public override Task<PluginHealthCheckResponse> PluginHealthCheckResponse() => Task<PluginHealthCheckResponse>.Run(() => {
        return new MessageFormats.Common.PluginHealthCheckResponse {
            ResponseHeader = new MessageFormats.Common.ResponseHeader {
                CorrelationId = Guid.NewGuid().ToString(),
                TrackingId = Guid.NewGuid().ToString(),
                Status = MessageFormats.Common.StatusCodes.Healthy,
                Message = "Hello from the plugin!"
            },
        };
    });

    public override Task<(DeployRequest?, DeployResponse?)> PreKubernetesDeployment(DeployRequest? input_request, DeployResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a PreKubernetesDeployment Event");
        return (input_request, input_response);
    });

    public override Task<(DeployRequest?, DeployResponse?)> PostKubernetesDeployment(DeployRequest? input_request, DeployResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a PostKubernetesDeployment Event");
        return (input_request, input_response);
    });

    public override Task<StringValue?> ProcessScheduleFile(StringValue? input_request) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a ProcessScheduleFile Event");
        return (input_request ?? null);
    });
}
