using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundTask.Demo
{
    public class BackgroundPrinter : IHostedService
    {
        private int number = 0;
        ILogger<BackgroundPrinter> logger;
        IWorker worker;
        private Timer timer;
        public BackgroundPrinter(ILogger<BackgroundPrinter> logger, IWorker worker)
        {
            this.logger = logger;
            this.worker = worker;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await worker.DoWork(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Printf worker stopp");
            return Task.CompletedTask;
        }
    }
}
