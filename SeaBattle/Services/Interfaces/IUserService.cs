using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;

namespace SeaBattle.Services.Interfaces;

public interface IUserService
{
    public Task<Game> PlaceShip(string login, PlaceShipDto dto);


}
