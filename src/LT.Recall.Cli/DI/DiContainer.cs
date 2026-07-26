using LT.Recall.Application.Abstractions;
using LT.Recall.Cli.Serialization;
using LT.Recall.Infrastructure;
using LT.Recall.Infrastructure.Export;
using LT.Recall.Infrastructure.Import;
using LT.Recall.Infrastructure.Installers;
using LT.Recall.Infrastructure.Installers.Github;
using LT.Recall.Infrastructure.Installers.Url;
using LT.Recall.Infrastructure.Logging;
using LT.Recall.Infrastructure.Persistence.FileSystem;
using Microsoft.Extensions.DependencyInjection;

namespace LT.Recall.Cli.DI
{
    internal class DiContainer : IDisposable
    {
        private IServiceScope _scope;
        public DiContainer(bool xTest)
        {
            var services = BuildServiceCollection(xTest);
#pragma warning disable IL3050
            _scope = services.BuildServiceProvider().CreateScope();
#pragma warning restore IL3050
        }

        public T Get<T>() where T : notnull
        {
            return _scope.ServiceProvider.GetRequiredService<T>();
        }

        public T Get<T>(Type type)
        {
            return (T)_scope.ServiceProvider.GetRequiredService(type);
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
        private IServiceCollection BuildServiceCollection(bool xTest)
        {
            var services = new ServiceCollection();

            services
                .AddTransient<Verbs.Help>()
                .AddTransient<Verbs.Save>()
                .AddTransient<Verbs.Search>()
                .AddTransient<Verbs.Import>()
                .AddTransient<Verbs.Stats>()
                .AddTransient<Verbs.Delete>()
                .AddTransient<Verbs.Install>()
                .AddTransient<Verbs.Export>()
                .AddTransient<Verbs.Version>();

            services
                .AddTransient<Application.Features.Save.Handler>()
                .AddTransient<Application.Features.Search.Handler>()
                .AddTransient<Application.Features.Import.Handler>()
                .AddTransient<Application.Features.Stats.Handler>()
                .AddTransient<Application.Features.Delete.Handler>()
                .AddTransient<Application.Features.Install.Handler>()
                .AddTransient<Application.Features.ListInstallers.Handler>()
                .AddTransient<Application.Features.Export.Handler>();

            services.AddSingleton<IRecallLogger>(new RecallLogger(LogLevel.Info));
            services.AddSingleton(new InfrastructureConfiguration(xTest));

            services.AddScoped<ICommandRepository, JsonFileSystemRepository>();
            services.AddScoped<IJsonSerializer, RecallJsonSerializer>();
            services.AddScoped<IImportFileReaderFactory, ImportFileReaderFactory>();
            services.AddScoped<IExportFileWriterFactory, ExportFileWriterFactory>();
            services.AddScoped<IInstallerFactory, InstallerFactory>();
            services.AddScoped<HttpClient>();
            services.AddScoped<GitHubClient>();
            services.AddScoped<GitHubInstaller>();
            services.AddScoped<UrlInstaller>();
            services.AddScoped<TempFileInstaller>();

            return services;
        }
    }
}
