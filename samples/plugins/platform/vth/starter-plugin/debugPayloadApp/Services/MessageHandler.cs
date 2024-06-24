namespace DebugClient;

public class MessageHandler<T> : Core.IMessageHandler<T> where T : notnull {
    private readonly ILogger<MessageHandler<T>> _logger;
    private readonly IServiceProvider _serviceProvider;
    public static event EventHandler<T>? MessageReceivedEvent;
    public MessageHandler(ILogger<MessageHandler<T>> logger, IServiceProvider serviceProvider) {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public void MessageReceived(T message, Microsoft.Azure.SpaceFx.MessageFormats.Common.DirectToApp fullMessage) {
        using (var scope = _serviceProvider.CreateScope()) {

            _logger.LogInformation($"Receieved message type '{typeof(T).Name}' from '{fullMessage.SourceAppId}'");

            if (MessageReceivedEvent != null) {
                foreach (Delegate handler in MessageReceivedEvent.GetInvocationList()) {
                    Task.Factory.StartNew(
                        () => handler.DynamicInvoke(fullMessage.SourceAppId, message));
                }
            }
        }
    }
}