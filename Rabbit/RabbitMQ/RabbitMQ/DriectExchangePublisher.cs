using System;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQ
{
    public class DriectExchangePublisher
    {
        public static void Publish(IModel chanel)
        {
            chanel.ExchangeDeclare("demo-direct-exchange", ExchangeType.Direct);
            var count = 0;
            while (true)
            {
                var message = new { Name = "Producer", Message = "Hello" };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                chanel.BasicPublish("demo-direct-exchange", "account.int", null, body);
                count++;
                Thread.Sleep(1000);
            }
        }
    }
}
