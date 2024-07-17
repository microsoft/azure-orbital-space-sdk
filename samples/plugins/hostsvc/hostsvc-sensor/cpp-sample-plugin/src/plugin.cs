namespace Microsoft.Azure.SpaceFx.HostServices.Sensor.Plugins;
public class StarterPlugin : Microsoft.Azure.SpaceFx.HostServices.Sensor.Plugins.PluginBase {

    // Temperature sensor simulates a broadcast sensor to validate the unknown destination scenario works
    const string SENSOR_TEMPERATURE_ID = "DemoTemperatureSensor";

    // HelloWorld sensor is a simple request/reply sensor to validate the direct path scenario works
    const string SENSOR_ID = "DemoHelloWorldSensor";
    readonly ConcurrentBag<string> CLIENT_IDS = new();
    public StarterPlugin() {
        LoggerFactory loggerFactory = new();
        this.Logger = loggerFactory.CreateLogger<StarterPlugin>();
    }

    public override void ConfigureLogging(ILoggerFactory loggerFactory) => this.Logger = loggerFactory.CreateLogger<StarterPlugin>();

    public override ILogger Logger { get; set; }

    public override Task BackgroundTask() => Task.Run(async () => {
        Random random_num_generator = new Random();
        int temperatureReading = 0;
        string? clientID;

        // Generate some fake Sensor Data every second after a tasking request.
        while (true) {
            // Loop through the client IDs we received for sensor data and send it out on direct path
            while (CLIENT_IDS.TryTake(out clientID)) {
                SensorData sensorData = new() {
                    ResponseHeader = new() {
                        TrackingId = Guid.NewGuid().ToString(),
                        CorrelationId = Guid.NewGuid().ToString(),
                        Status = StatusCodes.Successful,
                        AppId = clientID
                    },
                    DestinationAppId = clientID,
                    SensorID = SENSOR_ID,
                    GeneratedTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow),
                    ExpirationTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.MaxValue.ToUniversalTime()),
                    Data = Google.Protobuf.WellKnownTypes.Any.Pack(new Google.Protobuf.WellKnownTypes.StringValue() { Value = "Hello Space World!" })
                };

                Logger.LogInformation("Sending SensorData '{sensor}' to {clientId}", SENSOR_ID, clientID);

                // Route the message back to the Sensor Service so it looks like it came in as request sensor data
                await Core.DirectToApp(Core.GetAppID().Result, sensorData);
            }


            // Generate a fake sensor and send it out without any destination (unknown destination user scenario)
            temperatureReading = random_num_generator.Next(10, 50);

            SensorData temperatureProbe = new() {
                ResponseHeader = new() {
                    TrackingId = Guid.NewGuid().ToString(),
                },
                SensorID = SENSOR_TEMPERATURE_ID,
                Data = Google.Protobuf.WellKnownTypes.Any.Pack(new Google.Protobuf.WellKnownTypes.StringValue() { Value = $"Temperature: {temperatureReading}" })
            };

            Logger.LogInformation("Sending SensorData '{sensor}'", SENSOR_TEMPERATURE_ID);

            // Route the message back to the Sensor Service so it looks like it came in as request sensor data
            await Core.DirectToApp(Core.GetAppID().Result, temperatureProbe);

            await Task.Delay(1000);
        }
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

        if (input_request == null || input_response == null) return (input_request, input_response);

        input_response.ResponseHeader.Status = StatusCodes.Successful;
        input_response.Sensors.Add(new SensorsAvailableResponse.Types.SensorAvailable() { SensorID = SENSOR_ID });
        input_response.Sensors.Add(new SensorsAvailableResponse.Types.SensorAvailable() { SensorID = SENSOR_TEMPERATURE_ID });


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

        // Flip it to success
        input_response.ResponseHeader.Status = StatusCodes.Successful;
        input_response.SensorID = input_request.SensorID;

        // Add the client ID to the list so we can direct send it Sensor Data
        if (!CLIENT_IDS.Contains(input_request.RequestHeader.AppId))
            CLIENT_IDS.Add(input_request.RequestHeader.AppId);


        return (input_request, input_response);
    });

    public override Task<SensorData?> SensorData(SensorData? input_request) => Task.Run(() => {
        Logger.LogInformation("Plugin received and processed a SensorData Event");
        return (input_request ?? null);
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

}
