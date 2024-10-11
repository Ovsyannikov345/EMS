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
            var auth0Id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var createdEstate = await estateService.CreateEstateAsync(mapper.Map<EstateModel>(estateData), auth0Id, cancellationToken);

            return mapper.Map<EstateViewModel>(createdEstate);
        }
    }
}
