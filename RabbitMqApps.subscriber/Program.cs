using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

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

            channel.BasicQos(0, 1, true);

            var consumer =  new EventingBasicConsumer(channel);

            channel.BasicConsume("hello-queue", false, consumer);//2. parametre oto silme isteği 


            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {

                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(1000);
                Console.WriteLine("Gelen Mesaj : "+message);

                channel.BasicAck(e.DeliveryTag,false);// rabbitmq'ya işlemin tamamlandığını bildirir artık silebilir
            };
           

            Console.ReadLine();
        }

        
    }
}
