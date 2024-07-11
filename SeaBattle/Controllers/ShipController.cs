using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SeaBattle.Models.AuxilaryModels;
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

        public IActionResult PlaceShip(string json)
        {
            try
            {
                _shipService.PlaceShip(json);
            }
            catch (Exception ex) { return BadRequest(); }
            bool zz = true;
            string x = JsonConvert.SerializeObject(zz);
            return Ok(x);
        }

        public IActionResult GetCurrentShipLenght()
        {
            var result = _shipService.GetCurrentShipLenght();

            return Ok(result);
        }

        //public IActionResult Shoot(string json)
        //{

        //}


    }
}
