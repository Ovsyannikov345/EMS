using System.Security.Claims;

namespace CatalogueService.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetAuth0IdFromContext(this HttpContext context) =>
            context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
    }
}
