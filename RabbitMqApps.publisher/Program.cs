using RabbitMQ.Client;
using System;
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
            channel.QueueDeclare("hello-queue",true,false,false);
            //string queue = ""      -> kuyruk adı
            //bool durable = false   -> bellekten silinsin mi ?
            //bool exclusive = true  -> sadece bu kanaldan mı bağlanılsın ?
            //bool autoDelete = true -> subs bitince silinsin mi ?


            string message = "hello world ! :D";
            //mesaj byte olarak gönderilmeli
            var messageBody = Encoding.UTF8.GetBytes(message);


            //mesaj gönderme işlemi
            channel.BasicPublish(string.Empty,"hello-queue",null,messageBody);

            Console.WriteLine(message);     
            Console.ReadLine();
        }
    }
}
