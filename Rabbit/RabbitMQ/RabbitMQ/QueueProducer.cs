using System;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQ
{
    public class QueueProducer
    {
        public static void Publish(IModel chanel)
        {
            chanel.QueueDeclare("demo-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var message = new { Name = "Producer", Message = "Hello sađasadá" };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            chanel.BasicPublish("", "demo-queue", null, body);
        }
    }
}
