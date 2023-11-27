using FluentAssertions;
using LT.Recall.Infrastructure.Persistence.Models;
using LT.Recall.Infrastructure.Persistence.Search;
using NUnit.Framework;

namespace LT.Recall.UnitTests.Infrastructure.Persistence.Search
{
    internal class SearchStringParser_Should
    {
        [TestCaseSource(nameof(TestCases))]
        public void Parse_Search_Strings(string searchString, SearchModel expected)
        {
            // arrange

            // act
            var result = SearchStringParser.Parse(searchString);

            // assert
            result.Should().BeEquivalentTo(expected);
        }

        private static object[] TestCases = new object[]
        {
            new object[]
            {
                " a b c ",
                new SearchModel
                {
                    Terms = new List<string> { "a", "b", "c" }
                }
            },           
            new object[]
            {
                "delete ",
                new SearchModel
                {
                    Terms = new List<string> { "delete" }
                }
            },
            new object[]
            {
                " delete",
                new SearchModel
                {
                    Terms = new List<string> { "delete" }
                }
            }
        };
    }
}
