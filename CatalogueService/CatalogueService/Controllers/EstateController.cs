using AutoMapper;
using CatalogueService.BLL.Models;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var estateList = await estateService.GetEstateList(cancellationToken);

            return mapper.Map<IEnumerable<EstateModel>, IEnumerable<EstateViewModel>>(estateList);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<EstateWithProfileViewModel> GetEstateData(Guid id, CancellationToken cancellationToken)
        {
            var estate = await estateService.GetEstateDetails(id, cancellationToken);

            return mapper.Map<EstateWithProfileViewModel>(estate);
        }
    }
}
