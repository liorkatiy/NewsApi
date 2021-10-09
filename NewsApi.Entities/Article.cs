using MediatR;
using System;

namespace NewsApi.Entities
{
    public class Article : INotification
    {
        public string Author { get; set; }
        public Source Source { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Content { get; set; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Article)
            {
                return ((Article)obj).Url == Url;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Url.GetHashCode();
        }
    }
}