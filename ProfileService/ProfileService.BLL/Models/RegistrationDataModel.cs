namespace ProfileService.BLL.Models
{
    public class RegistrationDataModel
    {
        public required string Id { get; set; }

        public required string LastName { get; set; }

        public required string FirstName { get; set; }

        public DateTime BirthDate { get; set; }

        public required string PhoneNumber { get; set; }
    }
}
