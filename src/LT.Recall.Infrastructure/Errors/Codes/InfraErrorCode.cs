namespace LT.Recall.Infrastructure.Errors.Codes
{
    public enum InfraErrorCode
    {
        UnknownError = 0,
        NotFound = 1,
        StateFileNotFound = 2,
        InvalidFileFormat = 3,
        UnsupportedFileType = 4,
        NoTransactionInProgress = 5,
        UniqueConstraintViolation = 6,
        UnknownExportFailure = 7,
        GitHubCollectionNotFound = 8,
        SerializationError = 9,
    }
}
