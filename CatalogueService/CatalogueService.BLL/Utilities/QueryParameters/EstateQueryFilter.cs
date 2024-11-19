using CatalogueService.DAL.Models.Enums;

namespace CatalogueService.BLL.Utilities.QueryParameters
{
    public record EstateQueryFilter(
        EstateType? Types,
        string? Address,
        int? MinArea,
        int? MaxArea,
        short? MinRoomsCount,
        short? MaxRoomsCount,
        decimal? MinPrice,
        decimal? MaxPrice);
}
