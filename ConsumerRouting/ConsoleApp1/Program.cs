using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

class Receive
{
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "logexc",
                                    type: "direct");
            var queueName1 = channel.QueueDeclare().QueueName;
            var queueName2 = channel.QueueDeclare().QueueName;
            Console.WriteLine(queueName1.ToString());
            Console.WriteLine(queueName2.ToString());
            channel.QueueBind(queue: queueName1,
                  exchange: "logexc",
                  routingKey: "black");
            channel.QueueBind(queue: queueName2,
                 exchange: "logexc",
                 routingKey: "white");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine("Starting process for ");
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
               // Thread.Sleep(message.Length * 1000);
                Console.WriteLine(" [x] Received {0}", message);
               // channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            channel.BasicConsume(queue: queueName1,
                                 autoAck: true,
                                 consumer: consumer);
            channel.BasicConsume(queue: queueName2,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Consumer1");
            Console.ReadLine();
        }
    }
}
