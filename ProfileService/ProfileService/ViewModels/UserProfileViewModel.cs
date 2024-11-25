namespace ProfileService.ViewModels
{
    public class UserProfileViewModel
    {
        public Guid Id { get; set; }

        public required string Auth0Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public int EstateCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
