using AutoMapper;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.BLL.Utilities.Exceptions;
using CatalogueService.BLL.Utilities.Exceptions.Messages;
using CatalogueService.DAL.Models.Entities;
using CatalogueService.DAL.Repositories.IRepositories;
using Grpc.Core;

namespace CatalogueService.BLL.Grpc.Services
{
    public class EstateGrpcService(
        IEstateService estateService,
        IEstateRepository estateRepository,
        IMapper mapper) : EstateGrpcServiceProto.EstateGrpcServiceProtoBase
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

        public override async Task<EstateCountResponse> GetUserEstateCount(EstateCountRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId, out Guid userId))
            {
                throw new BadRequestException(ExceptionMessages.InvalidId(nameof(Estate.UserId), request.UserId));
            }

            var estateCount = await estateRepository.CountAsync(e => e.UserId == userId, context.CancellationToken);

            return new EstateCountResponse
            {
                Count = estateCount,
            };
        }
    }
}
