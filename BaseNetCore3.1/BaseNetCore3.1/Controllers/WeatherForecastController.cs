using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BaseNetCore3._1.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BaseNetCore3._1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {

            return new ResponseResult(HttpStatusCode.OK, new List<string>() { "thai", "meo" });

        }
    }
}
