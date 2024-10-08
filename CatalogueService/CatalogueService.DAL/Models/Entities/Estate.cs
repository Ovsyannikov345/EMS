using CatalogueService.DAL.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CatalogueService.DAL.Models.Entities
{
    public class Estate
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public EstateType Type { get; set; }

        [MaxLength(100)]
        public string Address { get; set; } = string.Empty;

        public int Area { get; set; }

        public short RoomsCount { get; set; }

        public decimal Price { get; set; }
    }
}
