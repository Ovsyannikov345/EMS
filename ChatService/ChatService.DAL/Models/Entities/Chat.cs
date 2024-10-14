namespace ChatService.DAL.Models.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }

        public Guid EstateId { get; set; }

        public ICollection<Message> Messages { get; set; } = [];
    }
}
