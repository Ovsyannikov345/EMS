namespace CatalogueService.Utilities.Messages
{
    public static class ExceptionMessages
    {
        public static string FieldIsRequired(string fieldName) =>
            $"{fieldName} is required.";

        public static string FieldIsInvalidType(string fieldName, string type) =>
            $"{fieldName} must be a valid {type}.";
        public static string FieldLimitExceeded(string fieldName, int maxLength) =>
            $"{fieldName} cannot exceed {maxLength} characters.";

        public static string FieldValueTooSmall(string fieldName, int minValue) =>
            $"{fieldName} must be greater than {minValue}.";
        public static string FieldValueTooSmall(string fieldName, decimal minValue) =>
            $"{fieldName} must be greater than {minValue}.";
    }
}
