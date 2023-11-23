using LT.Recall.Cli.Options;
using LT.Recall.Cli.Output;
using LT.Recall.Cli.Properties;
using LT.Recall.Cli.Verbs;
using System.Diagnostics;
using LT.Recall.Application.Abstractions;
using LT.Recall.Cli.DI;
using LT.Recall.Cli.Themes;
using LT.Recall.Cli.Verbs.Base;

// ReSharper disable MemberCanBePrivate.Global
#pragma warning disable IDE1007

namespace LT.Recall.Cli
{
    public class Program
    {
        public static readonly Dictionary<string, Type> Verbs = new(StringComparer.InvariantCultureIgnoreCase)
        {
            { "help" , typeof(Help) },
            { "save", typeof(Save) },
            { "search", typeof(Search) },
            { "delete", typeof(Delete) },
            { "import", typeof(Import) },
            { "export", typeof(Export) },
            { "stats", typeof(Stats) }
        };

        public class Options : IOptions
        {
            public bool XTest { get; set; }
            public bool Verbose { get; set; }
            public bool Help { get; set; }
        }

        public static async Task Main(string[] args)
        {
            Console.CancelKeyPress += delegate { Console.Clear(); Console.ResetColor();};

            var options = OptionsParser.Parse<Options>(args);

            if (options.XTest)
            {
                WaitToReadLine();
            }

            var diContainer = new DiContainer();
            var theme = ThemeStore.GetTheme();
            var verb = VerbParser.GetVerb(args, diContainer);
            var result = await verb.ExecuteAsync(theme, args, options);
            var serializer = diContainer.Get<IJsonSerializer>();

            OutputFormatter.Write(theme, result, options.XTest, serializer);
        }

        /// <summary>
        /// Read key to allow for a debugger to attach.
        /// </summary>
        private static void WaitToReadLine()
        {
            Console.WriteLine(Resources.DebugMessage, Process.GetCurrentProcess().Id);
            Console.ReadLine();
        }
    }
}