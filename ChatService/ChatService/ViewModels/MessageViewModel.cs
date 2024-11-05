using ChatService.BLL.Models;

namespace ChatService.ViewModels
{
    public record MessageViewModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid ChatId { get; set; }

        public Guid UserId { get; set; }
    }
}
