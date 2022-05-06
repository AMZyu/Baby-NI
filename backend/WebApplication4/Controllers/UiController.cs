using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UiController : ControllerBase
    {
        public static UiService _uiService;
        public UiController(UiService uiService)
        {
            _uiService = uiService;
        }

        [HttpGet]

        public IActionResult getGridData()
        {
          
            return Ok(_uiService.GetGridData());
        }

        
    }
}
