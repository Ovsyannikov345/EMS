using ProfileService.DAL.Models.Enums;

namespace ProfileService.DAL.Models
{
    public class ProfileInfoVisibility
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public required UserProfile User { get; set; }

        public InfoVisibility PhoneNumberVisibility { get; set; }

        public InfoVisibility BirthDateVisibility { get; set; }
    }
}
