namespace StarterApp {
    public class Program {
        private static Microsoft.Azure.SpaceFx.SDK.Client? Client = new Microsoft.Azure.SpaceFx.SDK.Client();
        private static ILogger Logger => Client.Logger;

        public static async Task Main() {
            Client.Build();
            Logger.LogInformation("Running DebugClient...");


            ListServicesOnline();

            await Microsoft.Azure.SpaceFx.SDK.Logging.SendLogMessage("Hello space world from DebugClient123!");


            ListenForSensorData();
            SendLogMessage();
            SendSensorsAvailableRequest();
            SendSensorTaskingPreCheckRequest();
            SendSensorTaskingRequest();

            SendFileToApp();

            GetLastKnownPosition();

            await Client.KeepAppOpen();
        }

        #region Heartbeats
        public static void ListServicesOnline() {
            // Services send out HeartBeats to let other apps know they are online.
            // We have to give enough time for heartbeats to come in before we check
            double heartbeatPulseTiming = double.Parse(Microsoft.Azure.SpaceFx.Core.GetConfigSetting("heartbeatpulsetimingms").Result);
            heartbeatPulseTiming = heartbeatPulseTiming * (double) 1.5;
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(heartbeatPulseTiming);

            Console.WriteLine($"Waiting for {timeSpan.TotalSeconds} seconds, then checking for services heard...");
            Thread.Sleep(timeSpan);

            List<HeartBeatPulse> heartBeats = Microsoft.Azure.SpaceFx.SDK.Client.ServicesOnline();

            heartBeats.ForEach((_heartBeat) => {
                Console.WriteLine($"Service Online: {_heartBeat.AppId}");
            });

            Console.WriteLine($"Total Services Online: {heartBeats.Count}");
        }

        #endregion

        #region PositionService
        public static void GetLastKnownPosition() {
            Task<PositionResponse> requestTask = Microsoft.Azure.SpaceFx.SDK.Position.LastKnownPosition();
            requestTask.Wait();

            PositionResponse response = requestTask.Result;

            Console.WriteLine($"Position response: {response.ResponseHeader.Status}");
        }

        #endregion

        #region SensorService
        public static void ListenForSensorData() {
            static void SensorDataEventHandler(object? _, SensorData sensorData) {
                Console.WriteLine($"Sensor Data: {sensorData.SensorID}");
            }

            Client.SensorDataEvent += SensorDataEventHandler;
        }

        public static void SendSensorsAvailableRequest() {
            Task<SensorsAvailableResponse> requestTask = Sensor.GetAvailableSensors();
            requestTask.Wait();
            SensorsAvailableResponse response = requestTask.Result;

            Logger.LogInformation($"Sensor response: {response.ResponseHeader.Status}");

            Logger.LogInformation("Available sensors:");
            response.Sensors.ToList().ForEach((sensor) => {
                Logger.LogInformation($"Sensor: {sensor.SensorID}");
            });

        }

        public static void SendSensorTaskingPreCheckRequest() {
            Task<TaskingPreCheckResponse> requestTask = Sensor.SensorTaskingPreCheck(sensorId: "someSensor");
            requestTask.Wait();

            TaskingPreCheckResponse response = requestTask.Result;

            Console.WriteLine($"Sensor Tasking PreCheck response: {response.ResponseHeader.Status}");
        }

        public static void SendSensorTaskingRequest() {

            Task<TaskingResponse> requestTask = Sensor.SensorTasking(sensorId: "someSensor");
            requestTask.Wait();

            TaskingResponse response = requestTask.Result;

            Console.WriteLine($"Sensor Tasking response: {response.ResponseHeader.Status}");
        }

        #endregion

        #region LinkService
        public static void SendFileToApp() {
            Task<LinkResponse> requestTask = Link.SendFileToApp(destinationAppId: "some-app", file: "/workspace/dotnet-starter-app/sampleData/astronaut.jpg", overwriteDestinationFile: true);
            requestTask.Wait();

            LinkResponse response = requestTask.Result;
            Console.WriteLine($"File uploaded response: {response.ResponseHeader.Status}.");
        }

        #endregion

        #region LoggingService
        public static void SendLogMessage() {
            Task<LogMessageResponse> logTask = Logging.SendLogMessage(logMessage: "Hello space world!");
            logTask.Wait();

            LogMessageResponse logResponse = logTask.Result;

            Console.WriteLine($"Logging response: {logResponse.ResponseHeader.Status}");
        }
        #endregion

    }
}