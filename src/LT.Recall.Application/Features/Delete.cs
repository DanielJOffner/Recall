using FluentValidation;
using LT.Recall.Application.Abstractions;
using LT.Recall.Application.Extensions;
using LT.Recall.Application.Properties;
using LT.Recall.Domain.ValueObjects;

namespace LT.Recall.Application.Features
{
    public class Delete
    {
        public class Request 
        {
            public List<string> Tags { get; init; } = new();
            public string Collection { get; init; } = string.Empty;
            public bool Preview { get; set; } = true;
        }

        public class Response
        {
            public int WillBeDeleted { get; set; }
            public int Deleted { get; set; }
        }

        private class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x)
                    .Must(x =>
                    {
                        var hasTags = x.Tags.Any();
                        var hasCollection = !string.IsNullOrWhiteSpace(x.Collection);
                        return hasTags || hasCollection;
                    })
                    .WithMessage(Resources.MissingDeleteParametersError);
            }
        }

        public class Handler
        {
            private readonly Validator _validator = new();
            private readonly ICommandRepository _commandRepository;

            public Handler(ICommandRepository commandRepository)
            {
                _commandRepository = commandRepository;
            }

            public async Task<Delete.Response> Handle(Request request, CancellationToken cancellationToken)
            {
                await _validator.ValidateOrThrowAsync(request);

                var response = new Delete.Response();
                var commands = await _commandRepository.FetchAsync(request.Collection, request.Tags.Select(t => new Tag(t)).ToList());

                if (request.Preview)
                {
                    response.WillBeDeleted = commands.Count;
                    return response;
                }

                await _commandRepository.DeleteAsync(commands.Select(c => c.Id ?? string.Empty).ToList());
                response.Deleted = commands.Count;
                return response;
            }
        }
    }
}
