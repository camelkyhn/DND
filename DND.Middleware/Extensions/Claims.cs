using System.Security.Claims;
using System;

namespace DND.Middleware.Extensions
{
    public static class Claims
    {
        public static int? GetUserId(this ClaimsPrincipal principal)
        {
            return !string.IsNullOrEmpty(principal?.FindFirstValue(ClaimTypes.NameIdentifier)) ? int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)) : null;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            return principal?.FindFirstValue(ClaimTypes.Email);
        }
    }
}
