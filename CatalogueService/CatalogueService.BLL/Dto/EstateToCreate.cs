namespace CatalogueService.BLL.Dto
{
    public record EstateToCreate(Guid UserId, string Type, string Address, int Area, short RoomsCount, decimal Price)
    {
    }
}
