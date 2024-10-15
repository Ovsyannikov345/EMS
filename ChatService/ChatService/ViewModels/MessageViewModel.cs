using ChatService.BLL.Models;

namespace ChatService.ViewModels
{
    public class MessageViewModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid ChatId { get; set; }

        public ChatViewModel Chat { get; set; } = null!;
    }
}
