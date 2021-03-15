using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Systemintegration.Opgave.DigiZuite.Utilities;
using static Systemintegration.Opgave.DigiZuite.Pages.Index;

namespace Systemintegration.Opgave.DigiZuite.Services
{
    public class ConvertPublisher
    {
        private RoutingKey _routingKey;
        public ConvertPublisher(RoutingKey routingKey)
        {
            _routingKey = routingKey;
        }

        public void SendMessage(ConvertRequest message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "convert_tasks",
                                        type: "topic",
                                        durable: true);


                var routingKey = "convert." + _routingKey.GetRoutingKey(message.Type);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish(exchange: "convert_tasks",
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);

                //Debug.WriteLine(" [x] Sent '{0}'", routingKey);
            }
        }

    }
}
