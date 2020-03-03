using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerAsHubClient
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HubConnection _hubConnection;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken token)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/hubs/clock")
                .Build();

            _hubConnection.On<DateTime>("OnTimeReceived", (serverTime) => 
            {
                _logger.LogWarning($"Time on server is {serverTime}");
            });

            await _hubConnection.StartAsync();
                
            await base.StartAsync(token);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Client running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
