using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HangFireDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet("get")]
        public IEnumerable<string> Get()
        {
            var a = "123";
            if (a is not null)
            {

            }
            return new string[] { "value1", "value2" };
        }

        // POST api/<ValuesController>
        //Fire-and-forgot job: thực thi 1 lần và gần như ngay lập tức sau khi khởi tạo.
        [HttpPost("post")]
        public IActionResult Welcome()
        {
            var jobid = BackgroundJob.Enqueue(() => SendWelcomeEmail("Welcome"));
            return Ok($"Welcome Email, jobId {jobid}");
        }

        [HttpGet("delay")]
        //Delayed job: Thực thi 1 lần sau 1 khoảng thời gian quy định
        public IActionResult Delay()
        {
            int time = 30;
            var jobid = BackgroundJob.Schedule(() => SendWelcomeEmail("Welcome"), TimeSpan.FromSeconds(time));
            return Ok($"doing after {time}");
        }


        [HttpGet("databaseupdate")]
        //Recurring job: Lặp lại công việc nhiều lần dựa trên CRON schedule.
        public IActionResult DatabaseUpdate()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Database updated"), Cron.Minutely);
            return Ok("updated");
        }

        [HttpPost("confirm")]
        //Continuations: Tiếp tục thực thi công việc sau khi công việc trước đó thực hiện xong.
        public IActionResult Confirm()
        {
            int time = 30;
            var jobid = BackgroundJob.Schedule(() => SendWelcomeEmail("Welcome"), TimeSpan.FromSeconds(time));

            BackgroundJob.ContinueJobWith(jobid, () => Console.WriteLine("Done"));
            return Ok("Confirm ");

        }

        [HttpGet("html")]
        public IHtmlContent html()
        {
            return new HtmlString("<a href='google.com'></a>");

        }

        public void SendWelcomeEmail(string text)
        {
            Console.WriteLine(text);
        }
    }
}
