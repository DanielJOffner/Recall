using LT.Recall.Cli.Output;
using LT.Recall.Cli.Properties;
using LT.Recall.Cli.Verbs.Base;
using System.Diagnostics.CodeAnalysis;

namespace LT.Recall.Cli.Verbs
{
    internal class Delete : Verb<Delete.Options>
    {
        private readonly Application.Features.Delete.Handler _deleteHandler;

        public Delete(Application.Features.Delete.Handler deleteHandler)
        {
            _deleteHandler = deleteHandler;
        }

        protected override string HelpText => Resources.DeleteHelpText;
        public class Options : Program.Options
        {
            [method: DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(Options))]
            public Options()
            {

            }
            public string Collection { get; init; } = string.Empty;
            public string Tags { get; init; } = string.Empty;
        }

        protected override async Task<CliResult> ExecuteInner(List<string> args, Options options)
        {
            var tags = string.IsNullOrWhiteSpace(options.Tags) ? new List<string>() : (options).Tags.Split(",").ToList();
            var collection = options.Collection;

            var request = new LT.Recall.Application.Features.Delete.Request
            {
                Tags = tags,
                Collection = collection,
                Preview = true
            };

            var previewResponse = await _deleteHandler.Handle(request, CancellationToken.None);

            if (previewResponse.WillBeDeleted == 0)
            {
                return new CliResult(Resources.DeleteNoCommandsFoundMessage, ResultType.Message, previewResponse);
            }

            Console.ForegroundColor = Theme.Message;
            Console.Write(Resources.PreviewDeleteMessage, previewResponse.WillBeDeleted);

            ConsoleKey key;
            if (options.XTest)
            {
                key = (ConsoleKey)Console.Read();
            }
            else
            {
                key = Console.ReadKey().Key;
            }
            Console.WriteLine();

            if (key != ConsoleKey.Y)
            {
                return new CliResult(Resources.DeleteAbortedMessage, ResultType.Message, previewResponse);
            }

            request.Preview = false;
            var response = await _deleteHandler.Handle(request, CancellationToken.None);

            return new CliResult(string.Format(Resources.CommandsDeletedMessage, response.Deleted), ResultType.Success, response);
        }
    }
}
