namespace ProfileService.BLL.Models
{
    public class UserProfileModel
    {
        public Guid Id { get; set; }

        public required string Auth0Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public int EstateCount { get; set; }
    }
}
