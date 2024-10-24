namespace CatalogueService.BLL.Utilities.Exceptions.Messages
{
    public static class ExceptionMessages
    {
        public static string NotFound(string entityType, string parameterName, object parameterValue) =>
            $"{entityType} with {parameterName}:{parameterValue} was not found";

        public static string AlreadyExists(string entityType, string parameterName, object parameterValue) =>
            $"{entityType} with {parameterName}:{parameterValue} already exists";

        public static string InvalidId(string entityType, object id) =>
            $"Provided id: {id} for {entityType} is invalid or id's in request doesn't match";

        public static string AccessDenied(string entityType, Guid id) =>
            $"You can't access {entityType} with id: {id}";

        public static string UpdateDenied(string entityType, Guid id) =>
            $"You can't update {entityType} with id: {id}";

        public static string DeleteDenied(string entityType, Guid id) =>
            $"You can't delete {entityType} with id: {id}";
    }
}
