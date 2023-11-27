using FluentValidation;
using LT.Recall.Application.Errors;

namespace LT.Recall.Application.Extensions
{
    internal static class ValidationExtensions
    {
        /// <exception cref="ValidationError">If the request is invalid</exception>
        public static void ValidateOrThrow<T>(this IValidator<T> validator, T request)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationError(validationResult);
            }
        }

        /// <exception cref="ValidationError">If the request is invalid</exception>
        public static async Task ValidateOrThrowAsync<T>(this IValidator<T> validator, T request)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationError(validationResult);
            }
        }
    }
}
