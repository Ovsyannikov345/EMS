namespace ChatService.BLL.Models
{
    public class UserProfileModel
    {
        public Guid Id { get; set; }

        public string Auth0Id { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }
    }
}
