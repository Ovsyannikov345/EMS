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
    public class EstateController(IEstateService estateService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<EstateViewModel>> GetEstateList(CancellationToken cancellationToken)
        {
            var estateList = await estateService.GetEstateListAsync(cancellationToken);

            return mapper.Map<IEnumerable<EstateModel>, IEnumerable<EstateViewModel>>(estateList);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<EstateWithProfileViewModel> GetEstateData(Guid id, CancellationToken cancellationToken)
        {
            var estate = await estateService.GetEstateDetailsAsync(id, cancellationToken);

            return mapper.Map<EstateWithProfileViewModel>(estate);
        }

        [HttpPost]
        [Authorize]
        public async Task<EstateViewModel> CreateEstate(EstateToCreateViewModel estateData, CancellationToken cancellationToken)
        {
            var auth0Id = GetAuth0IdFromContext();

            var createdEstate = await estateService.CreateEstateAsync(mapper.Map<EstateModel>(estateData), auth0Id, cancellationToken);

            return mapper.Map<EstateViewModel>(createdEstate);
        }

        [HttpPut]
        [Authorize]
        public async Task<EstateViewModel> UpdateEstate(EstateToUpdateViewModel estateToUpdate, CancellationToken cancellationToken)
        {
            var auth0Id = GetAuth0IdFromContext();

            var updatedEstate = await estateService.UpdateEstateAsync(mapper.Map<EstateModel>(estateToUpdate), auth0Id, cancellationToken);

            return mapper.Map<EstateViewModel>(updatedEstate);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task DeleteEstate(Guid id, CancellationToken cancellationToken)
        {
            var auth0Id = GetAuth0IdFromContext();

            await estateService.DeleteEstateAsync(id, auth0Id, cancellationToken);
        }

        private string GetAuth0IdFromContext()
        {
            return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
