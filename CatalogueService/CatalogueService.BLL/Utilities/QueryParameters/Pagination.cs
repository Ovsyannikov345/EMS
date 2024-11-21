namespace CatalogueService.BLL.Utilities.QueryParameters
{
    public record Pagination(int PageSize = 10, int PageNumber = 1);
}
