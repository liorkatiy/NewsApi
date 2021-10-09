using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NewsApi.Entities;
using NewsApi.Entities.Config;
using NewsApi.Entities.Interfaces;
using NewsApi.Logic.Entites;
using NewsApi.Logic.Logger;
using NewsApi.Logic.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace News.Test
{
    public class NewsServiceTests
    {
        private NewsApiConfig _config;
        private Mock<INewsHttpClient> _newsHttpClientMock;
        private Mock<IMediator> _mediatorMock;
        private Mock<INewsServiceLogger> _logerMock;

        [SetUp]
        public void Setup()
        {
            _config = new NewsApiConfig();
            _newsHttpClientMock = new Mock<INewsHttpClient>();
            _mediatorMock = new Mock<IMediator>();
            _logerMock = new Mock<INewsServiceLogger>();
        }

        [TestCase(TestName = "News Response Success")]
        public Task NewResponseSuccess()
        {
            var headlinesResponse = new HeadlinesResponse()
            {
                Status = "ok",
                Articles = new List<Article>
                {
                    new Article()
                    {
                        Title = "a",
                        Author = "a"
                    },
                            new Article()
                    {
                        Title = "b",
                        Author = "b"
                    },
                }
            };

            _newsHttpClientMock
                .Setup(s => s.GetAsync<HeadlinesResponse>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.FromResult(headlinesResponse));

            var newsService = new NewsService(_config, _newsHttpClientMock.Object, _mediatorMock.Object, _logerMock.Object);

            newsService.PullNews(null);

            _mediatorMock.Verify(m => m.Publish(It.IsAny<Article>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            return Task.CompletedTask;
        }

        [TestCase(TestName = "News Response Fail")]
        public Task NewResponseFail()
        {
            var headlinesResponse = new HeadlinesResponse()
            {
                Status = "fail",
                Code = "a",
                Message = "a"
            };

            _newsHttpClientMock
                .Setup(s => s.GetAsync<HeadlinesResponse>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.FromResult(headlinesResponse));

            var newsService = new NewsService(_config, _newsHttpClientMock.Object, _mediatorMock.Object, _logerMock.Object);

            newsService.PullNews(null);

            _logerMock.Verify(l => l.LogError(@$"new call error status: {headlinesResponse.Status} Code: {headlinesResponse.Code} Message: {headlinesResponse.Message}"));
            return Task.CompletedTask;
        }

        [TestCase(TestName = "News Response Crash")]
        public Task NewResponseCrash()
        {
            var headlinesResponse = new HeadlinesResponse()
            {
                Status = "fail",
                Code = "a",
                Message = "a"
            };

            _newsHttpClientMock
                .Setup(s => s.GetAsync<HeadlinesResponse>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Throws(new HttpRequestException());

            var newsService = new NewsService(_config, _newsHttpClientMock.Object, _mediatorMock.Object, _logerMock.Object);

            newsService.PullNews(null);

            _logerMock.Verify(l => l.LogError(new HttpRequestException(), @$"error while trying to get articles"));
            return Task.CompletedTask;
        }
    }
}