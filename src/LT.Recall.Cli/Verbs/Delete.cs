using LT.Recall.Cli.Options;
using LT.Recall.Cli.Output;
using LT.Recall.Cli.Properties;
using LT.Recall.Cli.Verbs.Base;
using MediatR;

namespace LT.Recall.Cli.Verbs
{
    internal class Delete : Verb
    {
        public class DeleteOptions : Program.Options
        {
            public string Collection { get; init; } = string.Empty;
            public string Tags { get; init; } = string.Empty;
        }

        public Delete(IMediator mediator) : base(mediator)
        {
        }

        protected override async Task<CliResult> ExecuteInner(List<string> args, IOptions options)
        {
            var tags = string.IsNullOrWhiteSpace(((DeleteOptions)options).Tags) ? new List<string>() : ((DeleteOptions)options).Tags.Split(",").ToList();
            var collection = ((DeleteOptions)options).Collection;

            var request = new LT.Recall.Application.Features.Delete.Request
            {
                Tags = tags,
                Collection = collection,
                Preview = true
            };

            var previewResponse = await Mediator.Send(request);

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
            var response = await Mediator.Send(request);

            return new CliResult(string.Format(Resources.CommandsDeletedMessage, response.Deleted), ResultType.Success, response);
        }
    }
}
