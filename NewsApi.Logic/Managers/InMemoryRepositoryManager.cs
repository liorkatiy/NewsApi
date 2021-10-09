using NewsApi.Entities;
using NewsApi.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsApi.Logic.Managers
{
    public class InMemoryRepositoryManager : IRepositoryManager
    {
        private readonly object _lock;
        private readonly IEnumerable<Article> Empty;
        private readonly int _maxAmount;
        private readonly HashSet<Article> _articles;

        public InMemoryRepositoryManager()
        {
            _lock = new object();
            _articles = new HashSet<Article>();
            Empty = new Article[0];
            _maxAmount = 100;
        }

        void ActOnArticles(Action<HashSet<Article>> action)
        {
            lock (_lock)
            {
                action(_articles);
            }
        }

        List<Article> QueryArticles(Func<HashSet<Article>, IEnumerable<Article>> func)
        {
            lock (_lock)
            {
                return func(_articles).ToList();
            }
        }

        public Task InsertArticle(Article article)
        {
            ActOnArticles((a) => a.Add(article));
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Article>> GetRecent(int amount)
        {
            amount = Math.Min(amount, _maxAmount);
            var result = QueryArticles(articles => articles.OrderByDescending(a => a.PublishedAt).Take(amount));
            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<Article>> GetArticleBySource(string source)
        {
            if (source == null) return Task.FromResult(Empty);

            source = source.ToLower();
            var result = QueryArticles(articles => articles.Where(a => (a.Source?.Name ?? string.Empty).ToLower().Contains(source)));
            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<Article>> GetArticleByAuthor(string author)
        {
            if (author == null) return Task.FromResult(Empty);

            author = author.ToLower();
            var result = QueryArticles(articles => articles.Where(a => a.Author != null && a.Author.ToLower().Contains(author)));
            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<Article>> GetLatestArticlesByContent(string search, int amount)
        {
            amount = Math.Min(amount, _maxAmount);
            search = search.ToLower();
            var result = QueryArticles(
                articles =>
                    articles
                    .Where(a =>
                     (a.Content ?? a.Description ?? a.Title).ToLower().Contains(search))
                    .OrderByDescending(a => a.PublishedAt)
                    .Take(amount));

            return Task.FromResult(result.AsEnumerable());
        }
    }
}