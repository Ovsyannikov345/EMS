﻿using Google.Rpc;
using Grpc.Core.Interceptors;
using Grpc.Core;
using ProfileService.BLL.Utilities.Exceptions;
using ProfileService.Utilities.Responses;

namespace ProfileService.Extensions
{
    public class GrpcExceptionHandlingInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await base.UnaryServerHandler(request, context, continuation);
            }
            catch (Exception e)
            {
                return MapResponse<TResponse>(e);
            }
        }

        private static TResponse MapResponse<TResponse>(Exception ex)
        {
            var status = ex switch
            {
                NotFoundException => new ExceptionResponse((int)Code.NotFound, ex.Message),

                InvalidOperationException => new ExceptionResponse((int)Code.FailedPrecondition, ex.Message),

                _ => new ExceptionResponse((int)Code.Internal, ex.Message),
            };

            var concreteResponse = Activator.CreateInstance<TResponse>();

            concreteResponse?.GetType().GetProperty(nameof(status.StatusCode))?.SetValue(concreteResponse, status.StatusCode);

            concreteResponse?.GetType().GetProperty(nameof(status.Message))?.SetValue(concreteResponse, status.Message);

            return concreteResponse;
        }
    }
}
