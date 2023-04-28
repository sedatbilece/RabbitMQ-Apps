
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqApps.WatermarkApp.Sevices;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMqApps.WatermarkApp.BackgroundServices
{
    public class WatermarkService : BackgroundService
    {

        private readonly RabbitMQClientService _rabbitMQClientService;
        private readonly ILogger<WatermarkService> _logger;
        private IModel _channel;

        public WatermarkService(RabbitMQClientService rabbitMQClientService, ILogger<WatermarkService> logger)
        {
            _rabbitMQClientService = rabbitMQClientService;
            _logger = logger;
            
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {

            _channel = _rabbitMQClientService.Connect();
            _channel.BasicQos(0, 1, false);

            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {


            var consumer = new AsyncEventingBasicConsumer(_channel);

            _channel.BasicConsume(RabbitMQClientService.QueueName,false, consumer);


            consumer.Received += Consumer_Received;


            return Task.CompletedTask;


        }

        private Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {

            try
            {

                var ImageCreatedEvent = JsonSerializer.Deserialize<ProductImageCreatedEvent>
                (Encoding.UTF8.GetString(@event.Body.ToArray()));

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/iamges", ImageCreatedEvent.ImageName);

                var watermark = "www.github.com/sedatbilece";

                using var img = Image.FromFile(path);

                using var grapic = Graphics.FromImage(img);

                var font = new Font(FontFamily.GenericSerif, 32, FontStyle.Bold, GraphicsUnit.Pixel);


                var textSize = grapic.MeasureString(watermark, font);

                var color = Color.FromArgb(128, 255, 255, 255);
                var brush = new SolidBrush(color);

                var position = new Point(img.Width - ((int)textSize.Width + 30), img.Height - ((int)textSize.Height + 30));


                grapic.DrawString(watermark, font, brush, position);

                img.Save("wwwroot/images/watermarks" + ImageCreatedEvent.ImageName);

                img.Dispose();
                grapic.Dispose();

                _channel.BasicAck(@event.DeliveryTag, false);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
