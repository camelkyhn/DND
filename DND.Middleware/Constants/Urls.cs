namespace DND.Middleware.Constants
{
    public class Urls
    {
        public const string ServerDomain = "https://dndforreal.com";

        public const string ConfirmEmail = ServerDomain + "/" + Areas.Identity + "/" + Controllers.Account + "/" + Actions.ConfirmEmail;
        public const string ResetPassword = ServerDomain + "/" + Areas.Identity + "/" + Controllers.Account + "/" + Actions.ResetPassword;
    }
}