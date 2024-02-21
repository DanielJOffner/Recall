namespace LT.Recall.Cli.Output
{
    public class CliResult
    {
        public string Message { get; }
        public ResultType ResultType { get; }
        public object? Data { get; }
        public CliResult(string message, ResultType resultType = ResultType.Message, object? data = null)
        {
            Message = message;
            ResultType = resultType;
            Data = data;
        }
    }
}
