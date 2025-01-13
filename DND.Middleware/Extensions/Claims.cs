using System.Security.Claims;

namespace DND.Middleware.Extensions
{
    public static class Claims
    {
        public static int? GetUserId(this ClaimsPrincipal principal)
        {
            return !string.IsNullOrEmpty(principal?.FindFirstValue(Constants.Claims.Id)) ? int.Parse(principal.FindFirstValue(Constants.Claims.Id)) : null;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            return principal?.FindFirstValue(ClaimTypes.Email);
        }
    }
}
