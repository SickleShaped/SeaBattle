using RabbitMQ.Client;
using System.Text;

namespace SeaBattle.Extensions
{
    public class Rabbit
    {
        void main()
        {
            var factory = new ConnectionFactory() {HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "dev-queue",
                    durable:false,
                    exclusive: false,
                    autoDelete:false,
                    arguments:null);

                string message = "MessageFromPublisher";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "localhost",
                                     basicProperties: null,
                                     body: body);
            }
        }
        
    }
    
    
}
