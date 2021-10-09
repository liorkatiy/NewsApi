using MediatR;
using NewsApi.Entities;
using NewsApi.Entities.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace NewsApi.Logic.NotificationsHandlers
{
    public class ArticleHandler : INotificationHandler<Article>
    {
        private readonly IRepositoryManager _repositoryManager;

        public ArticleHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task Handle(Article notification, CancellationToken cancellationToken)
        {
            await _repositoryManager.InsertArticle(notification);
        }
    }
}