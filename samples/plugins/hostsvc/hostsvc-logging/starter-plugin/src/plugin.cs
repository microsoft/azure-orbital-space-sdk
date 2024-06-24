namespace Microsoft.Azure.SpaceFx.HostServices.Logging.Plugins;
public class StarterPlugin : Microsoft.Azure.SpaceFx.HostServices.Logging.Plugins.PluginBase {

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

    public override Task<(LogMessage?, LogMessageResponse?)> LogMessageReceived(LogMessage? input_request, LogMessageResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a LogMessaageReceived Event");
        return (input_request, input_response);
    });


    public override Task<TelemetryMetric?> TelemetryMetricReceived(TelemetryMetric input_request) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a TelemetryMetricReceived Event");
        return (input_request ?? null);
    });

    public override Task<(TelemetryMetric?, TelemetryMetricResponse?)> TelemetryMetricResponse(TelemetryMetric? input_request, TelemetryMetricResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a TelemetryMetricResponse Event");
        return (input_request, input_response);
    });


    public override Task<(LogMessage?, string?)> PreWriteToLog(LogMessage? input_request, string? fileName) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a PreWriteToLog Event");
        return (input_request, fileName);
    });

    public override Task<(LogMessage?, string?)> PostWriteToLog(LogMessage? input_request, string? fileName) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a PostWriteToLog Event");
        return (input_request, fileName);
    });



}