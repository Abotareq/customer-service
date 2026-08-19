using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CustomerService.Infrastructure.Notifications;

[Authorize]
public sealed class RequestHub : Hub
{
    public async Task JoinRequestGroup(Guid requestId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, requestId.ToString());
    }

    public async Task LeaveRequestGroup(Guid requestId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, requestId.ToString());
    }
}