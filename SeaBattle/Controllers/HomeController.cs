using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Services.Implementations;
using SeaBattle.Services.Implementations.Consumer;
using SeaBattle.Services.Interfaces;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace SeaBattle.Controllers;

/// <summary>
/// Основной контроллер
/// </summary>
public class HomeController : Controller
{
    private readonly IGameService _gameService;
    private readonly IGamer _botService;

    public HomeController(IGameService gameService, IGamer botService)
    {
        _gameService = gameService;
        _botService = botService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetGame()
    {
        string login = HttpContext.Request.Headers.UserAgent.ToString();
        var data = JsonConvert.SerializeObject(await _gameService.GetGame(login));
        
        return Ok(data);
    }

    [HttpPut]
    public async Task<IActionResult> RestartGame()
    {
        string login = HttpContext.Request.Headers.UserAgent.ToString();
        var data = JsonConvert.SerializeObject(await _gameService.RestartGame(login));
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> StartGame()
    {
        string login = HttpContext.Request.Headers.UserAgent.ToString();
        var data = JsonConvert.SerializeObject(await _gameService.StartGame(login));
        return Ok(data);
    }

    
}
