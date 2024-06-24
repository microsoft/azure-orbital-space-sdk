namespace Microsoft.Azure.SpaceFx.HostServices.Link.Plugins;
public class StarterPlugin : Microsoft.Azure.SpaceFx.HostServices.Link.Plugins.PluginBase {
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

    public override Task<LinkRequest?> LinkRequest(LinkRequest? input_request) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a LinkRequest");
        return (input_request);
    });

    public override Task<(LinkRequest?, LinkResponse?)> LinkResponse(LinkRequest? input_request, LinkResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a LinkResponse");
        return (input_request, input_response);
    });
}
