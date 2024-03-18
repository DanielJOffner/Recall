using LT.Recall.Cli.Output;
using LT.Recall.Cli.Properties;
using LT.Recall.Cli.Verbs.Base;
using System.Diagnostics.CodeAnalysis;

namespace LT.Recall.Cli.Verbs
{
    internal class Save : Verb<Save.Options>
    {
        private readonly Application.Features.Save.Handler _saveHandler;

        public Save(Application.Features.Save.Handler saveHandler)
        {
            _saveHandler = saveHandler;
        }

        public class Options : Program.Options
        {
            [method: DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(Options))]
            public Options()
            {

            }
            public string Collection { get; set; } = string.Empty;
        }

        protected override string HelpText => Resources.SaveHelpText;

        protected override async Task<CliResult> ExecuteInner(List<string> args, Options options)
        {
            Console.ForegroundColor = Theme.Message;
            Console.Write(Resources.EnterDescriptionMessage);
            Console.ResetColor();
            var description = Console.ReadLine() ?? string.Empty;

            Console.ForegroundColor = Theme.Message;
            Console.Write(Resources.EnterCommandTextMessage);
            Console.ResetColor();
            var commandText = Console.ReadLine() ?? string.Empty;

            Console.ForegroundColor = Theme.Message;
            Console.Write(Resources.EnterTagsMessage);
            Console.ResetColor();
            var tags = Console.ReadLine() ?? string.Empty;

            var response = await _saveHandler.Handle(new Application.Features.Save.Request
            {
                CommandText = commandText,
                Description = description,
                Collection = options.Collection,
                Tags = tags.Split(",").Select(tag => tag.Trim()).ToList()
            }, CancellationToken.None);

            return new CliResult(GetResultMessage(commandText), ResultType.Success, response);
        }

        private string GetResultMessage(string commandText)
        {
            if (commandText.Length > 50)
            {
                return string.Format(Resources.SaveSuccessMessage, commandText.Substring(0, 50) + "...");
            }
            return string.Format(Resources.SaveSuccessMessage, commandText);
        }
    }
}
