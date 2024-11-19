using CatalogueService.DAL.Models.Enums;

namespace CatalogueService.ViewModels
{
    public record EstateWithProfileViewModel(
        Guid Id,
        Guid UserId,
        EstateType Type,
        string Address,
        int Area,
        short RoomsCount,
        decimal Price,
        UserProfileViewModel User,
        List<string> ImageIds);
}
