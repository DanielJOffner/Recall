namespace LT.Recall.Infrastructure.Errors.Codes
{
    public enum InfraErrorCode
    {
        NotFound = 1,
        StateFileNotFound = 2,
        InvalidFileFormat = 3,
        UnsupportedFileType = 4,
        NoTransactionInProgress = 5,
        UniqueConstraintViolation = 6,
        UnknownExportFailure = 7,
    }
}
