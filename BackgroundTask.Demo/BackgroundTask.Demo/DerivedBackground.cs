using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace BackgroundTask.Demo
{
    public class DerivedBackground : BackgroundService
    {
        public IWorker Worker { get; }

        public DerivedBackground(IWorker worker)
        {
            Worker = worker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Worker.DoWork(stoppingToken);
        }
    }
}
