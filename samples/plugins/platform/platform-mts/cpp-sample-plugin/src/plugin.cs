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

    [DllImport("ImageProcessor_lib", EntryPoint = "ProcessImageC")]
    private static extern IntPtr ProcessImageC(string inputImagePath, string outputImagePath);

    public override void ConfigureLogging(ILoggerFactory loggerFactory) => this.Logger = loggerFactory.CreateLogger<CppSamplePlugin>();

    public override ILogger Logger { get; set; }

    public override Task BackgroundTask() => Task.CompletedTask;

    public override Task<LinkResponse?> LinkResponse(LinkResponse? input_response) => Task.Run(() => {
        // Get the type of link response
        MessageFormats.Common.StatusCodes? status = input_response?.ResponseHeader?.Status;
        Logger.LogInformation($"Plugin received a LinkResponse Event with status: {status}");

        // If the link response is successful and not a downlink, process the image
        if (status == MessageFormats.Common.StatusCodes.Successful && input_response?.LinkRequest.LinkType != MessageFormats.HostServices.Link.LinkRequest.Types.LinkType.Downlink) {
            // Get the image path from the link response
            string filename = Path.GetFileName(input_response?.LinkRequest?.FileName ?? string.Empty);

            // If the filename is empty, return
            if (string.IsNullOrEmpty(filename)) {
                Logger.LogError("Plugin received a LinkResponse Event with an empty filename. No image processing was performed.");
                return (input_response ?? null);
            }

            // Wait for the linkResponse file to be present in the outbox directory
            // This file is named {filename}.linkResponse
            (string inbox, string outbox, string root) = Core.GetXFerDirectories().Result;
            string linkResponsePath = Path.Combine(inbox, $"{filename}.linkResponse");

            // Wait for the file to be present in the outbox directory
            int timeout = 10000;  // 10 seconds
            DateTime start = DateTime.Now;
            while (!File.Exists(linkResponsePath) && (DateTime.Now - start).TotalMilliseconds < timeout) {
                Thread.Sleep(100);
            }

            if (!File.Exists(linkResponsePath)) {
                Logger.LogError($"Plugin did not find the linkResponse file in the outbox directory: {linkResponsePath}");
                return (input_response ?? null);
            }

            // If the data is a valid image path, process the image
            string inImagePath = Path.Combine(inbox, filename);
            if (inImagePath != null && File.Exists(inImagePath)) {
                // Process the image, overwriting the original image
                Logger.LogInformation($"Plugin processing a LinkResponse image: {inImagePath}");

                // Create a new image path for the processed image
                string outImagePath = Path.Combine(outbox, filename);
                string? result = ProcessImage(inImagePath, outImagePath);
                if (result != null) {
                    Logger.LogInformation($"Plugin processed a LinkResponse image successfully: {result}");
                }

                // Delete the original image and the linkResponse file
                File.Delete(inImagePath);
                File.Delete(linkResponsePath);
            } else {
                Logger.LogInformation("Plugin received a LinkResponse Event that did not contain a valid image path. No image processing was performed.");
            }
        }

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
