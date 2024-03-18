using LT.Recall.Application.Errors;
using LT.Recall.Cli.Options;
using LT.Recall.Cli.Output;
using LT.Recall.Cli.Properties;
using LT.Recall.Cli.Themes;
using LT.Recall.Domain.Errors;

namespace LT.Recall.Cli.Verbs.Base
{
    internal interface IVerb
    {
        Task<CliResult> ExecuteAsync(ITheme theme, string[] args, Program.Options options);
    }

    internal abstract class Verb<TOptions> : IVerb where TOptions : IOptions, new()
    {
        protected ITheme Theme;
        protected Verb()
        {
            Theme = new DefaultTheme();
        }

        protected abstract string HelpText { get; }

        public async Task<CliResult> ExecuteAsync(ITheme theme, string[] args, Program.Options options)
        {
            Theme = theme;

            try
            {
                var verbOptions = OptionsParser.Parse<TOptions>(args);

                if (verbOptions.Help)
                    return new CliResult(HelpText);

                return await ExecuteInner(OptionsParser.RemoveOptions(VerbParser.RemoveVerb(args), verbOptions).ToList(), verbOptions);
            }
            catch (Exception ex)
            {
                return HandleException(options, ex);
            }
        }

        private static CliResult HandleException(Program.Options options, Exception ex)
        {
            string message;
            ResultType resultType = ResultType.Error;

            if (ex is BaseError baseException)
            {
                if (ex is ValidationError)
                {
                    resultType = ResultType.Warning;
                    message = baseException.GetUserFriendlyError();
                }
                else if (options.Verbose)
                {
                    message = baseException.ToString();
                }
                else
                {
                    message = baseException.Message;
                }
            }
            else
            {
                if (options.Verbose)
                {
                    message = ex.ToString();
                }
                else
                {
                    message = Resources.UnknownError;
                }
            }

            return new CliResult(message, resultType);
        }

        protected abstract Task<CliResult> ExecuteInner(List<string> args, TOptions options);
    }
}
