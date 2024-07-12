using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SeaBattle.Models.AuxilaryModels;
using SeaBattle.Models.Enums;
using SeaBattle.Services.Bot;
using SeaBattle.Services.Game;
using SeaBattle.Services.ShipService;
using SeaBattle.Services.UserService;

namespace SeaBattle.Controllers
{
    public class ShipController : Controller
    {
        private readonly ILogger<ShipController> _logger;
        private readonly IShipService _shipService;
        private readonly IUserService _userService;

        public ShipController(ILogger<ShipController> logger, IShipService shipService, IUserService userService)
        {
            _logger = logger;
            _shipService = shipService;
            _userService = userService;
        }

        /// <summary>
        /// Повернуть корабль (из Горизонтально в Вертикально) или наоборот
        /// </summary>
        /// <returns></returns>
        public IActionResult FlipShip()
        {
            var result = _shipService.FlipShip();


            return Ok(result);
        }     

        /// <summary>
        /// Получить длину текущего корабля
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCurrentShipLenght()
        {
            var result = _shipService.GetCurrentShipLenght();

            return Ok(result);
        }
    }
}
