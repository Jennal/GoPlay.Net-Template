using Common;
using GoPlay.Services.Core.Attributes;
using GoPlay.Services.Core.Protocols;

namespace Processor.Echo;

[ServerTag(ServerTag.FrontEnd)]
[Processor("echo")]
public class EchoProcessor : Common.Processor
{
    public override string[] Pushes => new[]
    {
        "echo.push"
    };

    private static HashSet<uint> s_clientIds = new();

    public override void OnClientConnected(uint clientId)
    {
        s_clientIds.Add(clientId);
    }

    public override void OnClientDisconnected(uint clientId)
    {
        s_clientIds.Remove(clientId);
    }

    [Request("request")]
    public PbString Request(Header header, PbString request)
    {
        var name = GetUserName(header);
        return new PbString
        {
            Value = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {name}: {request.Value}",
        };
    }
    
    [Notify("push.all")]
    public void PushAll(Header header, PbString request)
    {
        var name = GetUserName(header);
        var push = new PbString
        {
            Value = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {name}: {request.Value}",
        };

        foreach (var clientId in s_clientIds)
        {
            Push("echo.push", clientId, push);
        }
    }
}
