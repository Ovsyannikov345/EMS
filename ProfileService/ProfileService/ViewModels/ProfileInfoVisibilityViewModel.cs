using ProfileService.DAL.Models.Enums;
using ProfileService.DAL.Models;

namespace ProfileService.ViewModels
{
    public class ProfileInfoVisibilityViewModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public InfoVisibility PhoneNumberVisibility { get; set; }

        public InfoVisibility BirthDateVisibility { get; set; }
    }
}
