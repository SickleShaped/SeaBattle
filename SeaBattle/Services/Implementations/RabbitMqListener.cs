using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Diagnostics;
using System;
using Microsoft.Extensions.Configuration;
using SeaBattle.Services.Implementations;


public class RabbitMqListener : BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private readonly string _HostName;
    private readonly string _RoutingKey;

    public RabbitMqListener(IConfiguration configuration)
    {
        _HostName = configuration["HostName"];
        _RoutingKey = configuration["RoutingKey"];

        var factory = new ConnectionFactory { HostName = _HostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _RoutingKey, durable: false, exclusive: false, autoDelete: false, arguments: null);   
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());

            Console.WriteLine(content);

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(_RoutingKey, false, consumer);
        //_channel.BasicNack(_channel, consumer, true);
        //_channel.BasicNack()

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}