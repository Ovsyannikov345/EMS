namespace CatalogueService.DAL.Grpc.Models
{
    public record UserProfile(Guid Id, string Auth0Id, string FirstName, string LastName, string PhoneNumber, DateTime BirthDate);
}
