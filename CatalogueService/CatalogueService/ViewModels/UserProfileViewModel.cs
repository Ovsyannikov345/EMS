namespace CatalogueService.ViewModels
{
    public record UserProfileViewModel(
        Guid Id,
        string Auth0Id,
        string FirstName,
        string LastName,
        string PhoneNumber,
        DateTime BirthDate);
}
