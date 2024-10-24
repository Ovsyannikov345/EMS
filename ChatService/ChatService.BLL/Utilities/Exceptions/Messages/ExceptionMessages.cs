namespace ProfileService.BLL.Utilities.Exceptions.Messages
{
    public static class ExceptionMessages
    {
        public const string SelfChatCreation = "You can't create chat with yourself";

        public static string NotFound(string entityType, string parameterName, object parameterValue) =>
            $"{entityType} with {parameterName}:{parameterValue} was not found";

        public static string AlreadyExists(string entityType, string parameterName, object parameterValue) =>
            $"{entityType} with {parameterName}:{parameterValue} already exists";

        public static string InvalidId(string entityType, object id) =>
            $"Provided id: {id} for {entityType} is invalid or id's in request doesn't match";

        public static string AccessDenied(string entityType, Guid id) =>
            $"You can't access {entityType} with id: {id}";
    }
}
