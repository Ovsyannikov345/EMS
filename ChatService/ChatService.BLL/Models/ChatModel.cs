namespace ChatService.BLL.Models
{
    public class ChatModel
    {
        public Guid Id { get; set; }

        public required EstateModel Estate { get; set; }

        public required UserProfileModel User { get; set; }

        public required ICollection<MessageModel> Messages { get; set; }
    }
}
