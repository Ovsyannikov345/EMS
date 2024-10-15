using System.ComponentModel.DataAnnotations;

namespace ChatService.DAL.Models.Entities
{
    public class Message
    {
        public Guid Id { get; set; }

        [MaxLength(300)]
        public required string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid ChatId { get; set; }

        public required Chat Chat { get; set; }
    }
}
