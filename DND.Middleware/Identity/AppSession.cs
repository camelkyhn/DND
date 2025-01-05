namespace DND.Middleware.Identity
{
    public interface IAppSession
    {
        int? UserId { get; set; }
    }

    public class AppSession : IAppSession
    {
        public int? UserId { get; set; }
        public string UserEmail { get; set; }
    }
}
