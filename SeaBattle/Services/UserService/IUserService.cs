using SeaBattle.Models;

namespace SeaBattle.Services.UserService
{
    public interface IUserService
    {
        public void PlaceShip(string json);
        
        public bool Shoot(Table table, int x, int y);
    }
}
