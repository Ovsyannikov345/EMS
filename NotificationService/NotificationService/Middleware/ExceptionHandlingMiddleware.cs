using NotificationService.BLL.Utilities.Exceptions;
using NotificationService.Utilities.Responses;
using System.Net;

namespace NotificationService.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, Serilog.ILogger logger)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            logger.Error(ex.Message);

            ExceptionResponse response = ex switch
            {
                NotFoundException => new((int)HttpStatusCode.NotFound, ex.Message),
                ForbiddenException => new((int)HttpStatusCode.Forbidden, ex.Message),
                _ => new((int)HttpStatusCode.InternalServerError, ex.Message),
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
