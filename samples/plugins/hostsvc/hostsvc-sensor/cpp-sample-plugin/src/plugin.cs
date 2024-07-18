namespace Microsoft.Azure.SpaceFx.HostServices.Sensor.Plugins;
public class CppExamplePlugin : Microsoft.Azure.SpaceFx.HostServices.Sensor.Plugins.PluginBase {
    // AstronautCam sensor is a simple request/reply sensor
    // For this sensor this plugin will use a C++ library to process an image of an astronaut
    // The C++ library is located in the ImageProcessor.dll file
    const string SENSOR_ASTRONAUT_CAM_ID = "DemoAstronautCam";

    readonly ConcurrentBag<string> CLIENT_IDS = new();
    public CppExamplePlugin() {
        LoggerFactory loggerFactory = new();
        this.Logger = loggerFactory.CreateLogger<CppExamplePlugin>();
    }

    public override void ConfigureLogging(ILoggerFactory loggerFactory) => this.Logger = loggerFactory.CreateLogger<CppExamplePlugin>();

    public override ILogger Logger { get; set; }

    public override Task BackgroundTask() => Task.Run(async () => {
        // Start the astronaut cam task
        var astronautCamTask = AstronautCamTask();

        // Optionally wait for the task to complete (if needed)
        await Task.WhenAll(astronautCamTask);
    });

    private Task AstronautCamTask() => Task.Run(async () => {
        string? clientID;

        while (true) {
            // Loop through the client IDs we received for sensor data and send it out on direct path
            while (CLIENT_IDS.TryTake(out clientID)) {
                Logger.LogInformation("Processing image for {clientId}", clientID);

                // Process the astronaut image using the C++ library
                string? greyscaleImagePath = ProcessImage("/workspace/hostsvc-sensor-cpp-plugin-sample/img/astronaut.jpg");

                if (greyscaleImagePath == null) {
                    Logger.LogError("Failed to process the image");
                    return;
                }
                Logger.LogInformation("Image processed successfully");

                SensorData sensorData = new() {
                    ResponseHeader = new() {
                        TrackingId = Guid.NewGuid().ToString(),
                        CorrelationId = Guid.NewGuid().ToString(),
                        Status = StatusCodes.Successful,
                        AppId = clientID
                    },
                    DestinationAppId = clientID,
                    SensorID = SENSOR_ASTRONAUT_CAM_ID,
                    GeneratedTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow),
                    ExpirationTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.MaxValue.ToUniversalTime()),
                    Data = Google.Protobuf.WellKnownTypes.Any.Pack(new Google.Protobuf.WellKnownTypes.StringValue() { Value = greyscaleImagePath })
                };

                Logger.LogInformation("Sending SensorData '{sensor}' to {clientId}", SENSOR_ASTRONAUT_CAM_ID, clientID);

                // Route the message back to the Sensor Service so it looks like it came in as request sensor data
                await Core.DirectToApp(Core.GetAppID().Result, sensorData);
            }

            await Task.Delay(100);
        }
    });

    [DllImport("ImageProcessor", EntryPoint = "ProcessImageC")]
    private static extern IntPtr ProcessImageC(string imagePath);

    public static string? ProcessImage(string imagePath) {
        IntPtr resultPtr = ProcessImageC(imagePath);
        if (resultPtr == IntPtr.Zero) {
            return null;
        }

        string? result = Marshal.PtrToStringAnsi(resultPtr);
        return result;
    }

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

        if (input_request == null || input_response == null) return (input_request, input_response);

        input_response.ResponseHeader.Status = StatusCodes.Successful;
        input_response.Sensors.Add(new SensorsAvailableResponse.Types.SensorAvailable() { SensorID = SENSOR_ASTRONAUT_CAM_ID });


        return (input_request, input_response);
    });

    public override Task<TaskingPreCheckRequest?> TaskingPreCheckRequest(TaskingPreCheckRequest? input_request) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a TaskingPreCheckRequest Event");
        return (input_request ?? null);
    });

    public override Task<(TaskingPreCheckRequest?, TaskingPreCheckResponse?)> TaskingPreCheckResponse(TaskingPreCheckRequest? input_request, TaskingPreCheckResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a TaskingPreCheckResponse Event");
        if (input_request == null || input_response == null) return (input_request, input_response);

        // Flip it to success
        input_response.ResponseHeader.Status = StatusCodes.Successful;
        return (input_request, input_response);
    });

    public override Task<TaskingRequest?> TaskingRequest(TaskingRequest? input_request) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a TaskingRequest Event");
        return (input_request ?? null);
    });

    public override Task<(TaskingRequest?, TaskingResponse?)> TaskingResponse(TaskingRequest? input_request, TaskingResponse? input_response) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a TaskingResponse Event");
        if (input_request == null || input_response == null) return (input_request, input_response);

        if (input_request.SensorID != SENSOR_ASTRONAUT_CAM_ID) {
            Logger.LogInformation("Tasking requested for unknown sensor: {sensorId}", input_request.SensorID);
            input_response.ResponseHeader.Status = StatusCodes.NotFound;
        } else {
            Logger.LogInformation("Tasking requested for {sensorId}", SENSOR_ASTRONAUT_CAM_ID);
            input_response.ResponseHeader.Status = StatusCodes.Successful;
        }
        input_response.SensorID = input_request.SensorID;

        // Add the client ID to the list so we can direct send it Sensor Data
        if (!CLIENT_IDS.Contains(input_request.RequestHeader.AppId))
            CLIENT_IDS.Add(input_request.RequestHeader.AppId);


        return (input_request, input_response);
    });

    public override Task<SensorData?> SensorData(SensorData? input_request) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a SensorData Event");

        if (input_request?.SensorID == SENSOR_ASTRONAUT_CAM_ID) {
            Logger.LogInformation("AstronautCam Sensor Data: {data}", input_request.Data.Unpack<Google.Protobuf.WellKnownTypes.StringValue>().Value);
        } else {
            Logger.LogInformation("Recieved SensorData from Unknown Sensor: {sensorId}", input_request?.SensorID);
        }

        return (input_request ?? null);
    });

}
