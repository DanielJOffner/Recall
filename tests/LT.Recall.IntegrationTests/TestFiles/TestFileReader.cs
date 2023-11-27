namespace LT.Recall.IntegrationTests.TestFiles
{
    public enum TestFileType
    {
        Import,
    }

    internal static class TestFileReader
    {
        public static string GetFilePath(TestFileType type, string fileName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", type.ToString(), fileName);
        }
    }
}
