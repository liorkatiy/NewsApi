using Microsoft.Extensions.Logging;
using NewsApi.Logic.Services;
using System;

namespace NewsApi.Logic.Logger
{
    public class NewsServiceLogger : INewsServiceLogger
    {
        private readonly ILogger<NewsService> _logger;

        public NewsServiceLogger(ILogger<NewsService> logger)
        {
            _logger = logger;
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
        }

        public void LogError(Exception e, string message)
        {
            _logger.LogError(e, message);
        }
    }
}