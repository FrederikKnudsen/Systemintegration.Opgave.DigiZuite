using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace DeadLetters
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueBind("convert_tasks.notsupported", 
                    "convert_tasks", 
                    "convert.notsupported");

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, ea) =>
                {
                    Console.WriteLine("Deadletter: " + ea.Body);
                };

                channel.BasicConsume("convert_tasks.notsupported", true, consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
