using CatalogueService.BLL.Dto;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.DAL.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstateController(IEstateService estateService) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<Estate>> GetEstateList(CancellationToken cancellationToken)
        {
            return await estateService.GetEstateList(cancellationToken);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<EstateFullDetails> GetEstateData(Guid id, CancellationToken cancellationToken)
        {
            return await estateService.GetEstateDetails(id, cancellationToken);
        }
    }
}
