using System.Net;

namespace ProfileService.Utilities.Responses
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Message);
}
