using CatalogueService.BLL.Dto;
using CatalogueService.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstateController(IEstateService estateService) : ControllerBase
    {
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<EstateFullDetails> GetEstateData(Guid id)
        {
            return await estateService.GetEstateDetails(id);
        }
    }
}
