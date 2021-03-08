using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedisDemo.Attributes;
using RedisDemo.Service;

namespace RedisDemo.Controllers
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
        private readonly IResponseCacheService _responseCacheService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IResponseCacheService responseCacheService)
        {
            _logger = logger;
            _responseCacheService = responseCacheService;
        }

        [Route("getall")]
        [HttpGet]
        [Cache(100)]
        public async Task<IActionResult> Get(string keyword = null, int pageIndex = 1, int pageSize = 20)
        {
            var result = new List<WeatherForecast>()
            {
                new WeatherForecast(){Name = "thais"}
            };
            return Ok(result);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            await _responseCacheService.RemoveCachResponseAsync("/weatherforecast/getall");
            return Ok();
        }
    }
}
