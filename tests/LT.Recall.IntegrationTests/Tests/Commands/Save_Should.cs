using FluentAssertions;
using LT.Recall.Application.Features;
using LT.Recall.Cli.Output;
using LT.Recall.Domain.Entities;

namespace LT.Recall.IntegrationTests.Tests.Commands
{
    internal class Save_Should : TestBase
    {

        [TestCase]
        public async Task Assign_The_Next_CommandId_In_The_Collection()
        {
            // arrange
            var collection = new List<Command>()
            {
                new("command text 1", "description", collection: "Collection 1", commandId: 1),
                new("command text 2", "description", collection: "Collection 1", commandId: 2),
            };
            await SaveCommands(collection.Select(x => x).ToArray());

            // act
            var response = ExecuteSaveSequence<Save.Response>("description", "command text 3", args: new List<string>()
            {
                "--collection",
                "\"Collection 1\""
            });

            // assert
            response.ResultType.Should().Be(ResultType.Success);
            var command = (await FetchAllCommands()).commands!.Last();
            command.CommandId.Should().Be(3);
            command.Id.Should().Be("Collection 1_3");
        }

        [TestCase]
        public async Task Save_New_Commands_To_The_Provided_Collection()
        {
            // arrange
            string collection = "\"Company X\"";
            
            // act
            var response = ExecuteSaveSequence<Save.Response>("description", "command text", args: new List<string>()
            {
                "--collection",
                collection
            });
            
            // assert
            response.ResultType.Should().Be(ResultType.Success);
            var command = (await FetchAllCommands()).commands!.First();
            command.Collection.Should().Be("Company X");
        }


        [TestCase]
        public async Task Save_New_Commands_To_The_Default_Collection_If_No_Option_Is_Provided()
        {
            // arrange
            string defaultCollectionName = "User Collection";

            // act
            var response = ExecuteSaveSequence<Save.Response>("description", "command text");

            // assert
            response.ResultType.Should().Be(ResultType.Success);
            var command = (await FetchAllCommands()).commands!.First();
            command.Collection.Should().Be(defaultCollectionName);
        }

        [TestCase]
        public void Truncate_Output_For_Long_Command_Text()
        {
            // arrange
            string description = "description";
            string commandText =
                @"Get-ChildItem .\\ -include bin,obj -Recurse | ForEach-Object ($_) { Remove-Item $_.FullName -Force -Recurse; echo ""Deleted"" + $_.FullName; }";

            // act
            var response = ExecuteSaveSequence<Save.Response>(description, commandText);

            // assert
            response.Message.Should().Be(@"Saved command: Get-ChildItem .\\ -include bin,obj -Recurse | ForE...");
        }

        [TestCase]
        public async Task Treat_Quotes_As_Literal_Strings()
        {
            // arrange
            string description = "description";
            string commandText = @" ""double quotes"" 'single quotes' ";

            // act
            var response = ExecuteSaveSequence<Save.Response>(description, commandText);

            // assert
            var command = (await FetchAllCommands()).commands!.First();
            response.Message.Should().Be(@"Saved command:  ""double quotes"" 'single quotes' ");
            command.CommandText.Should().Be(@"""double quotes"" 'single quotes'");
        }
        
        [TestCaseSource(nameof(SaveTestCases))]
        public async Task Save_Commands(string description, string commandText, string tags)
        {
            // act
            var response = ExecuteSaveSequence<Save.Response>(description, commandText, tags);

            // assert
            response.ResultType.Should().Be(ResultType.Success);
            response.Message.Should().Be($"Saved command: {commandText}");
            await ValidateCommandSaved(description, commandText, tags);
        }

        [TestCase("command text", "command text")]
        [TestCase("command text", "   command text   ")]
        [TestCase("command text", "   command text")]
        public void Prevent_Duplicate_Command_Text_Within_The_Same_Collection(string commandText1, string commandText2)
        {
            // string
            string description = Guid.NewGuid().ToString();

            // act
            var response1 = ExecuteSaveSequence<Save.Response>(description, commandText1, args: new List<string>()
            {
                "--collection",
                "collection1"
            });
            var response2 = ExecuteSaveSequence<Save.Response>(description, commandText2, args: new List<string>()
            {
                "--collection",
                "collection1"
            });

            // assert
            response1.ResultType.Should().Be(ResultType.Success);
            response2.ResultType.Should().Be(ResultType.Warning);
            response1.Message.Should().Be($"Saved command: {commandText1}");
            response2.Message.Should().Be($"Invalid request: A command with the same command text already exists within the collection.");
        }

        [TestCase]
        public void Allow_Duplicate_Command_Text_Between_Collections()
        {
            // string
            string description = Guid.NewGuid().ToString();
            string commandText = "duplicate";

            // act
            var response1 = ExecuteSaveSequence<Save.Response>(description, commandText, args: new List<string>()
            {
                "--collection",
                "collection1"
            });
            var response2 = ExecuteSaveSequence<Save.Response>(description, commandText, args: new List<string>()
            {
                "--collection",
                "collection2"
            });

            // assert
            response1.ResultType.Should().Be(ResultType.Success);
            response2.ResultType.Should().Be(ResultType.Success);
            response1.Message.Should().Be($"Saved command: {commandText}");
            response1.Message.Should().Be($"Saved command: {commandText}");
        }

        [TestCaseSource(nameof(ValidationTestCases))]
        public void Validate_Input(string description, string commandText, string expectedOutput)
        {
            // act
            var response = ExecuteSaveSequence<Save.Response>(description, commandText);

            // assert
            response.ResultType.Should().Be(ResultType.Warning);
            response.Message.Should().Be(expectedOutput);
        }

        private async Task ValidateCommandSaved(string description, string commandText, string expectedTags)
        {
            var command = (await FetchAllCommands()).commands!.First();
            command.Description.Should().Be(description);
            command.CommandText.Should().Be(commandText);
            string.Join(",", command.Tags.Select(x => x.Name)).Should().Be(expectedTags);
            command.Id.Should().NotBeEmpty();
            command.HasId.Should().BeTrue();
            command.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        private static object[] ValidationTestCases = new object[][]
        {
            new object[]
            {
                "description",
                string.Empty,
                "Invalid request: Command is required"
            },
            new object[]
            {
                "description",
                "   ",
                "Invalid request: Command is required"
            },
            new object[]
            {
                "description",
                "",
                "Invalid request: Command is required"
            },
            new object[]
            {
                string.Empty,
                "command text",
                "Invalid request: Description is required"
            },
            new object[]
            {
                string.Empty,
                string.Empty,
                "Invalid request: Command is required, Description is required"
            },
            new object[]
            {
                "   ",
                "   ",
                "Invalid request: Command is required, Description is required"
            },
        };

        
        private static object[] SaveTestCases = new object[][]
        {
            new object[]
            {
                "description",
                "command text",
                string.Empty,
            },
            new object[]
            {
                "Get Linux Version",
                "cat /etc/os-release",
                string.Empty,
            },
            new object[]
            {
                "description",
                "command text",
                "Tag1,Tag2",
            },
        };
    }
}
