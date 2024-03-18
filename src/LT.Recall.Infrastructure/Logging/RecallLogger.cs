using LT.Recall.Application.Abstractions;
using LT.Recall.Domain.Errors;
using Serilog;

namespace LT.Recall.Infrastructure.Logging
{
    public class RecallLogger : IRecallLogger
    {
        private readonly ILogger _logger;
        private LogLevel _logLevel;
        public RecallLogger(LogLevel logLevel)
        {
            _logLevel = logLevel;
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public void SetLogLevel(LogLevel logLevel)
        {
           _logLevel = logLevel;
        }

        public void Debug(string message, params object[] args)
        {
            if(_logLevel <= LogLevel.Debug)
                _logger.Debug(message, args);
        }

        public void Info(string message, params object[] args)
        {
            if(_logLevel <= LogLevel.Info)
                _logger.Information(message, args);
        }

        public void Warn(string message, params object[] args)
        {
            if(_logLevel <= LogLevel.Warn)
                _logger.Warning(message, args);
        }

        public void Warn(BaseError error, string message, params object[] args)
        {
            if(_logLevel <= LogLevel.Warn)
                _logger.Warning(error, message, args);
        }

        public void Error(string message, params object[] args)
        {
            _logger.Error(message, args);
        }

        public void Error(BaseError error, string message, params object[] args)
        {
            _logger.Error(error, message, args);
        }
    }
}
