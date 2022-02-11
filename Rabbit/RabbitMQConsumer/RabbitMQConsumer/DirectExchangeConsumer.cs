using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQConsumer
{
    public static class DirectExchangeConsumer
    {
        //Direct Exchange sẽ điều hướng Messages dựa vào routing keys.
        //Routing Key là một thuộc tính của Message được tạo ra bởi Producer.
        //Hình dung Routing Key như là một địa chỉ, và Exchange sẽ dựa vào địa chỉ đó để đưa Message vào đúng con đường (con đường ở đây chính là Binding Key).
        public static void Consume(IModel chanel)
        {
            chanel.ExchangeDeclare("demo-direct-exchange", ExchangeType.Direct);
            chanel.QueueDeclare("demo-direct-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            chanel.QueueBind("demo-direct-queue", "demo-direct-exchange", "account.int");
            chanel.BasicQos(0, 10, false);
            var consumer = new EventingBasicConsumer(chanel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };
            chanel.BasicConsume("demo-direct-queue", true, consumer);
        }
    }
}
