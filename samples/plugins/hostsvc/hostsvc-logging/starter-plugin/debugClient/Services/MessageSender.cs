namespace PayloadApp.DebugClient;

public class MessageSender : BackgroundService
{
    private readonly ILogger<MessageSender> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly Microsoft.Azure.SpaceFx.Core.Client _client;
    private readonly string _appId;
    private readonly string _hostSvcAppId;

    public MessageSender(ILogger<MessageSender> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _client = _serviceProvider.GetService<Microsoft.Azure.SpaceFx.Core.Client>() ?? throw new NullReferenceException($"{nameof(Microsoft.Azure.SpaceFx.Core.Client)} is null");
        _appId = _client.GetAppID().Result;
        _hostSvcAppId = _appId.Replace("-client", "");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            _logger.LogInformation("MessageSender running at: {time}", DateTimeOffset.Now);

            System.Threading.Thread.Sleep(3000);

            Boolean SVC_ONLINE = _client.ServicesOnline().Any(pulse => pulse.AppId.Equals(_hostSvcAppId, StringComparison.CurrentCultureIgnoreCase));


            // while (!SVC_ONLINE) {
            //     ListHeardServices();
            //     SVC_ONLINE = _client.ServicesOnline().Any(pulse => pulse.AppId.Equals(_hostSvcAppId, StringComparison.CurrentCultureIgnoreCase));
            // }

            await UpdatePosition();

            await PositionRequest();
        }
    }

    public void ListHeardServices()
    {
        System.Threading.Thread.Sleep(250);
        _logger.LogInformation("Apps Running: ");
        var activeServices = _client.ServicesOnline();
        activeServices.ForEach((pulse) => _logger.LogInformation($"...{pulse.AppId}..."));
    }

    public async Task UpdatePosition()
    {
        _logger.LogInformation("Sending Position Request:");
        Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Logging.PositionUpdateRequest request = new()
        {
            RequestHeader = new()
            {
                TrackingId = Guid.NewGuid().ToString(),
                CorrelationId = Guid.NewGuid().ToString()
            },
            Position = new Position()
            {
                PositionTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow),
                Point = new Position.Types.Point()
                {
                    X = 1,
                    Y = 2,
                    Z = 3,
                },
                Attitude = new Position.Types.Attitude()
                {
                    X = 1,
                    Y = 2,
                    Z = 3,
                    K = 4
                }
            }
        };

        await _client.DirectToApp(_hostSvcAppId, request);
    }

    public async Task PositionRequest()
    {
        _logger.LogInformation("Requesting Position:");
        Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Logging.PositionRequest request = new()
        {
            RequestHeader = new()
            {
                TrackingId = Guid.NewGuid().ToString()
            }
        };

        await _client.DirectToApp(appId: _hostSvcAppId, message: request);
    }
}
