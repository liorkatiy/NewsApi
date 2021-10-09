using NewsApi.Entities;
using System.Collections.Generic;

namespace NewsApi.Logic.Entites
{
    public class HeadlinesResponse
    {
        public string Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public int TotalResults { get; set; }
        public IEnumerable<Article> Articles { get; set; }
    }
}
