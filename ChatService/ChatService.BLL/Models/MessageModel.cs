namespace ChatService.BLL.Models
{
    public class MessageModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid ChatId { get; set; }
    }
}
