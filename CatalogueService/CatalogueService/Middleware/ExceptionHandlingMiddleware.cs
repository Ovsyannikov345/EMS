﻿using CatalogueService.BLL.Utilities.Exceptions;
using CatalogueService.Utilities.Responses;
using System.Net;

namespace CatalogueService.Middleware
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
                BadRequestException => new((int)HttpStatusCode.BadRequest, ex.Message),
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
