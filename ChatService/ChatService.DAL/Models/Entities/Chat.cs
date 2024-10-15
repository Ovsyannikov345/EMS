namespace ChatService.DAL.Models.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }

        public Guid EstateId { get; set; }

        public Guid UserId { get; set; }

        public required ICollection<Message> Messages { get; set; }
    }
}
