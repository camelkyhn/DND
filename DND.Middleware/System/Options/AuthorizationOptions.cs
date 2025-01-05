namespace DND.Middleware.System.Options
{
    public class AuthorizationOptions
    {
        public const string Authorization = "Authorization";

        public AuthorizationJwtOptions Jwt { get; set; }
    }

    public class AuthorizationJwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
}
