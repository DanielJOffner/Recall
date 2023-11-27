namespace LT.Recall.Cli.Themes
{
    public class ThemeStore
    {
        private static readonly string ThemeEnvVar = "RECALL_THEME";
        public static void SetTheme(ITheme theme)
        {
            Environment.SetEnvironmentVariable(ThemeEnvVar, theme.GetType().Name);
        }

        public static ITheme GetTheme()
        {
            var themeName = Environment.GetEnvironmentVariable(ThemeEnvVar);
            if (themeName == null)
            {
                return new DefaultTheme();
            }

            var themeType = Type.GetType($"{typeof(DefaultTheme).Namespace}.{themeName}");
            if (themeType == null)
            {
                return new DefaultTheme();
            }

            return (ITheme)Activator.CreateInstance(themeType)!;
        }
    }
}
