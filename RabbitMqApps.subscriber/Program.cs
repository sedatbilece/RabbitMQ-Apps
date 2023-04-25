using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMqApps.subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://iflqkrhk:ykIuc0d4b8WuE3RX_gUx575_vULzNsUy@hawk.rmq.cloudamqp.com/iflqkrhk");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();


            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);

            var queueName = "direct-queue-Critical";
            

            Console.WriteLine("Logları dinleniyor...");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(1500);
                Console.WriteLine("Gelen Mesaj:" + message);

                // File.AppendAllText("log-critical.txt", message+ "\n");

                channel.BasicAck(e.DeliveryTag, false);
            };

            channel.BasicConsume(queueName, false, consumer);

            Console.ReadLine();
        }


    }
}
