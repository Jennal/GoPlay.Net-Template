using Common;
using GoPlay.Services.Core.Attributes;
using GoPlay.Services.Core.Protocols;

namespace Processor.Echo;

[Processor("echo")]
public class EchoProcessor : Common.Processor
{
    public override string[] Pushes => new[]
    {
        "echo.push"
    };
    
    [Notify("echo")]
    public void Echo(Header header, PbString request)
    {
        var name = SessionManager.Get<PbString>(header.ClientId, Consts.SessionKeys.UserName);
        var push = new PbString
        {
            Value = request.Value
        };
    }
}
