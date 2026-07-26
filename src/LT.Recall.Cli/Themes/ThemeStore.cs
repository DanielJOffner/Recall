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

            // Todo - add some more themes
            if(Enum.TryParse(themeName, out Theme theme))
            {
                switch (theme)
                {
                    case Theme.Default:
                        return new DefaultTheme();
                    default:
                        return new DefaultTheme();
                }   
            }

           return new DefaultTheme();
        }
    }
}
