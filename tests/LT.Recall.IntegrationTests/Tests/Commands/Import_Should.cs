using FluentAssertions;
using LT.Recall.Application.Errors.Codes;
using LT.Recall.Application.Features;
using LT.Recall.Cli.Output;
using LT.Recall.Domain.Entities;
using LT.Recall.Domain.ValueObjects;
using LT.Recall.Infrastructure.Errors.Codes;
using LT.Recall.IntegrationTests.Extensions;
using LT.Recall.IntegrationTests.TestFiles;

namespace LT.Recall.IntegrationTests.Tests.Commands
{
    internal class Import_Should : TestBase
    {
        [TestCase]
        public async Task Add_Commands_From_Csv_File()
        {
            // arrange
            var filePath = TestFileReader.GetFilePath(TestFileType.Import, "001_simple-import.csv");

            // act
            var response = ExecuteCommand<Import.Response>($"import -p {filePath}");

            // assert 
            response.ResultType.Should().Be(ResultType.Success);
            response.Data.Imported.Should().Be(3);

            var commands = await FetchAllCommands();
            commands.commands!.Count.Should().Be(3);
            commands.totalResults!.Should().Be(3);

            var command = commands.commands!.First();
            command.CommandId.Should().Be(1);
            command.Id.Should().Be("Collection1_1");
            command.CommandText.Should().Be("Command Text");
            command.Description.Should().Be("Description");
            command.Tags.Select(x => x.Name).Should().BeEquivalentTo(new List<string> { "Tag1", "Tag2" });
            command.Collection.Should().Be("Collection1");
        }

        [TestCase]
        public async Task Update_Commands_From_Csv_File()
        {
            // arrange
            await SaveCommand(new Command("old command text", "description", collection: "Collection1", commandId: 1));

            var filePath = TestFileReader.GetFilePath(TestFileType.Import, "001_simple-import.csv");

            // act
            var response = ExecuteCommand<Import.Response>($"import -p {filePath}");

            // assert 
            response.ResultType.Should().Be(ResultType.Success);
            response.Data.Imported.Should().Be(2);
            response.Data.Updated.Should().Be(1);

            var commands = await FetchAllCommands();
            commands.commands!.Count.Should().Be(3);
            commands.totalResults!.Should().Be(3);

            var command = commands.commands!.First();
            command.CommandId.Should().Be(1);
            command.CommandText.Should().Be("Command Text"); // modified from "old command text"
        }

        [TestCase]
        public void Throw_Validation_Error_On_Missing_File_Path()
        {
            // arrange
            var filePath = "";

            // act
            var response = ExecuteCommand<Import.Response>($"import -p {filePath}");

            // assert 
            response.ResultType.Should().Be(ResultType.Warning);
            response.Message.Should().Be("Invalid request: File path is required.");
        }

        [TestCase]
        public void Throw_Validation_Error_On_Unsupported_File_Type()
        {
            // arrange
            var filePath = @"C:\files\invalid-file-type.xlsx";

            // act
            var response = ExecuteCommand<Import.Response>($"import -p {filePath}");

            // assert 
            response.ResultType.Should().Be(ResultType.Warning);
            response.Message.Should().Contain("Invalid request: Unsupported file type");
        }

        [TestCase]
        public void Fail_If_Import_File_Contains_Duplicate_Ids()
        {
            // arrange
            var filePath = TestFileReader.GetFilePath(TestFileType.Import, "002_contains-duplicate-id.csv");

            // act
            var response = ExecuteCommand<Import.Response>($"import -p {filePath}");

            // assert 
            response.ResultType.Should().Be(ResultType.Warning);
            response.Message.Should().Contain("A command with the same command text already exists within the collection.");
        }

        [TestCase]
        public async Task Rollback_If_Any_Row_Fails_To_Import()
        {
            // arrange
            var c1 = new Command("Command Text", "Description", new List<Tag> { new("Tag1"), new("Tag2") }, collection: "collection1" , commandId: 1);
            var c2 = new Command("Command Text", "Description", new List<Tag> { new("Tag2"), new("Tag3") }, collection: "collection2" , commandId: 2);
            await SaveCommands(c1, c2);
            var filePath = TestFileReader.GetFilePath(TestFileType.Import, "002_contains-duplicate-id.csv");
            var commandsBeforeImport = (await FetchAllCommands()).commands;

            // act
            var response = ExecuteCommand<Import.Response>($"import -p {filePath}");

            // assert 
            response.ResultType.Should().Be(ResultType.Warning);

            var commandsAfterImport  = (await FetchAllCommands()).commands;
            commandsBeforeImport.Should().BeEquivalentTo(commandsAfterImport); // state should be rolled back
        }
    }
}
