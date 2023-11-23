using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using LT.Recall.Application.Abstractions;
using LT.Recall.Application.Errors;
using LT.Recall.Application.Extensions;
using LT.Recall.Application.Properties;
using LT.Recall.Domain.Entities;
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

                int imported = 0;
                int updated = 0;
                try
                {
                    await _commandRepository.BeginTransactionAsync();
                    var reader = _importFileReaderFactory.GetReader(request.FilePath);
                    var commands = reader.Read(request.FilePath);

                    foreach (var command in commands)
                    {
                        ThrowIfDuplicate(commands, command);

                        command.GenerateIdIfEmpty();
                        
                        var exists = await _commandRepository.ExistsAsync(command.Id ?? string.Empty);
                        if (exists)
                        {
                            await _commandRepository.UpdateAsync(command);
                            updated++;
                        }
                        else
                        {
                            await _commandRepository.SaveAsync(command);
                            imported++;
                        }
                    }
                    await _commandRepository.CommitTransactionAsync();
                }
                catch
                {
                    await _commandRepository.RollbackTransactionAsync();
                    throw;
                }
                return new Response
                {
                    Imported = imported,
                    Updated = updated,
                    UserFriendlyMessage = string.Format(Resources.ImportedMessage, imported, updated)
                };
            }

            private static void ThrowIfDuplicate(List<Command> commands, Command command)
            {
                if (commands.Count(x => x.CommandId == command.CommandId && string.Equals(x.Collection, command.Collection, StringComparison.InvariantCultureIgnoreCase)) > 1)
                {
                    throw new ValidationError(Resources.DuplicateCommandError);
                }
            }
        }

    }
}
