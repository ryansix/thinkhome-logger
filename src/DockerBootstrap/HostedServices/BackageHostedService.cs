using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DockerBootstrap.HostedServices
{
    public class BackageHostedService : IHostedService
    {
        private readonly ILogger<BackageHostedService> Logger;
        private BackageHostedService(ILogger<BackageHostedService> logger)
        {
            Logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation(" BackageHostedService started .....");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation(" BackageHostedService end .....");
            return Task.CompletedTask;
        }
    }
}
