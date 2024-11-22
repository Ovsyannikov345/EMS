using ProfileService.DAL.Models;

namespace ProfileService.BLL.Models
{
    public class UserProfileModelWithPrivacy : EntityBase
    {
        public Guid Id { get; set; }

        public required string Auth0Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public int EstateCount { get; set; }
    }
}
