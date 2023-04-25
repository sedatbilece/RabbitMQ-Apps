using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMqApps.publisher
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
            //channel.QueueDeclare("hello-queue",true,false,false);
            //string queue = ""      -> kuyruk adı
            //bool durable = false   -> bellekten silinsin mi ?
            //bool exclusive = true  -> sadece bu kanaldan mı bağlanılsın ?
            //bool autoDelete = true -> subs bitince silinsin mi ?

            channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);


            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {

                string message = $"Message log   {x} ";
                //mesaj byte olarak gönderilmeli
                var messageBody = Encoding.UTF8.GetBytes(message);


                //mesaj gönderme işlemi
                channel.BasicPublish("logs-fanout","", null, messageBody);

                Console.WriteLine($"Mesaj gönderildi : {message}");


            });


               
            Console.ReadLine();
        }
    }
}
