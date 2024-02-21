using FluentAssertions;
using LT.Recall.Application.Features;
using LT.Recall.Domain.Entities;
using LT.Recall.Domain.ValueObjects;

namespace LT.Recall.IntegrationTests.Tests.Commands
{
    internal class SearchShould : TestBase
    {
        private static string _commandText = "command_text";

        private static string _description = "description";

        [TestCase]
        public async Task Return_No_Match()
        {
            // arrange
            await SaveCommand(new Command("Command Text", "Description", new List<Tag> { new("Tag1") }));

            // act
            var response = ExecuteCommand<Search.Response>("search foo bar");

            // assert
            response.Data.TotalResults.Should().Be(0);
            response.Data.SearchResults.Count.Should().Be(0);
            response.Message.Should().Be("No commands found matching 'foo bar'"); 
        }

        [TestCase]
        public async Task Match_On_Terms_Appearing_In_Any_Order()
        {
            // arrange
            await SaveCommand(new Command("Command Text", _description, commandId:1));
            await SaveCommand(new Command("Text Command", _description, commandId:2));

            // act
            var response1 = ExecuteCommand<Search.Response>("search Text Command");
            var response2 = ExecuteCommand<Search.Response>("search Command Text");

            // assert
            response1.Data.TotalResults.Should().Be(2);
            response2.Data.TotalResults.Should().Be(2);
            response1.Message.Should().Be("Found 2 command(s) matching 'Text Command'");
            response2.Message.Should().Be("Found 2 command(s) matching 'Command Text'");
        }


        [TestCase]
        public async Task Match_On_Description()
        {
            // arrange
            await SaveCommand(new Command(_commandText, "description_1", commandId:1));
            await SaveCommand(new Command(_commandText, "description_2", commandId: 2));

            // act
            var response = ExecuteCommand<Search.Response>("search description_2");

            // assert
            response.Data.TotalResults.Should().Be(1);
            response.Data.SearchResults.First().Description.Should().Be("description_2");
        }

        [TestCase]
        public async Task Match_On_Command_Text()
        {
            // arrange
            await SaveCommand(new Command("command_text_1", _description, commandId: 1));
            await SaveCommand(new Command("command_text_2", _description, commandId: 2));

            // act
            var response = ExecuteCommand<Search.Response>("search command_text_2");

            // assert
            response.Data.TotalResults.Should().Be(1);
            response.Data.SearchResults.First().CommandText.Should().Be("command_text_2");
        }

        [TestCase]
        public async Task Match_On_Tags()
        {
            // arrange
            await SaveCommand(new Command(_commandText, _description, new List<Tag> { new("Tag1")}, commandId:1));
            await SaveCommand(new Command(_commandText, _description, new List<Tag> { new("Tag2") }, commandId:2));

            // act
            var response = ExecuteCommand<Search.Response>("search Tag2");

            // assert
            response.Data.TotalResults.Should().Be(1);
            response.Data.SearchResults.First().Tags.First().Should().Be("Tag2");
        }

        [TestCase]
        public async Task Be_Case_Insensitive_On_Command_Text_Matches()
        {
            // arrange
            await SaveCommand(new Command("Command Text", _description, new List<Tag>(), commandId: 1));
            await SaveCommand(new Command("command TExT", _description, new List<Tag>(), commandId: 2));

            // act
            var response1 = ExecuteCommand<Search.Response>("search COMMAND TEXT");
            var response2 = ExecuteCommand<Search.Response>("search command text");

            // assert
            response1.Data.TotalResults.Should().Be(2);
            response2.Data.TotalResults.Should().Be(2);
            response1.Data.SearchResults.Select(x => x.CommandId).Should().ContainInOrder(1, 2);
            response2.Data.SearchResults.Select(x => x.CommandId).Should().ContainInOrder(1, 2);
        }

        [TestCase]
        public async Task Be_Case_Insensitive_On_Description_Matches()
        {
            // arrange
            await SaveCommand(new Command(_commandText, "Description", new List<Tag>(), commandId: 1));
            await SaveCommand(new Command(_commandText, "dESCRIPTION", new List<Tag>(), commandId: 2));

            // act
            var response1 = ExecuteCommand<Search.Response>("search DESCRIPTION");
            var response2 = ExecuteCommand<Search.Response>("search description");

            // assert
            response1.Data.TotalResults.Should().Be(2);
            response2.Data.TotalResults.Should().Be(2);
            response1.Data.SearchResults.Select(x => x.CommandId).Should().ContainInOrder(1, 2);
            response2.Data.SearchResults.Select(x => x.CommandId).Should().ContainInOrder(1, 2);
        }

        [TestCase]
        public async Task Be_Case_Insensitive_On_Tag_Matches()
        {
            // arrange
            await SaveCommand(new Command(_commandText, _description, new List<Tag> { new("Tag") }, commandId: 1));
            await SaveCommand(new Command(_commandText, _description, new List<Tag> { new("TAG") }, commandId: 2));

            // act
            var response1 = ExecuteCommand<Search.Response>("search tag");
            var response2 = ExecuteCommand<Search.Response>("search TAG");

            // assert
            response1.Data.TotalResults.Should().Be(2);
            response2.Data.TotalResults.Should().Be(2);
            response1.Data.SearchResults.Select(x => x.CommandId).Should().ContainInOrder(1, 2);
            response2.Data.SearchResults.Select(x => x.CommandId).Should().ContainInOrder(1, 2);
        }


        /// <summary>
        /// Results matching more search terms should appear first
        /// Occurrences are considered on tags, description and command text (command.SearchableText)
        /// </summary>
        /// <returns></returns>
        [TestCaseSource(nameof(_orderTestCases))]
        public async Task Order_Results_By_Search_Term_Occurrences(List<Command> testData, string searchTerm, List<int> expectedOrder)
        {
            // arrange
            foreach (var command in testData)
            {
                await SaveCommand(command);
            }

            // act
            var response = ExecuteCommand<Search.Response>($"search {searchTerm}");

            // assert
            response.Data.SearchResults.Select(x => x.CommandId).Should().ContainInOrder(expectedOrder);
        }


        private static object[] _orderTestCases = new object[]
        {
            new object[]
            {
                new List<Command>()
                {
                    new(_commandText, _description, new List<Tag> { new("term") }, commandId: 1), // 1 occurrence of term
                    new(_commandText, "term", new List<Tag> { new("term") }, commandId: 2), // 2 occurrence of term
                    new("term", "term", new List<Tag> { new("term") }, commandId: 3), // 3 occurrence of term
                },
                "term",
                new List<int> { 3, 2, 1 }
            },
            new object[]
            {
                new List<Command>()
                {
                    new(_commandText, _description, new List<Tag> { new("TERM") }, commandId: 1), // 1 occurrence of term (uppercase) 
                    new(_commandText, "TERM", new List<Tag> { new("TERM") }, commandId : 2), // 2 occurrence of term (uppercase)
                    new("TERM", "TERM", new List<Tag> { new("TERM") }, commandId : 3), // 3 occurrence of term (uppercase)
                },
                "term",
                new List<int> {  3, 2, 1  }
            }
        };
    }
}
