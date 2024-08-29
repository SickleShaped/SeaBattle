using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;

namespace SeaBattle.Services.Interfaces;

public interface IUserService : IGamer
{
    Task<Game> PlaceShip(string login, PlaceShipDto dto);
}