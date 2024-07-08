using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Models.Tables;
using SeaBattle.Services;
using System.Diagnostics;

namespace SeaBattle.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGameService _gameService;

        public HomeController(ILogger<HomeController> logger, IGameService service)
        {
            _logger = logger;
            _gameService = service;
        }

        public IActionResult Index()
        {
            ViewData["TableCapacity"] = 10;

            return View();
        }

        public IActionResult GetInitialData()
        {
            string json = _gameService.GetInitialData();

            return Ok(json);
        }

        public IActionResult PlaceShip(string json/*Table Table, byte shipLenght, ShipDirection direction*/)
        {
            string result = _gameService.PlaceShip(json);


            return Ok(json);
        }

    }
}
