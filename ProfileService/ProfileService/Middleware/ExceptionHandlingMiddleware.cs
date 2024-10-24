using ProfileService.BLL.Utilities.Exceptions;
using ProfileService.Utilities.Responses;
using System.Net;

namespace ProfileService.Middleware
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
                BadRequestException => new((int)HttpStatusCode.BadRequest, ex.Message),
                NotFoundException => new((int)HttpStatusCode.NotFound, ex.Message),
                _ => new((int)HttpStatusCode.InternalServerError, ex.Message),
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
