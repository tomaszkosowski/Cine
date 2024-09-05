using Microsoft.Extensions.Logging;

namespace Cine.Shared.Application.Logger
{
    public static class LoggerExtensions
    {
        public static void LogApplicationError(this ILogger logger, Exception exception)
            => logger.LogError(exception, "Application error.");
    }
}
