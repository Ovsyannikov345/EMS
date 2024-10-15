namespace ChatService.BLL.Models
{
    public class MessageModel
    {
        public Guid Id { get; set; }

        public required string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid ChatId { get; set; }
    }
}
