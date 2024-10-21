namespace NotificationService.BLL.Models
{
    public class NotificationModel
    {
        public Guid Id { get; set; }

        public required string Title { get; set; }

        public Guid UserId { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
