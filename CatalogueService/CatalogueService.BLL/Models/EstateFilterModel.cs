using CatalogueService.DAL.Models.Enums;

namespace CatalogueService.BLL.Models
{
    public class EstateFilterModel : ModelBase
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public EstateType EstateTypes { get; set; }

        public int MinArea { get; set; }

        public int MaxArea { get; set; } = int.MaxValue;

        public short MinRoomsCount { get; set; }

        public short MaxRoomsCount { get; set; } = short.MaxValue;

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; } = decimal.MaxValue;
    }
}
