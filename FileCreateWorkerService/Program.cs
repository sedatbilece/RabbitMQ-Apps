using FileCreateWorkerService.Models;
using FileCreateWorkerService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileCreateWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                    IConfiguration configuration = hostContext.Configuration;
                    services.AddHostedService<Worker>();

                    services.AddDbContext<AdventureWorks2012Context>(options =>
                    {
                        options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
                    });


                    services.AddSingleton<RabbitMQClientService>();

                    IConfiguration Configuration = hostContext.Configuration;

                    services.AddSingleton(sp => new ConnectionFactory()
                    { Uri = new Uri(Configuration.GetConnectionString("RabbitMQ")), DispatchConsumersAsync = true });

                   
                });
    }
}
