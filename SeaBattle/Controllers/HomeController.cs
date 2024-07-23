using Microsoft.AspNetCore.Mvc;
using SeaBattle.Services.Implementations;
using SeaBattle.Services.Interfaces;
using System.Diagnostics;

namespace SeaBattle.Controllers;

/// <summary>
/// �������� ����������
/// </summary>
public class HomeController : Controller
{
    private readonly IGameService _gameService;
    private readonly BotService _botService;

    public HomeController(IGameService gameService, BotService botService)
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
        var data = await _gameService.GetGame(login);
        

        return Ok(data);
    }

    [HttpPut]
    public async Task<IActionResult> RestartGame()
    {
        

        string login = HttpContext.Request.Headers.UserAgent.ToString();
        var data = await _gameService.RestartGame(login);

        
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> StartGame()
    {
        string login = HttpContext.Request.Headers.UserAgent.ToString();
        await _gameService.StartGame(login);
        return Ok();
    }

    
}
