using AutoMapper;
using CatalogueService.BLL.Models;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CatalogueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EstateFilterController(IEstateFilterService estateFilterService, IMapper mapper) : ControllerBase
    {
        [HttpGet("my")]
        public async Task<EstateFilterViewModel> GetEstateFilter(CancellationToken cancellationToken)
        {
            var filter = await estateFilterService.GetEstateFilterAsync(GetAuth0IdFromContext(), cancellationToken);

            return mapper.Map<EstateFilterViewModel>(filter);
        }

        [HttpPost]
        public async Task<EstateFilterViewModel> CreateEstateFilter(EstateFilterToCreateViewModel filterData, CancellationToken cancellationToken)
        {
            var filter = await estateFilterService.CreateEstateFilterAsync(GetAuth0IdFromContext(), mapper.Map<EstateFilterModel>(filterData), cancellationToken);

            return mapper.Map<EstateFilterViewModel>(filter);
        }

        [HttpPut("{id}")]
        public async Task<EstateFilterViewModel> UpdateEstateFilter(Guid id, EstateFilterViewModel filterData, CancellationToken cancellationToken)
        {
            var userAuth0Id = GetAuth0IdFromContext();

            var updatedFilter = await estateFilterService.UpdateEstateFilterAsync(id, userAuth0Id, mapper.Map<EstateFilterModel>(filterData), cancellationToken);

            return mapper.Map<EstateFilterViewModel>(updatedFilter);
        }

        private string GetAuth0IdFromContext()
        {
            return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
