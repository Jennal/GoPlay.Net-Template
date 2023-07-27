using GoPlay.Core.Protocols;

namespace Common;

public static class Consts
{
    public static class Id
    {
        public const uint INVALID_CLIENT_ID = IdLoopGenerator.INVALID;
        public const int INVALID_USER_ID = -1;
    }
    
    public static class SessionKeys 
    {
        public const string UserId = "user_id";
        public const string UserName = "user_name";
    }

    public static class ErrCode
    {
        public const string LOGIN_REQUIRED = "LOGIN_REQUIRED";
    }
}
