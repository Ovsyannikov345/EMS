using System.Security.Claims;

namespace ProfileService.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetAuth0IdFromContext(this HttpContext context) =>
            context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
    }
}
