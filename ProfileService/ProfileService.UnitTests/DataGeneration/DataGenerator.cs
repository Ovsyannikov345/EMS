using Bogus;
using ProfileService.DAL.Models;
using ProfileService.DAL.Models.Enums;

namespace ProfileService.UnitTests.Datageneration
{
    internal static class DataGenerator
    {
        public static List<UserProfile> GenerateProfiles(int count) =>
            new Faker<UserProfile>()
                .RuleFor(p => p.Id, _ => Guid.NewGuid())
                .RuleFor(p => p.Auth0Id, f => "auth0|" + f.Random.String2(24, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"))
                .RuleFor(p => p.FirstName, f => f.Name.FirstName())
                .RuleFor(p => p.LastName, f => f.Name.LastName())
                .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(p => p.BirthDate, f => f.Date.Between(DateTime.UtcNow.AddYears(-90), DateTime.UtcNow))
                .Generate(count);

        public static List<ProfileInfoVisibility> GenerateProfileVisibilities(int count) =>
            new Faker<ProfileInfoVisibility>()
                .RuleFor(p => p.Id, _ => Guid.NewGuid())
                .RuleFor(p => p.PhoneNumberVisibility, f => f.PickRandom<InfoVisibility>())
                .RuleFor(p => p.BirthDateVisibility, f => f.PickRandom<InfoVisibility>())
                .Generate(count);
    }
}
