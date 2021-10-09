using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsApi.Controllers
{
    public class HealthCheckController : ControllerBase
    {
        [Route("/health")]
        [HttpGet]
        public string Get()
        {
            return "Ok";
        }
    }
}