using Common.Protocols;
using GoPlay.Services.Core.Attributes;
using GoPlay.Services.Core.Processors;
using GoPlay.Services.Core.Protocols;

namespace Common;

public abstract partial class Processor : ProcessorBase
{
    public override Package? OnPreRecv(Package pack)
    {
        var route = GetRoute(pack);
        var attrs = route.Method.GetCustomAttributes(typeof(BeforeLoginAttribute), true);
        if (attrs.Any()) return null;

        if (!IsLogined(pack.Header))
        {
            var header = pack.Header.Clone();
            header.ClientId = pack.Header.ClientId;
            header.PackageInfo.Type = PackageType.Response;
            header.Status = new Status
            {
                Code = StatusCode.Failed,
                Message = Consts.ErrCode.LOGIN_REQUIRED,
            };
            return new Package
            {
                Header = header
            };
        }

        return null;
    }

    protected virtual bool IsLogined(Header header)
    {
        var userId = GetUserId(header);
        return userId > 0;
    }

    protected virtual uint GetClientId(int userId)
    {
        return SessionManager.GetClientId(userId);
    }
    
    protected virtual List<uint> GetClientIds(int userId)
    {
        return SessionManager.GetClientIds(userId);
    }
    
    protected virtual int GetUserId(Header header)
    {
        return GetUserId(header.ClientId);
    }

    protected virtual int GetUserId(uint clientId)
    {
        var userId = SessionManager.Get<PbInt>(clientId, Consts.SessionKeys.UserId);
        return userId?.Value ?? Consts.Id.INVALID_USER_ID;
    }
    
    protected virtual string GetUserName(Header header)
    {
        return GetUserName(header.ClientId);
    }

    protected virtual string GetUserName(uint clientId)
    {
        var userName = SessionManager.Get<PbString>(clientId, Consts.SessionKeys.UserName);
        return userName?.Value ?? string.Empty;
    }

    protected virtual string GetClientAgent(uint clientId)
    {
        var handShake = SessionManager.Get<ReqHankShake>(clientId, nameof(ReqHankShake));
        return handShake?.ClientVersion ?? "";
    }

    protected virtual void PushByUserId<T>(string route, int userId, T data)
    {
        foreach (var clientId in GetClientIds(userId))
        {
            Push(route, clientId, data);
        }
    }
    
    protected virtual void PushToOtherByUserId<T>(Header header, string route, int userId, T data)
    {
        foreach (var clientId in GetClientIds(userId))
        {
            if (clientId == header.ClientId) continue;
            Push(route, clientId, data);
        }
    }
    
    public override void OnPostSendResult(Package pack)
    {
    }

    public override async void OnBroadcast(uint clientId, int eventId, object data)
    {
        try
        {
            var evt = (EventId)eventId;
            var userId = GetUserId(clientId);
            switch (evt)
            {
                case EventId.LoginSuccess:
                    var pack = data as Package<LoginResponse>;
                    await OnLoginSuccess(pack!.Header, userId);
                    break;
                case EventId.LogoutSuccess:
                    await OnLogoutSuccess(clientId, userId);
                    break;
                default:
                    break;
            }
        }
        catch (Exception err)
        {
            Server.OnErrorEvent(clientId, err);
        }
    }

    protected virtual Task OnLoginSuccess(Header header, int userId)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnLogoutSuccess(uint clientId, int userId)
    {
        return Task.CompletedTask;
    }
}