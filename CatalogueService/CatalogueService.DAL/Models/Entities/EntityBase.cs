namespace CatalogueService.DAL.Models.Entities
{
    public class EntityBase
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
