using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParserController : ControllerBase
    {
        public static ParserService _parserService;
        public ParserController( ParserService parserService)
        {
            _parserService = parserService;
        }

        [HttpGet]
       public IActionResult Import()
        {
            _parserService.ReadData();
            return Ok();
        }


        //public static void OnChanged(object sender, FileSystemEventArgs e)
        //{
        //    if (e.ChangeType != WatcherChangeTypes.Changed)
        //    {
        //        return;
        //    }
        //    _parserService.ReadData();
            
        //}
    }
}
