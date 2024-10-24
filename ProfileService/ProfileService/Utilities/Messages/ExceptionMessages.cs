namespace ProfileService.Utilities.Messages
{
    public static class ExceptionMessages
    {
        public static string FieldIsRequired(string fieldName) =>
            $"{fieldName} is required.";

        public static string FieldLimitExceeded(string fieldName, int maxLength) =>
            $"{fieldName} cannot exceed {maxLength} characters.";

        public static string InvalidAuth0Id(string fieldName) =>
            $"{fieldName} is not a valid auth0 id.";

        public static string DateFieldIsFuture(string fieldName) =>
            $"{fieldName} can't be in the future.";
    }
}
