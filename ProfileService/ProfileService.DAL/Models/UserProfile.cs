using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProfileService.DAL.Models
{
    public class UserProfile
    {
        public Guid Id { get; set; }

        public string Auth0Id { get; set; } = string.Empty;

        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(30)]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }
    }
}
