using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SeaBattle.Models;
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

            List <Ship> shipList = new List<Ship>();
            shipList.Add(new Ship(4));
            shipList.Add(new Ship(3));
            ViewBag.Ships = shipList;
            
            return View();
        }

        public void InitTables()
        {
            
        }

        public void GetShips()
        {
             //GameService.GetShips();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
