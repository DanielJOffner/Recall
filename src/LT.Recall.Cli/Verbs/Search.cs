using LT.Recall.Cli.Options;
using LT.Recall.Cli.Output;
using LT.Recall.Cli.Properties;
using LT.Recall.Cli.Verbs.Base;
using TextCopy;

namespace LT.Recall.Cli.Verbs
{
    internal class Search : Verb<Search.Options>
    {
        private readonly Application.Features.Search.Handler _searchHandler;

        public Search(Application.Features.Search.Handler searchHandler)
        {
            _searchHandler = searchHandler;
        }

        public class Options : Program.Options { }


        protected override string HelpText => Resources.SearchHelpText;

        protected override async Task<CliResult> ExecuteInner(List<string> args, Options options)
        {
            int page = 1;
            int pageSize = options.XTest ? 1000 : Console.WindowHeight - 4;
            var searchString = string.Join(" ", args);

            if (!options.XTest)
            {
                ClearConsoleArea();
            }

            return await MakeUserSelection(searchString, page, pageSize, options);
        }

        private async Task<CliResult> MakeUserSelection(string searchString, int page, int pageSize, IOptions options)
        {
            int cursor = 0;
            var response = await _searchHandler.Handle(new Application.Features.Search.Request
            {
                SearchString = searchString,
                PageSize = pageSize,
                Page = page
            }, CancellationToken.None);

            if (options.XTest)
            {
                return new CliResult(response.UserFriendlyMessage, ResultType.Success, response);
            }

            while (true)
            {
                ClearScreenArea();

                WriteHeaderRow();

                if (response.TotalResults == 0)
                {
                    Console.ForegroundColor = Theme.Warning;
                    Console.Write(response.UserFriendlyMessage);
                }
                else if (cursor > response.SearchResults.Count - 1 && cursor > 0)
                {
                    cursor = response.SearchResults.Count - 1;
                }


                for (int i = 0; i < response.SearchResults.Count; i++)
                {
                    // don't show more than the window height
                    if (Console.CursorTop >= Console.WindowHeight - 3)
                    {
                        break;
                    }

                    var selected = i == cursor;
                    Console.ResetColor();
                    if (selected)
                    {
                        Console.BackgroundColor = Theme.Selection;
                        Console.Write(" ");
                        Console.ResetColor();
                    }

                    Console.ForegroundColor = Theme.CommandText;
                    Console.Write(response.SearchResults[i].CommandText);
                    Console.ForegroundColor = Theme.Divider;

                    Console.Write(" - ");

                    Console.ForegroundColor =  Theme.Description;
                    Console.Write(response.SearchResults[i].Description);
                    Console.ForegroundColor = Theme.Divider;

                    Console.Write(" - ");

                    Console.ForegroundColor = Theme.Collection;
                    Console.Write(response.SearchResults[i].Collection);
                    Console.ForegroundColor = Theme.Divider;

                    Console.Write(" - ");

                    Console.ForegroundColor =  Theme.Tags;
                    Console.WriteLine($@"[{string.Join(",", response.SearchResults[i].Tags)}]");
                    Console.ForegroundColor = Theme.Divider;

                    Console.ResetColor();
                }


                Console.SetCursorPosition(0, Console.WindowHeight - 2);
                Console.ForegroundColor = Theme.Message;
                Console.Write(Resources.MakeSelectionMessage);
                Console.ResetColor();
                Console.SetCursorPosition(0, Console.WindowHeight - 1);
                Console.Write(searchString);
                Console.ForegroundColor = Theme.Success;
                Console.SetCursorPosition(searchString.Length, Console.WindowHeight - 1);
                Console.ResetColor();

                var input = Console.ReadKey();

                if (input.Key == ConsoleKey.Enter)
                {
                    if(response.SearchResults.Count > 0)
                        return await Selected(cursor, response);
                }
                else if (input.Key == ConsoleKey.UpArrow)
                {
                    if (cursor > 0)
                        cursor--;
                }
                else if (input.Key == ConsoleKey.DownArrow)
                {
                    if (cursor < response.SearchResults.Count - 1 && cursor < Console.WindowHeight - 2)
                        cursor++;
                }
                else if (input.Key == ConsoleKey.LeftArrow)
                {
                    if (page > 1)
                    {
                        page--;
                        return await MakeUserSelection(searchString, page, pageSize, options);
                    }
                        
                }
                else if (input.Key == ConsoleKey.RightArrow)
                {
                    if (page < response.TotalResults / response.SearchResults.Count)
                    {
                        page++;
                        return await MakeUserSelection(searchString, page, pageSize, options);
                    }
                }
                else if (input.Key == ConsoleKey.Backspace)
                {
                    if (searchString.Length > 0)
                    {
                        searchString = HandleBackSpace(searchString);
                        return await MakeUserSelection(searchString, page, pageSize, options);
                    }
                }
                else
                {
                    searchString += input.KeyChar;
                    return await MakeUserSelection(searchString, page, pageSize, options);
                }
            }
        }

        private void WriteHeaderRow()
        {
            Console.ResetColor();

            Console.ForegroundColor = Theme.CommandText;
            Console.Write("Command Text");
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.Write(" - ");

            Console.ForegroundColor = Theme.Description;
            Console.Write("Description");
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.Write(" - ");

            Console.ForegroundColor = Theme.Collection;
            Console.Write("Collection");
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.Write(" - ");

            Console.ForegroundColor = Theme.Tags;
            Console.WriteLine("Tags");
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.ResetColor();
        }


        /// <summary>
        /// Quickly clear the screen area
        /// </summary>
        private static void ClearScreenArea()
        {
            Console.SetCursorPosition(0, 0);

            Console.Write(string.Empty.PadLeft(Console.WindowHeight * Console.WindowWidth));

            Console.SetCursorPosition(0, 0);
        }


        /// <summary>
        /// Push everything in the current window up so that the application can use the entire screen area
        /// </summary>
        private static void ClearConsoleArea()
        {
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Pressing backspace will remove trailing whitespace OR all characters to the next whitespace OR all characters to the end of the string
        /// eg: 'text with whitespace   ' -> 'text with whitepsace'
        /// eg: 'text with' -> 'text'
        /// eg: 'text' -> ''
        /// </summary>
        private static string HandleBackSpace(string searchString)
        {
            var lastSpace = searchString.LastIndexOf(' ');
            if (lastSpace > 0)
            {
                return searchString.Substring(0, lastSpace);
            }
            return string.Empty;
        }

        private static async Task<CliResult> Selected(int cursor, Application.Features.Search.Response response)
        {
            var selected = response.SearchResults[cursor];
            await ClipboardService.SetTextAsync(selected.CommandText);
            return new CliResult($"Selection copied to clipboard.", ResultType.Success, response);
        }
    }
}
