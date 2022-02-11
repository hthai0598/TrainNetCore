using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQConsumer
{
    public static class QueueConsumer
    {
        //Default Exchange là một Direct Exchange nhưng không có tên.
        //Khi sử dụng default exchange thì message được đưa tên queue nào có tên trùng với routing key của message.
        public static void Consume(IModel chanel)
        {
            chanel.QueueDeclare("demo-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(chanel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };
            chanel.BasicConsume("demo-queue", true, consumer);
        }
    }
}
