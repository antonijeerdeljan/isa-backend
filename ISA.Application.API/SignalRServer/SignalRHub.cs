using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ISA.Application.API.SignalRServer;

public class SignalRHub : Hub
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");

        var userId = Guid.Parse(Context.User.Claims.First(x => x.Type == "id").Value);
        ConnectionMapping.Add(userId, Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Guid.Parse(Context.User.Claims.First(x => x.Type == "id").Value);
        ConnectionMapping.Remove(userId);

        await base.OnDisconnectedAsync(exception);
    }
}
