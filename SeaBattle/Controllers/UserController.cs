using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SeaBattle.Services.Game;
using SeaBattle.Services.ShipService;
using SeaBattle.Services.UserService;

namespace SeaBattle.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<ShipController> _logger;
        private readonly IGameService _gameService;
        private readonly IUserService _userService;

        public UserController(ILogger<ShipController> logger, IGameService gameService, IUserService userService)
        {
            _logger = logger;
            _gameService = gameService;
            _userService = userService;
        }

        /// <summary>
        /// Поставить корабль
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IActionResult PlaceShip(string json)
        {
            try
            {
                _userService.PlaceShip(json);
            }
            catch
            {
                return BadRequest();
            }
            string result = JsonConvert.SerializeObject(true);
            return Ok(result);
        }

        /// <summary>
        /// Выстрелить по полю
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IActionResult Shoot(string json)
        {
            string result;
            try
            {
                result = _gameService.MakeTurn(json);
            }
            catch (Exception ex ) { return BadRequest(); }

            return Ok(result);
        }
    }
}
