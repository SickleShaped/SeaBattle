using Newtonsoft.Json;
using RabbitMQ.Client;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Services.Interfaces;
using System.Text;

namespace SeaBattle.Services.Implementations
{
    public class RabbitMqService: IRabbitMqService
    {
        private readonly IConfiguration _configuration;
        private readonly string? _HostName;
        private readonly string? _RoutingKey;

        public RabbitMqService(IConfiguration configuration)
        {
            _configuration = configuration;
            _HostName = configuration["HostName"];
            _RoutingKey = configuration["RoutingKey"];
        }


        public void SendMessage(object obj)
        {
            var message = JsonConvert.SerializeObject(obj);
            SendMessage(message);
        }

        public void SendMessage(string message)
        {
            var factory = new ConnectionFactory { HostName = _HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _RoutingKey,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: _RoutingKey,
                                     basicProperties: null,
                                     body: body);

            }
        }
    }
}
