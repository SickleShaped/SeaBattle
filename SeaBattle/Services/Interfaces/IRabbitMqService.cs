using SeaBattle.Models.AuxilaryModels;

namespace SeaBattle.Services.Interfaces
{
    public interface IRabbitMqService
    {
        void SendMessage(object obj, string login);
        void SendMessage(string message, string login);
        void Clear(string login);
        List<RabbitMessage> GetAllMessagesByUser(string login);
        RabbitMessage GetLastMessageByUser(string login);
    }
}
