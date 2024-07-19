namespace Microsoft.Azure.SpaceFx.PlatformServices.MessageTranslationService.Plugins;
public class CppSamplePlugin : Microsoft.Azure.SpaceFx.PlatformServices.MessageTranslationService.Plugins.PluginBase {
    public CppSamplePlugin() {
        LoggerFactory loggerFactory = new();
        this.Logger = loggerFactory.CreateLogger<CppSamplePlugin>();
    }

    // Access the ProcessImage functionality from the ImageProcessor C++ library
    public static string? ProcessImage(string inputImagePath, string outputImagePath) {
        IntPtr resultPtr = ProcessImageC(inputImagePath, outputImagePath);
        if (resultPtr == IntPtr.Zero) {
            return null;
        }

        string? result = Marshal.PtrToStringAnsi(resultPtr);
        return result;
    }

    [DllImport("ImageProcessor", EntryPoint = "ProcessImageC")]
    private static extern IntPtr ProcessImageC(string inputImagePath, string outputImagePath);

    public override void ConfigureLogging(ILoggerFactory loggerFactory) => this.Logger = loggerFactory.CreateLogger<CppSamplePlugin>();

    public override ILogger Logger { get; set; }

    public override Task BackgroundTask() => Task.CompletedTask;

    public override Task<LinkResponse?> LinkResponse(LinkResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a LinkResponse Event");
        return (input_response ?? null);
    });

    public override Task<(PositionUpdateRequest?, PositionUpdateResponse?)> PositionUpdateRequest(PositionUpdateRequest? input_request, PositionUpdateResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a PositionUpdateRequest Event");
        return (input_request, input_response);
    });

    public override Task<PositionUpdateResponse?> PositionUpdateResponse(PositionUpdateResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a PositionUpdateResponse Event");
        return (input_response ?? null);
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

    public override Task<SensorData?> SensorData(SensorData? input_request) => Task.Run(() => {
        Logger.LogInformation("Plugin received a SensorData Event");

        // Get the Data attribute from the SensorData object
        string? imagePath = input_request?.Data?.Value?.ToString();

        // If the data is a valid image path, process the image
        if (imagePath != null && File.Exists(imagePath)) {
            // Process the image, overwriting the original image
            Logger.LogInformation($"Plugin processing a SensorData image: {imagePath}");
            string? result = ProcessImage(imagePath, imagePath);
            if (result != null) {
                Logger.LogInformation($"Plugin processed a SensorData image successfully: {result}");
            }
        } else {
            Logger.LogInformation("Plugin received a SensorData Event that did not contain a valid image path. No image processing was performed.");
        }

        return (input_request ?? null);
    });

    public override Task<(SensorsAvailableRequest?, SensorsAvailableResponse?)> SensorsAvailableRequest(SensorsAvailableRequest? input_request, SensorsAvailableResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a SensorsAvailableRequest Event");
        return (input_request, input_response);
    });

    public override Task<SensorsAvailableResponse?> SensorsAvailableResponse(SensorsAvailableResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a SensorsAvailableResponse Event");
        return (input_response ?? null);
    });

    public override Task<(TaskingPreCheckRequest?, TaskingPreCheckResponse?)> TaskingPreCheckRequest(TaskingPreCheckRequest? input_request, TaskingPreCheckResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a TaskingPreCheckRequest Event");
        return (input_request, input_response);
    });

    public override Task<TaskingPreCheckResponse?> TaskingPreCheckResponse(TaskingPreCheckResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a TaskingPreCheckResponse Event");
        return (input_response ?? null);
    });

    public override Task<(TaskingRequest?, TaskingResponse?)> TaskingRequest(TaskingRequest? input_request, TaskingResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a TaskingRequest Event");
        return (input_request, input_response);
    });

    public override Task<TaskingResponse?> TaskingResponse(TaskingResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a TaskingResponse Event");
        return (input_response ?? null);
    });
}
