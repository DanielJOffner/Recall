using LT.Recall.Domain.Errors.Codes;

namespace LT.Recall.Domain.Errors
{
    internal class DomainError : BaseError
    {
        private readonly string _message;
        private readonly DomainErrorCode _errorCode;

        public DomainError(string message, DomainErrorCode errorCode)
        {
            _message = message;
            _errorCode = errorCode;
        }

        protected override int ErrorCode => (int)_errorCode;
        protected override Enum Error => _errorCode;

        public override string GetUserFriendlyError() => _message;
    }
}
