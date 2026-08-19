using CustomerService.Application.Common.Interfaces;
using CustomerService.Application.Common.Interfaces.Message;
using CustomerService.Contracts.Messages;
using Microsoft.AspNetCore.SignalR;

namespace CustomerService.Infrastructure.Notifications;

public sealed class MessageNotifier : IMessageNotifier
{
    private readonly IHubContext<RequestHub> _hubContext;

    public MessageNotifier(IHubContext<RequestHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyNewMessageAsync(
        Guid requestId, MessageResponse message, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine($"[SignalR] Broadcasting to group {requestId}...");

            await _hubContext.Clients
                .Group(requestId.ToString())
                .SendAsync("ReceiveMessage", message, cancellationToken);

            Console.WriteLine("[SignalR] Broadcast completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SignalR] Broadcast FAILED: {ex}");
        }
    }
}