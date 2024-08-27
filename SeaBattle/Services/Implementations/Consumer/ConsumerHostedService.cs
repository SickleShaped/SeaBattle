using Confluent.Kafka;
using Fleck;
using Newtonsoft.Json;
using SeaBattle.Models.AuxilaryModels;
using System;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.WebSockets;
using System.Text;

namespace SeaBattle.Services.Implementations.Consumer;
public class ConsumerHostedService : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;

    protected async override Task ExecuteAsync(CancellationToken cancellingToken)
    {
        _consumer.Subscribe("Games");
        while (!cancellingToken.IsCancellationRequested)
        {
            await ProcessKafkaMessage(cancellingToken);
        }
        _consumer.Unsubscribe();
    }

    public ConsumerHostedService(IConfiguration configuration)
    {

        var consumerConfig = new ConsumerConfig()
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            GroupId = "GameGroup",
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
    }

    public async Task ProcessKafkaMessage(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            try
            { 
                var consumeResult = _consumer.Consume(stoppingToken);
                var kafkamessage = JsonConvert.DeserializeObject<KafkaMessage>(consumeResult.Message.Value);
                var socket = SocketService.GetSocket(kafkamessage.Login);
                var player = kafkamessage.Player == Models.Enums.PlayerEnum.Player ? "Player" : "Enemy";
                var message = $"{player} shooted in cell '{kafkamessage.Coordinate.X}, {kafkamessage.Coordinate.Y}'";
                byte[] data = Encoding.ASCII.GetBytes($"{message}");
                if (socket == null) throw new Exception("NoSocketExist");
                socket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);

                Console.WriteLine($"Received inventory update: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing Kafka message: {ex.Message}");
            }

        });
    }

}
