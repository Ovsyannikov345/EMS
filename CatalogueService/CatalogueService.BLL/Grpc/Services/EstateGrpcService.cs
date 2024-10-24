﻿using AutoMapper;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.BLL.Utilities.Exceptions;
using CatalogueService.BLL.Utilities.Exceptions.Messages;
using CatalogueService.DAL.Models.Entities;
using Grpc.Core;

namespace CatalogueService.BLL.Grpc.Services
{
    public class EstateGrpcService(IEstateService estateService, IMapper mapper) : EstateGrpcServiceProto.EstateGrpcServiceProtoBase
    {
        public override async Task<EstateResponse> GetEstate(EstateRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Id, out Guid estateId))
            {
                throw new BadRequestException(ExceptionMessages.InvalidId(nameof(Estate), request.Id));
            }

            var estate = await estateService.GetEstateDetailsAsync(estateId, context.CancellationToken);

            return new EstateResponse
            {
                Estate = mapper.Map<ProtoEstateModel>(estate)
            };
        }
    }
}
