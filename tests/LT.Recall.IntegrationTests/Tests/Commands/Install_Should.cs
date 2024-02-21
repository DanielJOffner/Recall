using FluentAssertions;
using LT.Recall.Application.Features;
using LT.Recall.Cli.Output;
using LT.Recall.Domain.Entities;

namespace LT.Recall.IntegrationTests.Tests.Commands
{
    internal class Install_Should : TestBase
    {

        [TestCase]
        public async Task Update_Commands_If_Collection_Is_Already_Installed()
        {
            // arrange
            int commandId = 1;
            string commandText = "old command text";
            string commandTextAfterInstall = "uname -a";
            var collectionOrLocation = "linux";

            await SaveCommand(new Command(commandText, "description", collection: collectionOrLocation, commandId: commandId));

            // act
            var response = ExecuteCommand<Install.Response>($"install {collectionOrLocation}");

            // assert
            response.ResultType.Should().Be(ResultType.Success);
            response.Data.Updated.Should().Be(1);

            var command = await FetchCommand(commandId);
            command.Should().NotBeNull();
            command!.CommandText.Should().Be(commandTextAfterInstall);
        }

        [TestCase]
        public async Task Import_Collection_Using_Github_Installer()
        {
            // arrange
            var collectionOrLocation = "linux";

            // act
            var response = ExecuteCommand<Install.Response>($"install {collectionOrLocation}");

            // assert
            response.ResultType.Should().Be(ResultType.Success);
            response.Data.Imported.Should().BeGreaterThan(1);
            response.Data.Updated.Should().Be(0);
            response.Message.Should().Contain("Installation completed successfully");
            response.Data.Installer.Should().Be("Github Installer");

            var commands = await FetchAllCommands();

            commands.commands!.Count.Should().BeGreaterThan(1);
            commands.totalResults!.Should().BeGreaterThan(1);
        }

        [TestCase]
        public async Task Import_Collection_Using_Url_Installer()
        {
            // arrange
            var collectionOrLocation = "https://raw.githubusercontent.com/DanielJOffner/Recall/main/collections/linux.csv";

            // act
            var response = ExecuteCommand<Install.Response>($"install {collectionOrLocation}");

            // assert
            response.ResultType.Should().Be(ResultType.Success);
            response.Data.Imported.Should().BeGreaterThan(1);
            response.Data.Updated.Should().Be(0);
            response.Message.Should().Contain("Installation completed successfully");
            response.Data.Installer.Should().Be("Url Installer");

            var commands = await FetchAllCommands();

            commands.commands!.Count.Should().BeGreaterThan(1);
            commands.totalResults!.Should().BeGreaterThan(1);
        }

    }
}
