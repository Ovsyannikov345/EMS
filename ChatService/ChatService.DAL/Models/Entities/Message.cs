using System.ComponentModel.DataAnnotations;

namespace ChatService.DAL.Models.Entities
{
    public class Message
    {
        public Guid Id { get; set; }

        [MaxLength(300)]
        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid ChatId { get; set; }

        public Chat Chat { get; set; } = null!;
    }
}
