using Microsoft.AspNetCore.Mvc;
using System.IO;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaderController : ControllerBase
    {
       public static LoaderService _loaderService;

        public LoaderController(LoaderService loaderService)
        {
            _loaderService = loaderService;
        }

        [HttpPost]
        public IActionResult Loading()
        {
            _loaderService.LoadData();
            return Ok();
        }

        //public static void OnCreated(object sender, FileSystemEventArgs e)
        //{
        //    _loaderService.LoadData();

        //}
    }

}
