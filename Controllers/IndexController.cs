using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FortalezaServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FortalezaServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public IndexController(fortalezaitdbContext context)
        {
            _context = context;
        }

        public class Status
        {
            public string Version { get; set; }
            public string ServerMode { get; set; }
            public string RequiredClient { get; set; }
            public string DbName { get; set; }
            public bool DbConnected { get; set; }
            public string ErrorMessage { get; set; }
        }

        private readonly string Version = "0.6.3";
        private readonly string RequiredClient = "0.6.3";
        private readonly string ServerMode = "development";
        private readonly string DbName = "fortalezaitdb";


        [HttpGet]
        public string Get()
        {
            return "Fortaleza Server v" + Version;
        }

        [HttpGet("status")]
        public async Task<ActionResult<Status>> GetStatus()
        {
            string errorMessage = "";

            bool dbConnected;

            try
            {
                dbConnected = await _context.Database.CanConnectAsync();
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                dbConnected = false;
            }

            return new Status
            {
                Version = Version,
                DbName = DbName,
                RequiredClient = RequiredClient,
                DbConnected = dbConnected,
                ServerMode = ServerMode,
                ErrorMessage = errorMessage
            };
        }
    }
}
