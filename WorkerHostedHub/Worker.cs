using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerHostedHub
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger, 
            IHubContext<ClockHub,IClockClient> hubContext)
        {
            _logger = logger;
            HubContext = hubContext;
        }

        public IHubContext<ClockHub, IClockClient> HubContext { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await HubContext.Clients.All.OnTimeReceived(DateTime.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
