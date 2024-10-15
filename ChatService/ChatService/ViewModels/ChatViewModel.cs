namespace ChatService.ViewModels
{
    public class ChatViewModel
    {
        public Guid Id { get; set; }

        public EstateViewModel Estate { get; set; } = null!;

        public UserProfileViewModel User { get; set; } = null!;

        public ICollection<MessageViewModel> Messages { get; set; } = [];
    }
}
