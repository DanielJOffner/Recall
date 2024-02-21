using System.Text;

namespace LT.Recall.Domain.Errors
{
    public abstract class BaseError : Exception
    {
        protected abstract int ErrorCode { get; }
        protected abstract Enum Error { get; }

        protected BaseError(Exception exception) : base(exception.Message, exception)
        {

        }

        protected BaseError()
        {

        }

        public abstract string GetUserFriendlyError();

        public override string Message => FormatMessage();

        private string FormatMessage()
        {
            // cast ErrorCode to int
            var sb = new StringBuilder();
            sb.Append($"Error {ErrorCodeFormatter.FormatErrorCode(Error.GetType(), ErrorCode)}. ");
            sb.Append(GetUserFriendlyError());
            return sb.ToString();
        }
    }
}
