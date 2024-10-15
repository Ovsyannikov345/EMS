using ChatService.BLL.Models.Enums;

namespace ChatService.BLL.Models
{
    public class EstateModel
    {
        public Guid Id { get; set; }

        public required UserProfileModel User { get; set; }

        public EstateType Type { get; set; }

        public required string Address { get; set; }

        public int Area { get; set; }

        public short RoomsCount { get; set; }

        public decimal Price { get; set; }
    }
}
