using System.Reflection;

namespace LT.Recall.Cli.Options
{
    internal static class OptionsParser
    {
        /// <summary>
        /// Remove any options arguments from the args array
        /// </summary>
        public static string[] RemoveOptions<TOptions>(string[] args, TOptions options) where TOptions : IOptions
        {
            List<string> filteredArgs = args.ToList();
            foreach (var property in options.GetType().GetProperties())
            {
                if (IsOption(filteredArgs.ToArray(), property.Name, out int index))
                {
                    filteredArgs.RemoveAt(index);
                }
            }
            return filteredArgs.ToArray();
        }

        /// <summary>
        /// Parse args to IOptions instance
        /// </summary>
        public static TOptions Parse<TOptions>(string[] args) where TOptions : IOptions, new()
        {
            var options = new TOptions();
            foreach (var property in options.GetType().GetProperties())
            {
                SetPropertyValue(property, options, args);
            }
            return options;
        }

        /// <summary>
        /// Assign arguments (if present) to the property
        /// </summary>
        private static void SetPropertyValue(PropertyInfo property, object instance, string[] args)
        {
            if (IsOption(args, property.Name, out int index))
            {
                switch (property.PropertyType)
                {
                    case var type when type == typeof(string):
                        if (args.Length > index + 1)
                        {
                            property.SetValue(instance, args[index + 1]);
                        }
                        else
                        {
                            property.SetValue(instance, string.Empty);
                        }
                        break;
                    case var type when type == typeof(bool):
                        property.SetValue(instance, true);
                        break;
                    default:
                        throw new InvalidOperationException($"Type {property.PropertyType} not supported");
                }
            }
        }

        /// <summary>
        /// Return true if the option is present in the args
        /// eg. --verbose or -v
        /// </summary>
        private static bool IsOption(string[] args, string property, out int index)
        {
            index = 0;

            index = Array.IndexOf(args, args.FirstOrDefault(x => x.StartsWith(OptionsFormatter.GetShortOption(property), StringComparison.InvariantCultureIgnoreCase)));
            if(index != -1) return true;

            index = Array.IndexOf(args, args.FirstOrDefault(x => x.StartsWith(OptionsFormatter.GetLongOption(property), StringComparison.InvariantCultureIgnoreCase)));
            if (index != -1) return true;

            return false;
        }
    }
}
