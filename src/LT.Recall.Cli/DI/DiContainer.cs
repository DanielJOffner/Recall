using LT.Recall.Application.Abstractions;
using LT.Recall.Infrastructure;
using LT.Recall.Infrastructure.Export;
using LT.Recall.Infrastructure.Import;
using LT.Recall.Infrastructure.Logging;
using LT.Recall.Infrastructure.Persistence.FileSystem;
using LT.Recall.Infrastructure.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Save = LT.Recall.Application.Features.Save;

namespace LT.Recall.Cli.DI
{
    internal class DiContainer : IDisposable
    {
        private IServiceScope _scope;
        public DiContainer()
        {
            var services = BuildServiceCollection();
            _scope = services.BuildServiceProvider().CreateScope();
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
        private IServiceCollection BuildServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Save).Assembly);
            });
            services
                .AddTransient<Verbs.Help>()
                .AddTransient<Verbs.Save>()
                .AddTransient<Verbs.Search>()
                .AddTransient<Verbs.Import>()
                .AddTransient<Verbs.Stats>()
                .AddTransient<Verbs.Delete>()
                .AddTransient<Verbs.Export>();

            services.AddSingleton<IRecallLogger>(new RecallLogger(LogLevel.Info));
            services.AddSingleton<InfrastructureConfiguration>();

            services.AddScoped<ICommandRepository, JsonFileSystemRepository>();
            services.AddScoped<IJsonSerializer, RecallJsonSerializer>();
            services.AddScoped<IImportFileReaderFactory, ImportFileReaderFactory>();
            services.AddScoped<IExportFileWriterFactory, ExportFileWriterFactory>();
            return services;
        }
    }
}
