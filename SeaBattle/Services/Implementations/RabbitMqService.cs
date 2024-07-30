using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Services.Interfaces;
using System.Text;
using System.Threading.Channels;

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


        public void SendMessage(object obj, string login)
        {
            var message = JsonConvert.SerializeObject(obj);
            SendMessage(message, login);
        }

        public void SendMessage(string message, string login)
        {
            var factory = new ConnectionFactory { HostName = _HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: $"{_RoutingKey}_{login}",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: $"{_RoutingKey}_{login}",
                                     basicProperties: null,
                                     body: body);

            }
        }

        public void Clear(string login)
        {
            var factory = new ConnectionFactory { HostName = _HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: $"{_RoutingKey}_{login}",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                channel.QueuePurge($"{_RoutingKey}_{login}");
            }
        }

        public List<RabbitMessage> GetAllMessagesByUser(string login)
        {
            List<RabbitMessage> messages = new List<RabbitMessage>();
            
            /*var cfactory = new ConnectionFactory { HostName = _HostName };
            var cconnection = cfactory.CreateConnection();
            var cchannel = cconnection.CreateModel();

            cchannel.QueueDeclare(queue: _RoutingKey, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(cchannel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                RabbitMessage message = JsonConvert.DeserializeObject<RabbitMessage>(content);
                messages.Add(message);
                cchannel.BasicNack(ea.DeliveryTag, false, true);
            };
            cchannel.BasicConsume(_RoutingKey, false, consumer);
            */
            return messages;
        }

        public RabbitMessage GetLastMessageByUser(string login)
        {
            RabbitMessage message = new RabbitMessage();


            return message;
        }
    }
}
