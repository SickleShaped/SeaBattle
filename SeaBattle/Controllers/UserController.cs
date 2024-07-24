using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Services.Implementations;
using SeaBattle.Services.Interfaces;
using System.Diagnostics;

namespace SeaBattle.Controllers;

/// <summary>
/// Контроллер пользователей
/// </summary>
public class UserController : Controller
{
    private readonly IGameService _gameService;
    private readonly UserService _userService;

    public UserController(IGameService gameService, UserService userService)
    {
        _gameService = gameService;
        _userService = userService;
    }

    /// <summary>
    /// Поставить корабль
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>

    [HttpPost]
    public async Task<IActionResult> PlaceShip(string json)
    {
        string login = HttpContext.Request.Headers.UserAgent.ToString();
        var data = JsonConvert.SerializeObject(await _userService.PlaceShip(login, JsonConvert.DeserializeObject<PlaceShipDto>(json)));
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceAllShip(string json)
    {
        string login = HttpContext.Request.Headers.UserAgent.ToString();
        var data = JsonConvert.SerializeObject(await _gameService.AutoMakeTablePlayer(login));
        return Ok(data);
    }
    
    /// <summary>
    /// Выстрелить по полю
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>

    [HttpPut]
    public async Task<IActionResult> Shoot(string json)
    {
        string login = HttpContext.Request.Headers.UserAgent.ToString();
        var data = JsonConvert.SerializeObject(await _gameService.MakeTurn(login, JsonConvert.DeserializeObject<MakeTurnDto>(json)));
        return Ok(data);
    }
}
