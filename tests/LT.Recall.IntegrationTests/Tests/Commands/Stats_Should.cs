using FluentAssertions;
using LT.Recall.Application.Extensions;
using LT.Recall.Application.Features;
using LT.Recall.Domain.Entities;
using LT.Recall.Domain.ValueObjects;

namespace LT.Recall.IntegrationTests.Tests.Commands
{
    internal class Stats_Should : TestBase
    {

        [TestCase]
        public async Task Show_Total_Number_Of_Commands_In_Storage()
        {
            // arrange
            await SaveCommand(new Command("Command Text1", "Description1", commandId: 1));
            await SaveCommand(new Command("Command Text2", "Description2", commandId: 2));

            // act
            var response = ExecuteCommand<Stats.Response>("stats");

            // assert
            response.Data.Totals.TotalCommands.Should().Be(2);
        }

        [TestCase]
        public async Task Show_Total_Size_Of_Commands_In_Storage()
        {
            // arrange
            await SaveCommand(new Command("Command Text1", "Description1", commandId: 1));
            await SaveCommand(new Command("Command Text2", "Description2", commandId: 2));

            // act
            var response = ExecuteCommand<Stats.Response>("stats");

            // assert
            var totalSize = (await FetchAllCommands()).commands?.Sum(c => c.Size);
            totalSize.Should().BeGreaterThan(0);
            response.Data.Totals.TotalSize.Should().Be(totalSize!.Value.ToBytesDisplayValue());
        }

        [TestCase]
        public async Task Show_Size_And_Storage_Size_For_All_Collections()
        {
            // arrange
            var c1 = new Command("Command Text1", "Description1", commandId: 1, collection: "Default");
            var c2 = new Command("Command Text2", "Description1", commandId: 2, collection: "Company X");
            await SaveCommands(c1, c2);

            // act
            var response = ExecuteCommand<Stats.Response>("stats");

            // assert
            response.Data.Collections.Count.Should().Be(2);
                          
            response.Data.Collections[0].Collection.Should().Be("default");
            response.Data.Collections[0].Count.Should().Be(1);
            response.Data.Collections[0].Size.Should().Be(c1.Size.ToBytesDisplayValue());

            response.Data.Collections[1].Collection.Should().Be("company x");
            response.Data.Collections[1].Count.Should().Be(1);
            response.Data.Collections[1].Size.Should().Be(c2.Size.ToBytesDisplayValue());
        }

        [TestCase]
        public async Task Evaluate_Collections_As_case_Insensitive()
        {
            // arrange
            var c1 = new Command("Command Text1", "Description1", commandId: 1, collection: "default");
            var c2 = new Command("Command Text2", "Description2", commandId: 2, collection: "DEFAULT");
            await SaveCommands(c1, c2);

            // act
            var response = ExecuteCommand<Stats.Response>("stats");

            // assert
            response.Data.Collections.Count.Should().Be(1);

            response.Data.Collections[0].Collection.Should().Be("default");
            response.Data.Collections[0].Count.Should().Be(2);
            response.Data.Collections[0].Size.Should().Be((c1.Size + c2.Size).ToBytesDisplayValue());
        }

        [TestCase]
        public async Task Order_Collections_By_Total_Count_Descending()
        {
            // arrange
            var c1 = new Command("command text 1", "d", commandId: 1, collection: "collection 1");
            var c2 = new Command("command text 2", "d", commandId: 2, collection: "collection 2");
            var c3 = new Command("command text 3", "d", commandId: 3, collection: "collection 2");
            await SaveCommands(c1, c2, c3);

            // act
            var response = ExecuteCommand<Stats.Response>("stats");

            // assert
            response.Data.Collections.Count.Should().Be(2);
            response.Data.Collections.Select(x => x.Collection).Should().ContainInOrder("collection 2", "collection 1");
        }

        [TestCase]
        public async Task Show_Size_And_Storage_Size_For_All_Tags()
        {
            // arrange
            var c1 = new Command("Command Text1", "Description1", new List<Tag> { new("Tag1") }, commandId:1);
            var c2 = new Command("Command Text2", "Description2", new List<Tag> { new("Tag2") }, commandId:2);
            await SaveCommands(c1, c2);

            // act
            var response = ExecuteCommand<Stats.Response>("stats");

            // assert
            response.Data.Tags.Count.Should().Be(2);

            response.Data.Tags[0].Tag.Should().Be("tag1");
            response.Data.Tags[0].Count.Should().Be(1);
            response.Data.Tags[0].Size.Should().Be(c1.Size.ToBytesDisplayValue());

            response.Data.Tags[1].Tag.Should().Be("tag2");
            response.Data.Tags[1].Count.Should().Be(1);
            response.Data.Tags[1].Size.Should().Be(c2.Size.ToBytesDisplayValue());
        }

        [TestCase]
        public async Task Evaluate_Tag_Stats_As_Case_Insensitive()
        {
            // arrange
            var c1 = new Command("Command Text1", "Description1", new List<Tag> { new("tag1") }, commandId:1);
            var c2 = new Command("Command Text2", "Description2", new List<Tag> { new("TAG1") }, commandId:2);
            await SaveCommands(c1, c2);

            // act
            var response = ExecuteCommand<Stats.Response>("stats");

            // assert
            response.Data.Tags.Count.Should().Be(1);

            response.Data.Tags[0].Tag.Should().Be("tag1");
            response.Data.Tags[0].Count.Should().Be(2);
            response.Data.Tags[0].Size.Should().Be((c1.Size + c2.Size).ToBytesDisplayValue());
        }

        [TestCase]
        public async Task Order_Tags_By_Total_Count_Descending()
        {
            // arrange
            var c1 = new Command("c 1", "d", new List<Tag> { new("tag1") }, commandId: 1);
            var c2 = new Command("c 2", "d", new List<Tag> { new("tag2") }, commandId: 2);
            var c3 = new Command("c 2", "d", new List<Tag> { new("tag2") }, commandId: 3);
            await SaveCommands(c1, c2, c3);

            // act
            var response = ExecuteCommand<Stats.Response>("stats");

            // assert
            response.Data.Tags.Count.Should().Be(2);
            response.Data.Tags.Select(x => x.Tag).Should().ContainInOrder("tag2", "tag1");
        }
    }
}
