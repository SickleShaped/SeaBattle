using Confluent.Kafka;
using SeaBattle.Services.Interfaces;
using static Confluent.Kafka.ConfigPropertyNames;

namespace SeaBattle.Services.Implementations;
public class ProducerService: IProducerService
{
    private readonly IConfiguration _configuration;

    private readonly IProducer<Null, string> _producer;

    public ProducerService(IConfiguration configuration)
    {
        _configuration = configuration;

        var producerconfig = new ProducerConfig
        {
            BootstrapServers = _configuration["Kafka:BootstrapServers"]
        };

        _producer = new ProducerBuilder<Null, string>(producerconfig).Build();
    }

    public async Task ProduceAsync(string login, string message)
    {
        var kafkamessage = new Message<Null, string> { Value = message, };

        Console.WriteLine("Отправил " + message);
        var result = await _producer.ProduceAsync("Games", kafkamessage);
    }
}
