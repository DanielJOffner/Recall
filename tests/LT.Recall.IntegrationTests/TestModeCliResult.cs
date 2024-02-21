using LT.Recall.Cli.Output;

namespace LT.Recall.IntegrationTests
{
    internal class TestModeCliResult<T> where T : class, new()
    {
        public string Message { get; set; } = string.Empty;
        public ResultType ResultType { get; set; } = ResultType.Message;
        public T Data { get; set; } = new T();
    }
}
