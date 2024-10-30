using ChatService.BLL.Models;

namespace ChatService.ViewModels
{
    public class MessageViewModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid ChatId { get; set; }

        public Guid UserId { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not MessageViewModel messageViewModel)
            {
                return false;
            }

            return Id.Equals(messageViewModel.Id) &&
                Text.Equals(messageViewModel.Text) &&
                CreatedAt.Equals(messageViewModel.CreatedAt) &&
                ChatId.Equals(messageViewModel.ChatId) &&
                UserId.Equals(messageViewModel.UserId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id.GetHashCode(), 
                Text.GetHashCode(),
                CreatedAt.GetHashCode(),
                ChatId.GetHashCode(),
                UserId.GetHashCode());
        }
    }
}
