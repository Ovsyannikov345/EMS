using ProfileService.DAL.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ProfileService.DAL.Models
{
    public class UserProfile : ICacheable
    {
        public Guid Id { get; set; }

        public required string Auth0Id { get; set; }

        [MaxLength(50)]
        public required string FirstName { get; set; }

        [MaxLength(50)]
        public required string LastName { get; set; }

        [MaxLength(30)]
        public required string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public required ProfileInfoVisibility InfoVisibility { get; set; }
    }
}
