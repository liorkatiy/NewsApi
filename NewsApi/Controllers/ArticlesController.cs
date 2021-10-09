﻿using Microsoft.AspNetCore.Mvc;
using NewsApi.Entities;
using NewsApi.Entities.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsApi.Controllers
{
    public class ArticlesController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;

        public ArticlesController(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        [Route("/articles/author")]
        [HttpGet]
        public async Task<IEnumerable<Article>> GetArticleByAuthor([FromQuery] string author)
        {
            return await _repositoryManager.GetArticleByAuthor(author);
        }

        [Route("/articles/source")]
        [HttpGet]
        public async Task<IEnumerable<Article>> GetArticleBySource([FromQuery] string author)
        {
            return await _repositoryManager.GetArticleBySource(author);
        }

        [Route("/articles")]
        [HttpGet]
        public async Task<IEnumerable<Article>> GetRecent([FromQuery] int amount = 10)
        {
            return await _repositoryManager.GetRecent(amount);
        }

        [Route("/articles/search")]
        [HttpGet]
        public async Task<IEnumerable<Article>> GetLatestArticlesByContent([FromQuery] string search, [FromQuery] int amount = 10)
        {
            return await _repositoryManager.GetLatestArticlesByContent(search, amount);
        }
    }
}