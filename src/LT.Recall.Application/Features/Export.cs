using FluentValidation;
using LT.Recall.Application.Abstractions;
using LT.Recall.Application.Extensions;
using LT.Recall.Application.Properties;
using LT.Recall.Domain.Entities;
using MediatR;

namespace LT.Recall.Application.Features
{
    public class Export
    {
        public class Request : IRequest<Response>
        {
            public string FilePath { get; set; } = string.Empty;
        }

        public class Response
        {
            public int Exported { get; set; }
            public string UserFriendlyMessage { get; set; } = string.Empty;
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
            private readonly IExportFileWriterFactory _exportFileWriterFactory;
            private readonly ICommandRepository _commandRepository;
            private readonly Validator _validator;

            public Handler(IExportFileWriterFactory exportFileWriterFactory, ICommandRepository commandRepository)
            {
                _exportFileWriterFactory = exportFileWriterFactory;
                _commandRepository = commandRepository;
                _validator = new Validator();
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                await _validator.ValidateOrThrowAsync(request);
                var writer = _exportFileWriterFactory.GetWriter(request.FilePath);
                var records = await _commandRepository.FetchAllAsync(1, int.MaxValue);
                var exported = writer.Write(request.FilePath, records.commands ?? new List<Command>());
                return new Response { Exported = exported, UserFriendlyMessage = string.Format(Resources.ExportedMessage, exported)};
            }
        }
    }
}
