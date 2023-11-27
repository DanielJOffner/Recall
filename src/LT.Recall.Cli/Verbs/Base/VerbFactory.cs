using LT.Recall.Cli.DI;
using LT.Recall.Cli.Options;

namespace LT.Recall.Cli.Verbs.Base
{
    internal static class VerbParser
    {
        /// <summary>
        /// Verb is the first argument that is not an option argument
        /// eg. -v -x -h search -> search
        /// eg. search -v -x -h -> search
        /// </summary>
        internal static Verb GetVerb(string[] args, DiContainer container)
        {
            var defaultVerb = container.Get<Search>();

            if (args.Length == 0) return defaultVerb;

            foreach (var arg in args)
            {
                if(OptionsFormatter.IsOptionArg(arg)) continue;

                if (Program.Verbs.TryGetValue(arg, out var verbType))
                {
                    var result = container.Get<Verb>(verbType);
                    return result;
                }
            }

            return defaultVerb;
        }

        /// <summary>
        /// Remove the verb from the args array
        /// </summary>
        public static string[] RemoveVerb(string[] args)
        {
            if (args.Length == 0) return args;

            var temp = args.ToList();

            for (int i = 0; i < args.Length; i++)
            {
                if (OptionsFormatter.IsOptionArg(args[i])) continue;

                temp.RemoveAt(i);
                break;
            }

            return temp.ToArray();
        }
    }
}
