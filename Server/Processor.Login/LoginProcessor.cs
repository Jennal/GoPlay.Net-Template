using Common;
using Common.Protocols;
using GoPlay.Core.Attributes;
using GoPlay.Core.Protocols;

namespace Processor.Login;

[ServerTag(ServerTag.FrontEnd)]
[Processor("login")]
public class LoginProcessor : Common.Processor
{
    private static IdLoopGenerator _idLoopGenerator = new IdLoopGenerator();
    
    public override string[] Pushes => new[]
    {
        "login.new.user"
    };
    
    [BeforeLogin]
    [Request("login")]
    public LoginResponse Login(Header header, LoginRequest request)
    {
        return new LoginResponse
        {
            Id = (int)_idLoopGenerator.Next(),
            Username = request.Username,
        };
    }
    
    public override void OnPostSendResult(Package pack)
    {
        if (pack.Header.Status.Code != StatusCode.Success) return;
            
        if (pack is Package<LoginResponse> p)
        {
            SessionManager.Set(pack.Header.ClientId, Consts.SessionKeys.UserId, new PbInt
            {
                Value = p.Data.Id
            });
            SessionManager.Set(pack.Header.ClientId, Consts.SessionKeys.UserName, new PbString
            {
                Value = p.Data.Username
            });
            Server.Broadcast(pack.Header.ClientId, (int)EventId.LoginSuccess, p);
        }
    }
    
    public override void OnClientDisconnected(uint clientId)
    {
        var userId = GetUserId(clientId);
            
        SessionManager.Remove(clientId, Consts.SessionKeys.UserId);
        SessionManager.Remove(clientId, Consts.SessionKeys.UserName);

        if (userId == Consts.Id.INVALID_USER_ID) return;
        Server.Broadcast(clientId, (int)EventId.LogoutSuccess, userId);
    }
}
