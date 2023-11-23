using LT.Recall.Application.Errors;
using LT.Recall.Domain.Errors;

namespace LT.Recall.Application.Abstractions
{
    public enum LogLevel { Debug = 0, Info = 1, Warn = 2, Error = 3 }
    public interface IRecallLogger
    {
        public void Debug(string message, params object[] args);
        public void Info(string message, params object[] args);
        public void Warn(string message, params object[] args);
        public void Warn(BaseError error, string message, params object[] args);
        public void Error(string message, params object[] args);
        public void Error(BaseError error, string message, params object[] args);
    }
}
