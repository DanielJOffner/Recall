using LT.Recall.Cli.Properties;
using LT.Recall.Cli.Verbs;
using System.Text;

namespace LT.Recall.Cli.Options
{
    internal static class HelpTextGenerator
    {
        /// <summary>
        /// Generate localized help text containing version, commands and options.
        /// </summary>
        public static string GenerateHelpText<T>() where T : IOptions
        {
            return GenerateHelpTextFromType(typeof(T));
        }

        /// <summary>
        /// Generate localized help text containing version, commands and options.
        /// </summary>
        public static string GenerateHelpText(Type optionsType)
        {
            return GenerateHelpTextFromType(optionsType);
        }

        private static string GenerateHelpTextFromType(Type optionsType)
        {
            var sb = new StringBuilder();
            
            GenerateVersionInformation(optionsType, sb);
            GenerateOptionsHelp(optionsType, sb);
            GenerateVerbHelp(optionsType, sb);
            GenerateExamples(optionsType, sb);

            return sb.ToString();
        }


        private static void GenerateExamples(Type optionsType, StringBuilder sb)
        {
            if (!IsMainOptions(optionsType))
            {
                sb.AppendLine(string.Empty);
                sb.AppendLine(GetVerbExampleText(GetVerb(optionsType)));
            }
        }

        private static void GenerateVerbHelp(Type optionsType, StringBuilder sb)
        {
            if (IsMainOptions(optionsType))
            {
                sb.AppendLine(string.Empty);
                foreach (var verb in Program.Verbs.Where(x => x.Key != "help"))
                {
                    sb.AppendLine($"{verb.Key}\t{GetVerbHelpText(verb.Key)}");
                }
            }
        }

        private static void GenerateOptionsHelp(Type optionsType, StringBuilder sb)
        {
            var mainOptions = typeof(Program.Options).GetProperties().Select(x => x.Name).ToList();
            foreach (var option in optionsType.GetProperties())
            {
                if (mainOptions.Contains(option.Name) && !IsMainOptions(optionsType))
                    continue;

                sb.AppendLine(
                    $"{OptionsFormatter.GetShortOption(option.Name)}, {OptionsFormatter.GetLongOption(option.Name)}\t{GetOptionHelpText(option.Name)}");
            }
        }

        private static void GenerateVersionInformation(Type optionsType, StringBuilder sb)
        {
            if (IsMainOptions(optionsType))
            {
                sb.AppendLine($"v{typeof(Program).Assembly.GetName().Version}");
            }
            sb.AppendLine(string.Empty);
        }


        private static bool IsMainOptions(Type optionsType)
        {
            return optionsType == typeof(Program.Options) || optionsType == typeof(Help.HelpOptions);
        }

        /// <summary>
        /// Resource file must contain a string matching {property}OptionHelpText
        /// eg. VerboseOptionHelpText, DebugOptionHelpText etc
        /// </summary>
        private static string GetOptionHelpText(string property)
        {
            var localizedHelpText  = Resources.ResourceManager.GetString($"{property}OptionHelpText");
            if (!string.IsNullOrWhiteSpace(localizedHelpText))
            {
                return localizedHelpText;
            }
            return Resources.HelpTextNotFoundError;
        }

        /// <summary>
        /// Resource file must contain a string matching {verb}VerbHelpText
        /// eg. SaveVerbHelpText, SearchVerbHelpText etc
        /// </summary>
        private static string GetVerbHelpText(string verb)
        {
            var pascalCase = ToPascalCase(verb);
            var localizedHelpText = Resources.ResourceManager.GetString($"{pascalCase}VerbHelpText");
            if (!string.IsNullOrWhiteSpace(localizedHelpText))
            {
                return localizedHelpText;
            }
            return Resources.HelpTextNotFoundError;
        }

        /// <summary>
        /// Resource file must contain a string matching {verb}ExampleText
        /// eg. SaveVerbExampleText, SearchVerbExampleText etc
        /// </summary>
        /// <param name="verb"></param>
        private static string GetVerbExampleText(string verb)
        {
            var pascalCase = ToPascalCase(verb);
            var localizedHelpText = Resources.ResourceManager.GetString($"{pascalCase}VerbExampleText");
            if (!string.IsNullOrWhiteSpace(localizedHelpText))
            {
                return localizedHelpText;
            }
            return Resources.HelpTextNotFoundError;
        }

        private static string GetVerb(Type optionsType)
        {
            return optionsType.Name.Replace("Options", string.Empty);
        }

        private static string ToPascalCase(string verb)
        {
            var pascalCase = char.ToUpperInvariant(verb[0]) + verb.Substring(1);
            return pascalCase;
        }
    }
}
