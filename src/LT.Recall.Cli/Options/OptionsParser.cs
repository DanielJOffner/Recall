using System.Reflection;

namespace LT.Recall.Cli.Options
{
    internal static class OptionsParser
    {
        /// <summary>
        /// Remove any options arguments from the args array
        /// </summary>
        public static string[] RemoveOptions(string[] args, Type verb)
        {
            List<string> filteredArgs = args.ToList();
            var instance = GetInstanceFromVerbType(verb);
            foreach (var property in instance!.GetType().GetProperties())
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
            var instance = Activator.CreateInstance(typeof(TOptions));
            foreach (var property in instance!.GetType().GetProperties())
            {
                SetPropertyValue(property, instance, args);
            }
            return (TOptions)instance;
        }

        /// <summary>
        /// Parse args to IOptions instance
        /// </summary>
        public static IOptions Parse(string[] args, Type verb)
        {
            var instance = GetInstanceFromVerbType(verb);
            foreach (var property in instance!.GetType().GetProperties())
            {
                SetPropertyValue(property, instance, args);
            }
            return (instance as IOptions)!;
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
        /// Get the options type for the verb
        /// eg. Search -> SearchOptions
        /// </summary>
        private static Type GetOptionsType(Type verbType)
        {
            return verbType.GetNestedType($"{verbType.Name}Options") ?? throw new Exception("Options type not found");
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

        private static object? GetInstanceFromVerbType(Type verb)
        {
            var optionsType = GetOptionsType(verb);
            var instance = Activator.CreateInstance(optionsType);
            return instance;
        }
    }
}
