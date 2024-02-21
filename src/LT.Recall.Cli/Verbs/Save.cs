using LT.Recall.Cli.Options;
using LT.Recall.Cli.Output;
using LT.Recall.Cli.Properties;
using LT.Recall.Cli.Verbs.Base;
using LT.Recall.Domain.ValueObjects;
using MediatR;

namespace LT.Recall.Cli.Verbs
{
    internal class Save : Verb
    {
        public class SaveOptions : Program.Options
        {
            public string Collection { get; set; } = string.Empty;
        }

        public Save(IMediator mediator) : base(mediator)
        {

        }

        protected override async Task<CliResult> ExecuteInner(List<string> args, IOptions options)
        {
            var saveOptions = (SaveOptions)options;

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

            var response = await Mediator.Send(new Application.Features.Save.Request
            {
                CommandText = commandText,
                Description = description,
                Collection = saveOptions.Collection,
                Tags = tags.Split(",").Select(tag => tag.Trim()).ToList()
            });

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
