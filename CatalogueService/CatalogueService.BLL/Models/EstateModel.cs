using CatalogueService.DAL.Models.Enums;

namespace CatalogueService.BLL.Models
{
    public class EstateModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public EstateType Type { get; set; }

        public string Address { get; set; } = string.Empty;

        public int Area { get; set; }

        public short RoomsCount { get; set; }

        public decimal Price { get; set; }

        public List<string> ImageIds { get; set; } = [];
    }
}
