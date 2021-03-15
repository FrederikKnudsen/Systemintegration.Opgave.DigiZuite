using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Picture.Microservice
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueName = "convert_tasks.picture";

                channel.QueueDeclare("convert_tasks.picture", durable: true, exclusive: false);

                channel.QueueBind(queue: queueName,
                                  exchange: "convert_tasks",
                                  routingKey: "convert.picture.*");

                Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();

                    var message = JsonConvert.DeserializeObject<ConvertRequest>(Encoding.UTF8.GetString(body));

                    var routingKey = ea.RoutingKey;

                    Console.WriteLine($" [x] Converting image - Time to process: {message.Size / 1000} seconds.");

                    Console.WriteLine($" [x] Received '{routingKey}'");

                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
    public class ConvertRequest
    {
        public string FileName { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
    }
}
