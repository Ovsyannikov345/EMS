namespace CatalogueService.BLL.Utilities.Exceptions.Messages
{
    public static class FileExceptionMessages
    {
        public static readonly string EmptyFile = "File is empty or not provided.";

        public static readonly string InvalidFormat = "Invalid file format. Only .jpg, .jpeg, and .png are allowed.";

        public static readonly string FileSizeExceeded = "File size exceeded. Max allowed size is 2 MB.";

        public static readonly string UnknownError = "Error while saving the image.";

        public static readonly string NotFound = "Image not found";

        public static string CountLimitExceeded(int limit) => $"You can't upload more than {limit} files.";
    }
}
