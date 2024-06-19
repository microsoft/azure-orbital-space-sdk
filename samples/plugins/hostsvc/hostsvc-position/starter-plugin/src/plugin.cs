namespace Microsoft.Azure.SpaceFx.HostServices.Position.Plugins;
public class StarterPlugin : Microsoft.Azure.SpaceFx.HostServices.Position.Plugins.PluginBase {

    public StarterPlugin() {
        LoggerFactory loggerFactory = new();
        this.Logger = loggerFactory.CreateLogger<StarterPlugin>();
    }

    public override ILogger Logger { get; set; }

    public override Task BackgroundTask() => Task.Run(() => Console.WriteLine("Successfully started plugin background task."));

    public override void ConfigureLogging(ILoggerFactory loggerFactory) => this.Logger = loggerFactory.CreateLogger<StarterPlugin>();

    public override Task<PluginHealthCheckResponse> PluginHealthCheckResponse() => Task.FromResult<PluginHealthCheckResponse>(new MessageFormats.Common.PluginHealthCheckResponse());

    public override Task<(PositionRequest?, PositionResponse?)> PositionRequest(PositionRequest? input_request, PositionResponse? input_response) => Task.Run(() => {
        return (input_request, input_response);
    });

    public override Task<(PositionUpdateRequest?, PositionUpdateResponse?)> PositionUpdateRequest(PositionUpdateRequest? input_request, PositionUpdateResponse? input_response) => Task.Run(() => {
        return (input_request, input_response);
    });
}
