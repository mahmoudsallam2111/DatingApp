using System.Security.Claims;

namespace DatingApp.WebApi.Infrastracture.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userName =  user.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null) return null;
            return userName;

        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            var userName = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userName == null) return null;
            return userName;

        }
    }
}
