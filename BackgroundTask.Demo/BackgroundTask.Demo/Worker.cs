using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BackgroundTask.Demo
{
    public class Worker : IWorker
    {
        ILogger<Worker> logger;
        private int number = 0;
        public Worker(ILogger<Worker> logger)
        {
            this.logger = logger;
        }
        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //Interlocked.Increment(ref number);
                //logger.LogInformation($"{number}");
                Console.WriteLine("10000");
                await Task.Delay(5000);
            }
        }
    }
}
