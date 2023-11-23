using LT.Recall.Application.Errors;
using LT.Recall.Domain.Errors;
using LT.Recall.Infrastructure.Errors.Codes;

namespace LT.Recall.Infrastructure.Errors
{
    public class InfrastructureError : BaseError
    {
        private readonly string _message;
        private readonly InfraErrorCode _errorCode;

        public InfrastructureError(string message, InfraErrorCode errorCode)
        {
            _message = message;
            _errorCode = errorCode;
        }
        public InfrastructureError(string message, InfraErrorCode errorCode, Exception exception) : base(exception) 
        {
            _message = message;
            _errorCode = errorCode;
        }

        public InfrastructureError(InfraErrorCode errorCode)
        {
            _message = string.Empty;
            _errorCode = errorCode;
        }

        protected override int ErrorCode => (int)_errorCode;
        protected override Enum Error => _errorCode;

        public override string GetUserFriendlyError() => _message;
    }
}
