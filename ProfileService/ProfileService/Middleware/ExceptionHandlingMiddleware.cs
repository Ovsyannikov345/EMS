using ProfileService.BLL.Utilities.Exceptions;
using ProfileService.Utilities.Responses;
using System.Net;

namespace ProfileService.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            ExceptionResponse response = ex switch
            {
                BadRequestException => new(HttpStatusCode.BadRequest, ex.Message),
                NotFoundException => new(HttpStatusCode.NotFound, ex.Message),
                _ => new(HttpStatusCode.InternalServerError, ex.Message),
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
