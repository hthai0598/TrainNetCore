using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQConsumer
{
    public static class TopicExchangeConsumer
    {
        //Topic Exchange điều hướng Messages dựa trên wildcard matchs (ko biết dịch ra sao) giữa routing keys và binding keys.
        //Messages sẽ được gửi đến một hoặc nhiều queues dựa trên việc so khớp giữa routing keys và pattern.
        //Wildcard Match: chỉ việc so khớp dựa trên pattern chứa các kí tự như là dấu sao(*), hoặc dấu chấm(.)

        //Ví dụ:
        // abc*: chuỗi bắt đầu bằng “abc”, nối tiếp sẽ là 1 hoặc nhiều kí tự
        //abc.: bắt đầu bằng “abc”, nối tiếp là 1 ký tự bất kì
        //Tương tự ta có thể có: *abc, abc* def., ….

        //Routing Key sẽ là những từ được phân cách bằng dấu chấm(.). Ví dụ ta có users.us và users.vn.hcm,
        //trường hợp này ta hiểu rằng có nhiều users ở các địa điểm khác nhau.Routing Patterns có thể chứa dấu sao(*)
        //để match với 1 từ ở 1 vị trí cụ thể trong routing key
        //Ví dụ: Routing Pattern là users.*.*.b.* sẽ khớp với các Routing Keys bắt đầu bằng từ “users” và từ thứ 4 là “b”.
        //Ký tự dấu thăng (#) chỉ việc khớp với 0 hoặc nhiều từ (Lưu ý là nhiều từ nha). Ví dụ với pattern sau “users.vn.hcm.#”
        //sẽ khớp với “users.vn.hcm” và “users.vn.hcm.tanphu” và “users.vn.hcm.tanphu.abc.xyz”
        //Consumer tạo ra queue và setup một cái binding pattern tới Exchange.
        //Tất cả các messages có routing key khớp với pattern thì được điều hướng tới queue đó và đợi có consumer lấy ra sử dụng.
        public static void Consume(IModel chanel)
        {
            chanel.ExchangeDeclare("demo-topic-exchange", ExchangeType.Topic);
            chanel.QueueDeclare("demo-topic-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            chanel.QueueBind("demo-topic-queue", "demo-topic-exchange", "account.*");
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
