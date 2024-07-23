using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;

namespace SeaBattle.Services.Interfaces;

public interface IGameService
{
    Task StartGame(string login);
    Task<string> GetGame(string login);
    Task<string> RestartGame(string login);
    Task MakeTurn(string login, MakeTurnDto dto);
    Task AutoMakeTablePlayer(string login);

}
