using FluentAssertions;

namespace LT.Recall.IntegrationTests.Tests.Cli
{
    internal class Cli_Should : TestBase
    {
        [TestCase]
        public void Treat_Verbs_As_Case_Insensitive()
        {
            // arrange
            var lowerCase = new[] { "help" };
            var upperCase = new[] { "HELP" };

            // act
            var lowerCaseOutput = ExecuteCommand<object>(lowerCase);
            var upperCaseOutput = ExecuteCommand<object>(upperCase);

            // assert
            lowerCaseOutput.Should().BeEquivalentTo(upperCaseOutput);
            lowerCaseOutput.Message.Should().Contain("-h, --help\tShow help."); 
            upperCaseOutput.Message.Should().Contain("-h, --help\tShow help.");
        }


        [TestCase]
        public void Create_State_File_Automatically()
        {
            // arrange
            string stateFileName = "state.json";

            if(File.Exists(stateFileName))
                File.Delete(stateFileName);

            // act
            ExecuteSaveSequence<object>("description", "command text");

            // assert
            File.Exists(stateFileName).Should().BeTrue();
        }


        [TestCaseSource(nameof(HelpTestCases))]
        public void Show_Localized_Help_Text(string command, string expectedResponse)
        {
            // arrange

            // act
            var output = ExecuteCommand<object>(command);

            // assert
            output.Message.Trim().Should().Be(expectedResponse.Trim());
        }

        private static object[] HelpTestCases = new object[]
        {
            new object[]
            {
                "help",
                @"v1.0.0.0

-x, --xtest	Start in test mode.
-v, --verbose	Enable verbose output.
-h, --help	Show help.

save	Save a new command.
search	Search commands.
delete	Delete commands.
import	Import commands.
export	Export commands.
stats	Storage statistics.
install	Install collections."
            },
            new object[]
            {
                "save --help",
                @"-c, --collection	Storage collection.

usage: 
    save -c ""infrastructure"""
            },
        };
    }
}
