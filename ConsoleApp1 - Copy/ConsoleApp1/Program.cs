using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

class Receive
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "myexc", type: "fanout");
            var queueName = channel.QueueDeclare().QueueName;
           // channel.BasicQos(0, 1, false);
            channel.QueueBind(queue:queueName,
                exchange: "myexc",
                routingKey: "");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine("Started");
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                //Thread.Sleep(message.Length * 1000);
                Console.WriteLine(" [x] Received {0}", message);
                //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

            Console.WriteLine(" Console1");
            Console.ReadLine();
        }
    }
}
