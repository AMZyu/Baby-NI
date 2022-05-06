using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregatorController : ControllerBase
    {
        private AggregatorService _aggregatorService;

        public AggregatorController(AggregatorService aggregatorService)
        {
            _aggregatorService = aggregatorService;
        }

        [HttpPost]
        public IActionResult Aggregating()
        {
            _aggregatorService.AggregateData();
            return Ok();
        }
    }
}
