using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMqApps.WatermarkApp.BackgroundServices
{
    public class test : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
