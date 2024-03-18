using FluentValidation;
using FluentValidation.Results;
using LT.Recall.Application.Abstractions;
using LT.Recall.Application.Errors;
using LT.Recall.Application.Extensions;
using LT.Recall.Application.Properties;
using LT.Recall.Domain.Entities;
using LT.Recall.Domain.ValueObjects;

namespace LT.Recall.Application.Features
{
    public class Save
    {
        public class Request
        {
            public string CommandText { get; init; } = string.Empty;
            public string Description { get; init; } = string.Empty;
            public List<string> Tags { get; init; } = new();
            public string Collection { get; set; } = string.Empty;
        }

        public class Response
        {
            public string CommandId { get; init; } = string.Empty;
            public string CommandText { get; init; } = string.Empty;
            public string Description { get; init; } = string.Empty;
            public List<string> Tags { get; init; } = new();
        }

        private class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.CommandText).NotEmpty().WithMessage(Resources.CommandTextMissingError);
                RuleFor(x => x.Description).NotEmpty().WithMessage(Resources.DescriptionMissingError);
            }
        }

        public class Handler
        {
            private readonly Validator _validator;
            private readonly ICommandRepository _commandRepository;
            public Handler(ICommandRepository commandRepository)
            {
                _commandRepository = commandRepository;
                _validator = new Validator();
            }
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                await _validator.ValidateOrThrowAsync(request);

                var command = new Command(request.CommandText.Trim(), request.Description.Trim(), request.Tags.Select(tag => new Tag(tag.Trim())).ToList(), collection: request.Collection);

                var collection = await _commandRepository.FetchByCollectionAsync(command.Collection);
                if (collection.Exists(x => x.CommandText == command.CommandText && string.Equals(x.Collection, command.Collection)))
                {
                    throw new ValidationError(Resources.DuplicateCommandError);
                }
                command.SetCommandId(collection);
                var commandId = await _commandRepository.SaveAsync(command);

                return new Response
                {
                    CommandId = commandId,
                    CommandText = command.CommandText,
                    Description = command.Description,
                    Tags = command.Tags.Select(tag => tag.Name).ToList()
                };
            }
        }

    }
}
