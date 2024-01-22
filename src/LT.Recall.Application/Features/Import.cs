using FluentValidation;
using LT.Recall.Application.Abstractions;
using LT.Recall.Application.Extensions;
using LT.Recall.Application.Properties;
using MediatR;

namespace LT.Recall.Application.Features
{
    public class Import
    {
        public class Request : IRequest<Response>
        {
            public string FilePath { get; init; } = string.Empty;
        }

        public class Response
        {
            public int Updated { get; init; }
            public int Imported { get; init; }
            public string UserFriendlyMessage { get; init; } = string.Empty;
        }

        private class Validator : AbstractValidator<Request>
        {
            private readonly List<string> _supportedExtensions = new() { ".csv" };

            public Validator()
            {
                this.ClassLevelCascadeMode = CascadeMode.Stop;
                RuleFor(x => x.FilePath).NotEmpty().WithMessage(Resources.FilePathIsRequiredError);
                RuleFor(x => x.FilePath).Must(p => _supportedExtensions.Contains(Path.GetExtension(p).ToLowerInvariant()))
                    .WithMessage(string.Format(Resources.UnsupportedFileTypeError, string.Join(",", _supportedExtensions)));
            }
        }


        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IImportFileReaderFactory _importFileReaderFactory;
            private readonly ICommandRepository _commandRepository;
            private readonly Validator _validator;
            public Handler(IImportFileReaderFactory importFileReaderFactory, ICommandRepository commandRepository)
            {
                _validator = new Validator();
                _importFileReaderFactory = importFileReaderFactory;
                _commandRepository = commandRepository;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                await _validator.ValidateOrThrowAsync(request);

                var reader = _importFileReaderFactory.GetReader(request.FilePath);
                var commands = reader.Read(request.FilePath);
                var result = await _commandRepository.BulkUpsert(commands);

                return new Response
                {
                    Imported = result.inserted,
                    Updated = result.updated,
                    UserFriendlyMessage = string.Format(Resources.ImportedMessage, result.inserted, result.updated)
                };
            }
        }

    }
}
