﻿using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;

namespace RabbitMqApps.CreateExcelApp.Services
{
    public class RabbitMQPublisher
    {


        private readonly RabbitMQClientService _rabbitMQClientService;

        public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }


        public void Publish(CreateExcelMessage message)
        {
            var channel = _rabbitMQClientService.Connect();

            var bodyString = JsonSerializer.Serialize(message);
            var bodyByte = Encoding.UTF8.GetBytes(bodyString);

            var property = channel.CreateBasicProperties();

            property.Persistent = true;

            channel.BasicPublish(
                exchange: RabbitMQClientService.ExchangeName,
                routingKey: RabbitMQClientService.RoutingExcel,
                basicProperties: property,
                body: bodyByte
                );

        }
    }
}
