using AutoMapper;
using CatalogueService.BLL.Models;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.BLL.Utilities.QueryParameters;
using CatalogueService.DAL.Utilities.Pagination;
using CatalogueService.Extensions;
using CatalogueService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogueService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EstateController(IEstateService estateService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<PagedResult<EstateViewModel>> GetEstateList(
            [FromQuery] SortOption sortOption,
            [FromQuery] EstateQueryFilter estateFilter,
            [FromQuery] Pagination pagination,
            CancellationToken cancellationToken)
        {
            var pagedResult = await estateService.GetEstateListAsync(
                sortOption, estateFilter, pagination, cancellationToken);

            return mapper.Map<PagedResult<EstateModel>, PagedResult<EstateViewModel>>(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<EstateWithProfileViewModel> GetEstateData(Guid id, CancellationToken cancellationToken)
        {
            var estate = await estateService.GetEstateDetailsAsync(id, cancellationToken);

            return mapper.Map<EstateWithProfileViewModel>(estate);
        }

        [HttpPost]
        public async Task<EstateViewModel> CreateEstate(EstateToCreateViewModel estateData, CancellationToken cancellationToken)
        {
            var auth0Id = HttpContext.GetAuth0IdFromContext();

            var createdEstate = await estateService.CreateEstateAsync(mapper.Map<EstateModel>(estateData), auth0Id, cancellationToken);

            return mapper.Map<EstateViewModel>(createdEstate);
        }

        [HttpPut]
        public async Task<EstateViewModel> UpdateEstate(EstateToUpdateViewModel estateToUpdate, CancellationToken cancellationToken)
        {
            var auth0Id = HttpContext.GetAuth0IdFromContext();

            var updatedEstate = await estateService.UpdateEstateAsync(mapper.Map<EstateModel>(estateToUpdate), auth0Id, cancellationToken);

            return mapper.Map<EstateViewModel>(updatedEstate);
        }

        [HttpDelete("{id}")]
        public async Task DeleteEstate(Guid id, CancellationToken cancellationToken)
        {
            var auth0Id = HttpContext.GetAuth0IdFromContext();

            await estateService.DeleteEstateAsync(id, auth0Id, cancellationToken);
        }
    }
}
