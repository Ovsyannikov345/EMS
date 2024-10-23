namespace CatalogueService.BLL.Utilities.Messages
{
    public static class EstateMessages
    {
        // TODO create single message class
        public const string EstateNotFound = "Estate is not found";

        public const string EstateDeleteForbidden = "You can't delete estate of other user";

        public const string EstateUpdateForbidden = "You can't edit estate of other user";

        public const string EstateIdIsInvalid = "Estate id is invalid";
    }
}
