namespace ChatService.BLL.Models
{
    public class ChatModel
    {
        public Guid Id { get; set; }

        public EstateModel Estate { get; set; } = null!;

        public UserProfileModel User { get; set; } = null!;

        public ICollection<MessageModel> Messages { get; set; } = [];
    }
}
