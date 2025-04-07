
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace OA2MCP_TestMe;

public class myServerAddress : IHostedService
{
    private readonly IServer server;
    private readonly IHostApplicationLifetime hostApplicationLifetime;

    public myServerAddress(IServer server, IHostApplicationLifetime hostApplicationLifetime)
    {
        this.server = server;
        this.hostApplicationLifetime = hostApplicationLifetime;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        hostApplicationLifetime.ApplicationStarted.Register(
              () => GetAddresses());
        return Task.CompletedTask;
    }
    private void GetAddresses()
    {
        var feature = server.Features.Get<IServerAddressesFeature>();
        if(feature == null)
            return;
        var theAddresses= feature.Addresses;
        if(theAddresses == null)
            return;
        Console.WriteLine("Addresses:"+ string.Join(",", theAddresses));
        OpenApi2MCP.MCPTools_localSwagger.SetAdresses(theAddresses.ToArray());
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
