using CatalogueService.DAL.Models.Enums;
using System.Text.Json.Serialization;

namespace CatalogueService.ViewModels
{
    public record EstateToCreateViewModel(
        [property: JsonConverter(typeof(JsonStringEnumConverter))] EstateType Type,
        string Address,
        int Area,
        short RoomsCount,
        decimal Price);
}
