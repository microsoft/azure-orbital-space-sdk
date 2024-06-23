
namespace Microsoft.Azure.SpaceFx.HostServices.Sensor.Plugins;
public class StarterPlugin : Microsoft.Azure.SpaceFx.HostServices.Sensor.Plugins.PluginBase {

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

    public override Task<SensorsAvailableRequest?> SensorsAvailableRequest(SensorsAvailableRequest? input_request) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a SensorsAvailableRequest Event");
        return (input_request ?? null);
    });

    public override Task<(SensorsAvailableRequest?, SensorsAvailableResponse?)> SensorsAvailableResponse(SensorsAvailableRequest? input_request, SensorsAvailableResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a SensorsAvailableResponse Event");
        return (input_request, input_response);
    });

    public override Task<TaskingPreCheckRequest?> TaskingPreCheckRequest(TaskingPreCheckRequest? input_request) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a TaskingPreCheckRequest Event");
        return (input_request ?? null);
    });

    public override Task<(TaskingPreCheckRequest?, TaskingPreCheckResponse?)> TaskingPreCheckResponse(TaskingPreCheckRequest? input_request, TaskingPreCheckResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a TaskingPreCheckResponse Event");
        return (input_request, input_response);
    });

    public override Task<TaskingRequest?> TaskingRequest(TaskingRequest? input_request) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a TaskingRequest Event");
        return (input_request ?? null);
    });

    public override Task<(TaskingRequest?, TaskingResponse?)> TaskingResponse(TaskingRequest? input_request, TaskingResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a TaskingResponse Event");
        return (input_request, input_response);
    });

    public override Task<SensorData?> SensorData(SensorData? input_request) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a SensorData Event");
        return (input_request ?? null);
    });
}
