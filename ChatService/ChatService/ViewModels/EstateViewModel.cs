using ChatService.BLL.Models.Enums;

namespace ChatService.ViewModels
{
    public class EstateViewModel
    {
        public Guid Id { get; set; }

        public UserProfileViewModel User { get; set; } = null!;

        public EstateType Type { get; set; }

        public string Address { get; set; } = string.Empty;

        public int Area { get; set; }

        public short RoomsCount { get; set; }

        public decimal Price { get; set; }
    }
}
