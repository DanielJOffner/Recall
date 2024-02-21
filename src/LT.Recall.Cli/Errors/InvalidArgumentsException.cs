using LT.Recall.Cli.Properties;
using LT.Recall.Domain.Errors;

namespace LT.Recall.Cli.Errors
{
    public class InvalidArgumentsError : BaseError
    {
        protected override int ErrorCode => (int)CommandLineError.InvalidArguments;
        protected override Enum Error => CommandLineError.InvalidArguments;

        private readonly string _verb;

        public InvalidArgumentsError(string verb)
        {
            _verb = verb;
        }

        public override string GetUserFriendlyError()
        {
            return string.Format(Resources.InvalidArgumentsError, _verb);
        }
    }
}
