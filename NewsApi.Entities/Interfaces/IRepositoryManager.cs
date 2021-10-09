using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsApi.Entities.Interfaces
{
    public interface IRepositoryManager
    {
        Task InsertArticle(Article article);
        Task<IEnumerable<Article>> GetRecent(int amount);
        Task<IEnumerable<Article>> GetArticleBySource(string source);
        Task<IEnumerable<Article>> GetArticleByAuthor(string author);
        Task<IEnumerable<Article>> GetLatestArticlesByContent(string search, int amount);
    }
}