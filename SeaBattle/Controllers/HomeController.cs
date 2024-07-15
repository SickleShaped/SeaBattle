using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Bot;
using SeaBattle.Services.Game;
using System.Diagnostics;

namespace SeaBattle.Controllers
{
    /// <summary>
    /// �������� ����������
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGameService _gameService;
        private readonly IBotService _botService;

        public HomeController(ILogger<HomeController> logger, IGameService gameService, IBotService botService)
        {
            _logger = logger;
            _gameService = gameService;
            _botService = botService;
        }

        /// <summary>
        /// �������� ������� ��������� ����
        /// </summary>
        /// <returns></returns>
        public IActionResult GetGame()
        {
            var data = _gameService.GetGameData();
            return Ok(data);
        }

        /// <summary>
        /// ����������� ����
        /// </summary>
        /// <returns></returns>
        public IActionResult RestartGame()
        {
            var data = _gameService.RestartGame();
            return Ok(data);
        }

        /// <summary>
        /// ��������� ������
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// ������ ����
        /// </summary>
        /// <returns></returns>
        public IActionResult StartGame()
        {
            try
            {
                _gameService.StartGame();
            }
            catch (Exception ex) { return BadRequest(); }

            return Ok(true);
        }

    }
}
