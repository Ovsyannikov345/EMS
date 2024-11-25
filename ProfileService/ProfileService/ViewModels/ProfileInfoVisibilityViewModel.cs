using ProfileService.DAL.Models.Enums;

namespace ProfileService.ViewModels
{
    public class ProfileInfoVisibilityViewModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public InfoVisibility PhoneNumberVisibility { get; set; }

        public InfoVisibility BirthDateVisibility { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
