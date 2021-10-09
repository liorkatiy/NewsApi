using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsApi.Entities.Config;
using NewsApi.Entities.Interfaces;
using NewsApi.Logic.Entites;
using NewsApi.Logic.Logger;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NewsApi.Logic.Services
{
    public class NewsService : INewsService, IHostedService, IDisposable
    {

        private const string _url = "https://newsapi.org/";
        private const string _topHeadLinesUrl = _url + "v2/top-headlines";
        private const string _everythingUrl = _url + "v2/everything";
        private const string _sourcesUrl = _url + "v2/top-headlines/sources";

        private readonly NewsApiConfig _config;
        private readonly INewsServiceLogger _logger;
        private readonly INewsHttpClient _client;
        private readonly IMediator _mediator;
        private Timer _timer;
        private bool _isRunning;

        public NewsService(
            NewsApiConfig config,
            INewsHttpClient client,
            IMediator mediator,
            INewsServiceLogger logger)
        {
            _config = config;
            _logger = logger;
            _client = client;
            _mediator = mediator;
            _isRunning = false;
        }

        public async void PullNews(object state)
        {
            if (!_isRunning)
            {
                try
                {
                    _isRunning = true;
                    var queryParams = new Dictionary<string, string>();
                    queryParams.Add("apikey", _config.ApiKey);
                    queryParams.Add("country", _config.Country);
                    queryParams.Add("pageSize", "100");

                    var response = await _client.GetAsync<HeadlinesResponse>(_topHeadLinesUrl, queryParams);
                    if (response.Status == "ok")
                    {
                        foreach (var article in response.Articles)
                        {
                            await _mediator.Publish(article);
                        }
                    }
                    else
                    {
                        _logger.LogError(@$"new call error status: {response.Status} Code: {response.Code} Message: {response.Message}");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, @$"error while trying to get articles");
                }
                finally
                {
                    _isRunning = false;
                }
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(PullNews, null, TimeSpan.Zero,
        TimeSpan.FromSeconds(_config.ServiceInterval));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}