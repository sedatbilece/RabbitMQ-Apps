using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMqApps.subscriber
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://iflqkrhk:ykIuc0d4b8WuE3RX_gUx575_vULzNsUy@hawk.rmq.cloudamqp.com/iflqkrhk");

            using var connection = factory.CreateConnection();

            // bağlantı kurulacak bağlantı
            var channel = connection.CreateModel();

            // queue oluşturma 
            channel.QueueDeclare("hello-queue", true, false, false);
          

            var consumer =  new EventingBasicConsumer(channel);

            channel.BasicConsume("hello-queue", true, consumer);


            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {

                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Console.WriteLine("Gelen Mesaj : "+message);
            };
           

            Console.ReadLine();
        }

        
    }
}
