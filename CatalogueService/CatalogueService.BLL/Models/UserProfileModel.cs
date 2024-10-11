namespace CatalogueService.BLL.Models
{
    public record UserProfileModel(Guid Id, string Auth0Id, string FirstName, string LastName, string PhoneNumber, DateTime BirthDate);
}
