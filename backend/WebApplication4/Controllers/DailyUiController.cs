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
    public class DailyUiController : ControllerBase
    {
        public static DailyUiService _dailyuiService;
        public DailyUiController(DailyUiService dailyuiService)
        {
            _dailyuiService = dailyuiService;
        }

        [HttpGet]

        public IActionResult getGridData()
        {

            return Ok(_dailyuiService.GetGridData());
        }
    }
}
