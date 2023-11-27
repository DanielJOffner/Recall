using FluentAssertions;
using LT.Recall.Application.Errors.Codes;
using LT.Recall.Domain.Errors;
using LT.Recall.Infrastructure.Errors.Codes;

namespace LT.Recall.IntegrationTests.Extensions
{
    internal static class Assertions
    {
        public static void AssertIsError(this string errorMessage, InfraErrorCode errorCode)
        {
            errorMessage.Should().Contain(ErrorCodeFormatter.FormatErrorCode(errorCode.GetType(), (int)errorCode));
        }

        public static void AssertIsError(this string errorMessage, ApplicationErrorCode errorCode)
        {
            errorMessage.Should().Contain(ErrorCodeFormatter.FormatErrorCode(errorCode.GetType(), (int)errorCode));
        }
    }
}
