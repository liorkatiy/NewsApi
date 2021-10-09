using NewsApi.Entities.Interfaces;
using System.Threading.Tasks;

namespace NewsApi.Logic.Managers
{
    public class ArticleManager
    {
        private readonly IRepositoryManager _repositoryManager;

        public ArticleManager(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        async Task GetArticles()
        {

        }
    }
}