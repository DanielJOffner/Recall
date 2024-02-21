namespace LT.Recall.Infrastructure.IO
{
    /// <summary>
    /// Must be in a using block
    /// </summary>
    public class TempFileWriter : IDisposable   
    {
        private List<string> _tempFiles = new List<string>();
        private string TempFileDirectory => Path.Combine(Path.GetTempPath(), "RecallCliTemp");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileExtension">The extension to use for the temp file</param>
        /// <returns>The absolute path to the temp file</returns>
        public string Write(string contents, string fileExtension)
        {
            if (!Directory.Exists(TempFileDirectory))
            {
                Directory.CreateDirectory(TempFileDirectory);
            }
            
            var tempFilePath = Path.Combine(TempFileDirectory, $"{Guid.NewGuid()}{fileExtension}");
            _tempFiles.Add(tempFilePath);
            File.WriteAllText(tempFilePath, contents);
            return tempFilePath;
        }

        public void Dispose()
        {
            foreach (var file in _tempFiles)
            {
                // TODO - maybe add backoff retry logic here in case the file is locked by another process
                File.Delete(file);
            }
        }
    }
}
