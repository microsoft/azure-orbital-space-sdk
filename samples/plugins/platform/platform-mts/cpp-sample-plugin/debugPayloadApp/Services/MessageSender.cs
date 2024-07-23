using Microsoft.Azure.SpaceFx.MessageFormats.Common;
using Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Link;
using Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Position;
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
    private readonly string _testFile = "/workspace/platform-mts-cpp-sample-plugin/sampleData/astronaut.jpg";

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

            // Send astronaut.jpg to platform-mts, triggering its C++ plugin
            await SendAstronautFile();

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

    private async Task SendAstronautFile() {
        var (inbox, outbox, root) = _client.GetXFerDirectories().Result;

        _logger.LogInformation($"Sending '{Path.GetFileName(_testFile)}' to '{outbox}'");

        File.Copy(_testFile, string.Format($"{outbox}/{Path.GetFileName(_testFile)}"), overwrite: true);

        LinkRequest request = new() {
            RequestHeader = new() {
                TrackingId = Guid.NewGuid().ToString(),
                CorrelationId = Guid.NewGuid().ToString()
            },
            FileName = Path.GetFileName(_testFile),
            DestinationAppId = "platform-mts",
            Overwrite = true
        };

        _logger.LogInformation($"Sending '{request.GetType().Name}' request (TrackingId: '{request.RequestHeader.TrackingId}') (DestinationAppId: '{request.DestinationAppId}')");

        await _client.DirectToApp(appId: "hostsvc-link", message: request);
    }
}