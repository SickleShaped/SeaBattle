using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;

namespace SeaBattle.Services.Interfaces;

public interface IUserService
{
    public Task PlaceShip(string login, PlaceShipDto dto);


}
