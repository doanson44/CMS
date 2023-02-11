using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CMS.Core.Extensions
{
    public static class LoggerExtension
    {
        public static void Dump(this ILogger logger, string message)
        {
            Debug.WriteLine($"> {message}");
            logger.LogInformation($"> {message}");
        }
    }
}
