using CatalogueService.BLL.Grpc.Models;

namespace CatalogueService.BLL.Dto
{
    public class EstateFullDetails
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public int Area { get; set; }

        public short RoomsCount { get; set; }

        public decimal Price { get; set; }

        public UserProfile User { get; set; } = null!;
    }
}
