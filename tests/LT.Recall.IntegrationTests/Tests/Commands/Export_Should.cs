using FluentAssertions;
using LT.Recall.Application.Features;
using LT.Recall.Cli.Output;
using LT.Recall.Domain.Entities;

namespace LT.Recall.IntegrationTests.Tests.Commands
{
    internal class Export_Should : TestBase
    {
        [TestCase]
        public void Throw_Validation_Error_On_Missing_File_Path()
        {
            // arrange
            var filePath = "";

            // act
            var response = ExecuteCommand<Import.Response>($"export -p {filePath}");

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
            var response = ExecuteCommand<Import.Response>($"export -p {filePath}");

            // assert 
            response.ResultType.Should().Be(ResultType.Warning);
            response.Message.Should().Contain("Invalid request: Unsupported file type");
        }

        [TestCase]
        public async Task Export_To_Csv_File()
        {
            // arrange
            var c1 = new Command("command 1", "description", collection: "Collection1", commandId: 1);
            var c2 = new Command("command 2", "description", collection: "Collection1", commandId: 2);
            await SaveCommands(c1, c2);
            var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv");

            // act
            var response = ExecuteCommand<Import.Response>($"export -p {filePath}");
            
            // assert
            response.ResultType.Should().Be(ResultType.Success);
            response.Message.Should().Be("Exported 2.");
        }
    }
}
