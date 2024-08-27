using Confluent.Kafka;
using Newtonsoft.Json;
using SeaBattle.Models.AuxilaryModels;
using System.Net.WebSockets;
using System.Text;

namespace SeaBattle.Services.Implementations.Consumer
{
    public static class ConsumerService
    {
        public static async Task ReadAllMessageFromClient(CancellationToken stoppingToken, string login, IConfiguration configuration)
        {
            IConsumer<Ignore, string> _consumer;
            var consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "GameGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };
            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();

            var topicPartition = new TopicPartitionOffset("Games", 0, Offset.Beginning);
            _consumer.Assign(topicPartition);

            ///интересно, насколько оно грузит комп7
            while (true)
            {
                try
                {
                    var socket = SocketService.GetSocket(login);
                    var consumeResult = _consumer.Consume();
                    var kafkamessage = JsonConvert.DeserializeObject<KafkaMessage>(consumeResult.Message.Value);
                    if (kafkamessage.Login == login) {
                        var player = kafkamessage.Player == Models.Enums.PlayerEnum.Player ? "Player" : "Enemy";
                        var message = $"{player} shooted in cell '{kafkamessage.Coordinate.X}, {kafkamessage.Coordinate.Y}'";
                        byte[] data = Encoding.ASCII.GetBytes($"{message}");
                        if (socket == null) throw new Exception("NoSocketExist");
                        socket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
                    }

                    if (consumeResult.IsPartitionEOF) break;
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Consume error: {e.Error}");
                }
            }
            _consumer.Close();
        }

    }
}
