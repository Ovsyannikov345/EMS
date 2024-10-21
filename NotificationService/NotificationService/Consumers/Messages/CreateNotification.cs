namespace NotificationService.Consumers.Messages
{
    public record CreateNotification
    {
        public required string Title { get; init; }

        public Guid UserId { get; init; }
    }
}
