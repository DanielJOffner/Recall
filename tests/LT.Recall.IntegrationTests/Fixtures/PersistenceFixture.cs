using LT.Recall.Application.Abstractions;
using LT.Recall.Cli.Serialization;
using LT.Recall.Infrastructure;
using LT.Recall.Infrastructure.Persistence.FileSystem;

namespace LT.Recall.IntegrationTests.Fixtures
{
    [SetUpFixture]
    internal class PersistenceFixture
    {
        private readonly ICommandRepository _commandRepository = new JsonFileSystemRepository( new RecallJsonSerializer(), new InfrastructureConfiguration());

        public ICommandRepository Repository => _commandRepository;

        /// <summary>
        /// Reset application state
        /// </summary>
        public void Reset()
        {
            if (File.Exists("recall-state.json"))
                File.Delete("recall-state.json");
        }
    }
}
