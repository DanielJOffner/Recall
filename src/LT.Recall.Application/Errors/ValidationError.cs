using System.Text;
using FluentValidation.Results;
using LT.Recall.Application.Errors.Codes;
using LT.Recall.Application.Properties;
using LT.Recall.Domain.Errors;

namespace LT.Recall.Application.Errors
{
    public class ValidationError : BaseError
    {
        protected override int ErrorCode => (int)ApplicationErrorCode.ValidationError;
        protected override Enum Error => ApplicationErrorCode.ValidationError;

        public ValidationError(ValidationResult validationResult)
        {
            if (validationResult == null) 
                throw new ArgumentException(string.Format(Resources.InputRequiresValueError, nameof(validationResult)));
            
            if (validationResult.Errors == null || !validationResult.Errors.Any()) 
                throw new ArgumentException(string.Format(Resources.InputRequiresValueError, nameof(validationResult.Errors)));
        
            ValidationResult = validationResult;
        }

        public ValidationError(string message)
        {
            ValidationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Error", message) });
        }

        ValidationResult ValidationResult { get; }


        public override string GetUserFriendlyError()
        {
            var sb = new StringBuilder();
            sb.Append(Resources.ValidationError);
            sb.Append(": ");
            sb.Append(string.Join(", ", ValidationResult.Errors.Select(x => x.ErrorMessage).ToList()));
            return sb.ToString();
        }
    }
}
