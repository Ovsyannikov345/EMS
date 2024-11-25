using ProfileService.DAL.Models.Enums;
using ProfileService.DAL.Models;

namespace ProfileService.BLL.Models
{
    public class ProfileInfoVisibilityModel : EntityBase
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public required UserProfileModel User { get; set; }

        public InfoVisibility PhoneNumberVisibility { get; set; }

        public InfoVisibility BirthDateVisibility { get; set; }
    }
}
