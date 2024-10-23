using CatalogueService.DAL.Models.Enums;

namespace CatalogueService.ViewModels
{
    public class EstateFilterToCreateViewModel
    {
        public EstateType EstateTypes { get; set; }

        public int MinArea { get; set; }

        public int MaxArea { get; set; }

        public short MinRoomsCount { get; set; }

        public short MaxRoomsCount { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }
    }
}
