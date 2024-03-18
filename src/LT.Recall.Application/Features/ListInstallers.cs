using LT.Recall.Application.Abstractions;

namespace LT.Recall.Application.Features
{
    public class ListInstallers
    {
        public class Request
        {
        }

        public class Response
        {
            public class Installer
            {
                public string Name { get; init; } = string.Empty;

                public List<string> Collections { get; init; } = new List<string>();
            }

            public List<Installer> Installers { get; init; } = new List<Installer>();
        }


        public class Handler
        {
            private readonly IInstallerFactory _installerFactory;
            public Handler(IInstallerFactory installerFactory)
            {
                _installerFactory = installerFactory;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var response = new Response();
                var installers = _installerFactory.ListInstallers();
                foreach (var installer in installers)
                {
                    response.Installers.Add(new Response.Installer
                    {
                        Name = installer.Name,
                        Collections = await installer.ListCollections()
                    });
                }
                return response;
            }
        }
    }
}
