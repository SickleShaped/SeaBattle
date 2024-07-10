using Microsoft.AspNetCore.Mvc;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Bot;
using SeaBattle.Services.Game;
using SeaBattle.Services.ShipService;

namespace SeaBattle.Controllers
{
    public class ShipController : Controller
    {
        private readonly ILogger<ShipController> _logger;
        private readonly IShipService _shipService;

        public ShipController(ILogger<ShipController> logger, IShipService shipService)
        {
            _logger = logger;
            _shipService = shipService;
        }

        public IActionResult FlipShip()
        {
            var result = _shipService.FlipShip();


            return Ok(result);
        }

        public IActionResult PlaceShip(int CellId, int ShipId, ShipDirection direction)
        {
            string result = "";
            /*
            try
            {
                result = _gameService.PlaceShip(json);
            }
            catch (Exception ex) { return BadRequest(); }
            */



            return Ok(result);
        }

    }
}
