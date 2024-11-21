using CatalogueService.DAL.Models.Enums;

namespace CatalogueService.ViewModels
{
    public record EstateViewModel(
        Guid Id,
        Guid UserId,
        EstateType Type,
        string Address,
        int Area,
        short RoomsCount,
        decimal Price,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        List<string> ImageIds);
}
