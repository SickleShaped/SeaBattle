namespace SeaBattle.Services.Interfaces
{
    public interface IProducerService
    {
        public Task ProduceAsync(string login, string message);
    }
}
