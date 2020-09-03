using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FortalezaServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Fortaleza Server v0.2.0";
        }
    }
}
