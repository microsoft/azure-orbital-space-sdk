using Microsoft.Azure.SpaceFx.MessageFormats.Common;
using Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor;

namespace DebugClient;

public class MessageSender : BackgroundService {
    private readonly ILogger<MessageSender> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly Core.Client _client;
    private readonly string _appId;
    private readonly string _hostSvcAppId;
    private readonly List<string> _appsOnline = new();
    private readonly TimeSpan MAX_TIMESPAN_TO_WAIT_FOR_MSG = TimeSpan.FromSeconds(10);

    public MessageSender(ILogger<MessageSender> logger, IServiceProvider serviceProvider) {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _client = _serviceProvider.GetService<Core.Client>() ?? throw new NullReferenceException($"{nameof(Core.Client)} is null");
        _appId = _client.GetAppID().Result;
        _hostSvcAppId = _appId.Replace("-client", "");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        DateTime maxTimeToWait = DateTime.Now.Add(TimeSpan.FromSeconds(10));

        using (var scope = _serviceProvider.CreateScope()) {
            _logger.LogInformation("MessageSender running at: {time}", DateTimeOffset.Now);

            Boolean SVC_ONLINE = _client.ServicesOnline().Any(pulse => pulse.AppId.Equals(_hostSvcAppId, StringComparison.CurrentCultureIgnoreCase));

            _logger.LogInformation($"Waiting for service '{_hostSvcAppId}' to come online...");

            while (!SVC_ONLINE && DateTime.Now < maxTimeToWait) {
                await Task.Delay(1000);
                SVC_ONLINE = _client.ServicesOnline().Any(pulse => pulse.AppId.Equals(_hostSvcAppId, StringComparison.CurrentCultureIgnoreCase));
                ListHeardServices();
            }

            if (!SVC_ONLINE) {
                throw new Exception($"Service '{_hostSvcAppId}' did not come online in time.");
            }

            await RegisterForSensorData();
            await SendPluginHealthCheck();
            await SendSensorsAvailableRequest();
            await SendTaskingPreCheckRequest();
            await SendTaskingRequest();

            _logger.LogInformation("DebugPayloadApp completed at: {time}", DateTimeOffset.Now);
        }
    }

    private void ListHeardServices() {
        _client.ServicesOnline().ForEach((pulse) => {
            if (_appsOnline.Contains(pulse.AppId)) return;
            _appsOnline.Add(pulse.AppId);
            _logger.LogInformation($"App:...{pulse.AppId}...");
        });
    }

    private async Task SendPluginHealthCheck() {
        DateTime maxTimeToWait = DateTime.Now.Add(TimeSpan.FromSeconds(10));
        PluginHealthCheckMultiResponse? response = null;
        PluginHealthCheckRequest request = new() {
            RequestHeader = new() {
                TrackingId = Guid.NewGuid().ToString(),
                CorrelationId = Guid.NewGuid().ToString()
            }
        };

        _logger.LogInformation($"Sending Plugin Healthcheck request (TrackingId: '{request.RequestHeader.TrackingId}')");

        // Register a callback event to catch the response
        void PluginResponseEventHandler(object? _, PluginHealthCheckMultiResponse _response) {
            response = _response;
            MessageHandler<PluginHealthCheckMultiResponse>.MessageReceivedEvent -= PluginResponseEventHandler;
        }

        MessageHandler<PluginHealthCheckMultiResponse>.MessageReceivedEvent += PluginResponseEventHandler;

        await _client.DirectToApp(appId: _hostSvcAppId, message: request);

        _logger.LogInformation($"Waiting for response (TrackingId: '{request.RequestHeader.TrackingId}')");

        while (response == null && DateTime.Now <= maxTimeToWait) {
            Thread.Sleep(100);
        }

        if (response == null) throw new TimeoutException($"Failed to hear {nameof(response)} after {MAX_TIMESPAN_TO_WAIT_FOR_MSG}.  Please check that {_hostSvcAppId} is deployed");

        if (response.ResponseHeader.Status != Microsoft.Azure.SpaceFx.MessageFormats.Common.StatusCodes.Successful) {
            throw new Exception($"Plugin Health Check failed with status '{response.ResponseHeader.Status}' and message '{response.ResponseHeader.Message}'");
        }


        _logger.LogInformation($"Plugin Healthcheck request received.  Status: '{response.ResponseHeader.Status}' (TrackingId: '{request.RequestHeader.TrackingId}')");

    }

    private async Task RegisterForSensorData() {

        _logger.LogInformation($"Registering a function to process Sensor Data");

        // Register a callback event to catch the Sensor Data
        void SensorDataEventHandler(object? _, SensorData _response) {
            _logger.LogInformation($"Received Sensor Data: '{_response.SensorID}'");
            // Do something with data
        }

        MessageHandler<SensorData>.MessageReceivedEvent += SensorDataEventHandler;

        _logger.LogInformation($"Successfully registered a function to process any Sensor Data");

    }

    private async Task SendSensorsAvailableRequest() {
        DateTime maxTimeToWait = DateTime.Now.Add(TimeSpan.FromSeconds(10));
        SensorsAvailableResponse? response = null;
        SensorsAvailableRequest request = new() {
            RequestHeader = new() {
                TrackingId = Guid.NewGuid().ToString(),
                CorrelationId = Guid.NewGuid().ToString()
            }
        };

        _logger.LogInformation($"Sending '{request.GetType().Name}' request (TrackingId: '{request.RequestHeader.TrackingId}')");

        // Register a callback event to catch the response
        void ResponseEventHandler(object? _, SensorsAvailableResponse _response) {
            if (_response.ResponseHeader.CorrelationId != request.RequestHeader.CorrelationId) return;
            response = _response;
            MessageHandler<SensorsAvailableResponse>.MessageReceivedEvent -= ResponseEventHandler;
        }

        MessageHandler<SensorsAvailableResponse>.MessageReceivedEvent += ResponseEventHandler;

        await _client.DirectToApp(appId: _hostSvcAppId, message: request);

        _logger.LogInformation($"Waiting for response (TrackingId: '{request.RequestHeader.TrackingId}')");

        while (response == null && DateTime.Now <= maxTimeToWait) {
            Thread.Sleep(100);
        }

        if (response == null) throw new TimeoutException($"Failed to hear {nameof(response)} after {MAX_TIMESPAN_TO_WAIT_FOR_MSG}.  Please check that {_hostSvcAppId} is deployed");

        _logger.LogInformation($"'{request.GetType().Name}' request received.  Status: '{response.ResponseHeader.Status}' (TrackingId: '{request.RequestHeader.TrackingId}')");

    }

    private async Task SendTaskingPreCheckRequest() {
        DateTime maxTimeToWait = DateTime.Now.Add(TimeSpan.FromSeconds(10));
        TaskingPreCheckResponse? response = null;
        TaskingPreCheckRequest request = new() {
            RequestHeader = new() {
                TrackingId = Guid.NewGuid().ToString(),
                CorrelationId = Guid.NewGuid().ToString()
            }
        };

        _logger.LogInformation($"Sending '{request.GetType().Name}' request (TrackingId: '{request.RequestHeader.TrackingId}')");

        // Register a callback event to catch the response
        void ResponseEventHandler(object? _, TaskingPreCheckResponse _response) {
            if (_response.ResponseHeader.CorrelationId != request.RequestHeader.CorrelationId) return;
            response = _response;
            MessageHandler<TaskingPreCheckResponse>.MessageReceivedEvent -= ResponseEventHandler;
        }

        MessageHandler<TaskingPreCheckResponse>.MessageReceivedEvent += ResponseEventHandler;

        await _client.DirectToApp(appId: _hostSvcAppId, message: request);

        _logger.LogInformation($"Waiting for response (TrackingId: '{request.RequestHeader.TrackingId}')");

        while (response == null && DateTime.Now <= maxTimeToWait) {
            Thread.Sleep(100);
        }

        if (response == null) throw new TimeoutException($"Failed to hear {nameof(response)} after {MAX_TIMESPAN_TO_WAIT_FOR_MSG}.  Please check that {_hostSvcAppId} is deployed");

        _logger.LogInformation($"'{request.GetType().Name}' request received.  Status: '{response.ResponseHeader.Status}' (TrackingId: '{request.RequestHeader.TrackingId}')");

    }

    private async Task SendTaskingRequest() {
        DateTime maxTimeToWait = DateTime.Now.Add(TimeSpan.FromSeconds(10));
        TaskingResponse? response = null;
        TaskingRequest request = new() {
            RequestHeader = new() {
                TrackingId = Guid.NewGuid().ToString(),
                CorrelationId = Guid.NewGuid().ToString()
            }
        };

        _logger.LogInformation($"Sending '{request.GetType().Name}' request (TrackingId: '{request.RequestHeader.TrackingId}')");

        // Register a callback event to catch the response
        void ResponseEventHandler(object? _, TaskingResponse _response) {
            if (_response.ResponseHeader.CorrelationId != request.RequestHeader.CorrelationId) return;
            response = _response;
            MessageHandler<TaskingResponse>.MessageReceivedEvent -= ResponseEventHandler;
        }

        MessageHandler<TaskingResponse>.MessageReceivedEvent += ResponseEventHandler;

        await _client.DirectToApp(appId: _hostSvcAppId, message: request);

        _logger.LogInformation($"Waiting for response (TrackingId: '{request.RequestHeader.TrackingId}')");

        while (response == null && DateTime.Now <= maxTimeToWait) {
            Thread.Sleep(100);
        }

        if (response == null) throw new TimeoutException($"Failed to hear {nameof(response)} after {MAX_TIMESPAN_TO_WAIT_FOR_MSG}.  Please check that {_hostSvcAppId} is deployed");

        _logger.LogInformation($"'{request.GetType().Name}' request received.  Status: '{response.ResponseHeader.Status}' (TrackingId: '{request.RequestHeader.TrackingId}')");

    }
}