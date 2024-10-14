using ChatService.BLL.Models.Enums;

namespace ChatService.BLL.Models
{
    public class EstateModel
    {
        public Guid Id { get; set; }

        public UserProfileModel User { get; set; } = null!;

        public EstateType Type { get; set; }

        public string Address { get; set; } = string.Empty;

        public int Area { get; set; }

        public short RoomsCount { get; set; }

        public decimal Price { get; set; }
    }
}
