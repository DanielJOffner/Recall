using FluentValidation;
using LT.Recall.Application.Abstractions;
using LT.Recall.Application.Extensions;
using LT.Recall.Application.Properties;
using MediatR;

namespace LT.Recall.Application.Features
{
    public class Install
    {
        public class Request : IRequest<Response>
        {
            public string CollectionOrLocation { get; set; } = string.Empty;
        }

        public class Response
        {
            public int Updated { get; init; }
            public int Imported { get; init; }
            public string UserFriendlyMessage { get; init; } = string.Empty;
            public string Installer { get; init; } = string.Empty;
        }

        private class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.CollectionOrLocation).NotEmpty().WithMessage(string.Format(Resources.InputRequiresValueError, nameof(Request.CollectionOrLocation)));
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IInstallerFactory _installerFactory;
            private readonly Validator _validator = new Validator();
            public Handler(IInstallerFactory installerFactory)
            {
                _installerFactory = installerFactory;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                await _validator.ValidateOrThrowAsync(request);

                var installer = _installerFactory.GetInstaller(request.CollectionOrLocation);
                var result = await installer.Install(request.CollectionOrLocation);
                return new Response
                {
                    Updated = result.updated,
                    Imported = result.inserted,
                    UserFriendlyMessage = string.Format(Resources.InstallSuccessMessage, result.updated, result.inserted),
                    Installer = installer.Name
                };
            }
        }
    }
}
