using System;
using System.Collections.Generic;
using System.Text;

namespace NewsApi.Entities.Config
{
   public class NewsApiConfig
    {
        public string ApiKey { get; set; }
        public string Country { get; set; }
        public int ServiceInterval { get; set; }
    }
}
