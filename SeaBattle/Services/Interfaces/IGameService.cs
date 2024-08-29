using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;

namespace SeaBattle.Services.Interfaces;

public interface IGameService
{
    Task<Game> StartGame(string login);
    Task<Game> GetGame(string login);
    Task<Game> RestartGame(string login);
    Task<Game> MakeTurn(string login, MakeTurnDto dto);
    Task<Game> AutoMakeTablePlayer(string login);

}
