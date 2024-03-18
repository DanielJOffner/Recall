using FluentAssertions;
using LT.Recall.Application.Features;
using LT.Recall.Cli.Output;
using LT.Recall.Domain.Entities;
using LT.Recall.Domain.ValueObjects;

namespace LT.Recall.IntegrationTests.Tests.Commands
{
    internal class Delete_Should : TestBase
    {
        [TestCase]
        public void Show_Help_Text()
        {
            // arrange

            // act
            var response = ExecuteCommand<Delete.Response>("delete -h");    

            // assert
            response.Message.Trim().Should().Be(@"-c, --collection    	storage collection.
-t, --tags 		tags (command separated).

usage:
	delete -c ""Linux"" -t ""Linux,IO""");
        }

        [TestCase]
        public async Task Treat_Tags_As_Case_Insensitive()
        {
            // arrange
            var c1 = new Command("command text", "description", new List<Tag> { new Tag("tag1")}, collection: "Collection 1", commandId:1);
            await SaveCommands(c1);
            string args = "delete -t \"TAG1\"";

            // act
            var response = ExecuteDeleteSequence<Delete.Response>(args);

            // assert
            response.ResultType.Should().Be(ResultType.Success);
            response.Message.Should().Be("Deleted 1.");
        }

        [TestCase]
        public async Task Teat_Collections_As_Case_Insensitive()
        {
            // arrange
            var c1 = new Command("command text", "description", new List<Tag> { }, collection: "Collection 1", commandId:1);
            await SaveCommands(c1);
            string args = "delete -c \"COLLECTION 1\"";

            // act
            var response = ExecuteDeleteSequence<Delete.Response>(args);

            // assert
            response.ResultType.Should().Be(ResultType.Success);
            response.Message.Should().Be("Deleted 1.");
        }


        [TestCase]
        public async Task Delete_Multiple_Commands_By_Collection()
        {
            // arrange
            var c1 = new Command("command text", "description", new List<Tag> { }, collection: "Collection 1", commandId: 1);
            var c2 = new Command("command text", "description", new List<Tag> { }, collection: "Collection 1", commandId: 2);
            await SaveCommands(c1, c2);
            string args = "delete -c \"Collection 1\"";

            // act
            var response = ExecuteDeleteSequence<Delete.Response>(args);

            // assert
            response.ResultType.Should().Be(ResultType.Success);
            response.Message.Should().Be("Deleted 2.");

            var commands = (await FetchAllCommands()).commands;
            commands!.Count.Should().Be(0);
        }

        [TestCase]
        public async Task Delete_Commands_By_Collection()
        {
            // arrange
            var c1 = new Command("command text", "description", new List<Tag> { }, collection: "Collection 1", commandId: 1);
            var c2 = new Command("command text", "description", new List<Tag> { }, collection: "Collection 2", commandId: 2);
            await SaveCommands(c1, c2);
            string args = "delete -c \"Collection 1\"";

            // act
            var response = ExecuteDeleteSequence<Delete.Response>(args);

            // assert
            response.ResultType.Should().Be(ResultType.Success);
            response.Message.Should().Be("Deleted 1.");

            var commands = (await FetchAllCommands()).commands;
            commands!.Count.Should().Be(1);
            commands.First().Id.Should().Be(c2.Id);
        }

        [TestCase]
        public async Task Delete_Multiple_Commands_By_Tag()
        {
            // arrange
            var c1 = new Command("command text", "description", new List<Tag> { new("Tag1") }, commandId: 1);
            var c2 = new Command("command text", "description", new List<Tag> { new("Tag2") }, commandId: 2);
            await SaveCommands(c1, c2);
            string args = "delete -t Tag1,Tag2";

            // act
            var response = ExecuteDeleteSequence<Delete.Response>(args);

            // assert
            response.ResultType.Should().Be(ResultType.Success);
            response.Message.Should().Be("Deleted 2.");

            var commands = (await FetchAllCommands()).commands;
            commands!.Count.Should().Be(0);
        }

        [TestCase]
        public async Task Delete_Commands_By_Tag()
        {
            // arrange
            var c1 = new Command("command text", "description", new List<Tag> { new("Tag1") }, commandId: 1);
            var c2 = new Command("command text", "description", new List<Tag> { new("Tag2") }, commandId: 2);
            await SaveCommands(c1, c2);
            string args = "delete -t Tag1";

            // act
            var response = ExecuteDeleteSequence<Delete.Response>(args);

            // assert
            response.ResultType.Should().Be(ResultType.Success);
            response.Message.Should().Be("Deleted 1.");

            var commands = (await FetchAllCommands()).commands;
            commands!.Count.Should().Be(1);
            commands.First().Id.Should().Be(c2.Id);
        }

        [TestCaseSource(nameof(DeleteValidationTestCases))]
        public void Validate_Input(string args)
        {
            // arrange

            // act
            var response = ExecuteDeleteSequence<Delete.Response>($"delete {args}");

            // assert
            response.Message.Should().Be("Invalid request: Collection or tags is required.");
            response.ResultType.Should().Be(ResultType.Warning);
        }

        private static object[] DeleteValidationTestCases = new object[][]
        {
            new object[]
            {
                ""
            },
            new object[]
            {
                "-c"
            },
            new object[]
            {
                "-c \"\""
            },
            new object[]
            {
                "-c \"  \""
            },
            new object[]
            {
                "-t"
            },
            new object[]
            {
                "-t \"\""
            },
            new object[]
            {
                "-t \"  \""
            },
            new object[]
            {
                "-t \"\" -c \"\""
            }
        };
    }
}
