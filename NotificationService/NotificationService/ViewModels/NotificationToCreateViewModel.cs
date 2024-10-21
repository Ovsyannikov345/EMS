namespace NotificationService.ViewModels
{
    public class NotificationToCreateViewModel
    {
        public required string Title { get; set; }

        public Guid UserId { get; set; }
    }
}
