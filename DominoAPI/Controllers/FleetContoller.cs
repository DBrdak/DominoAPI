using DominoAPI.Models.Create.Fleet;
using DominoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DominoAPI.Controllers
{
    [ApiController]
    [Route("fleet")]
    public class FleetContoller : ControllerBase
    {
        private readonly IFleetService _fleetService;

        public FleetContoller(IFleetService fleetService)
        {
            _fleetService = fleetService;
        }

        [HttpGet("cars")]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _fleetService.GetAllCars();

            return Ok(cars);
        }

        [HttpGet("fuel-supply")]
        public async Task<IActionResult> GetAllFuelSupplies()
        {
            var fuelNotes = await _fleetService.GetAllFuelSupplies();

            return Ok(fuelNotes);
        }

        [HttpGet("fuel-supply/{fuelSupplyId}/fuel-notes")]
        public async Task<IActionResult> GetFuelNotes([FromRoute] int fuelSupplyId)
        {
            var fuelNotes = await _fleetService.GetFuelNotes(fuelSupplyId);

            return Ok(fuelNotes);
        }

        [HttpPost("cars")]
        public async Task<IActionResult> AddCar([FromBody] CreateCarDto dto)
        {
            await _fleetService.AddCar(dto);

            return Created("New car has been added", null);
        }

        [HttpPost("fuel-supply")]
        public async Task<IActionResult> AddFuelSupply([FromBody] CreateFuelSupplyDto dto)
        {
            await _fleetService.AddFuelSupply(dto);

            return Created("New supply has been added", null);
        }

        [HttpPost("fuel-note")]
        public async Task<IActionResult> AddRecentFuelNote([FromBody] CreateFuelNoteDto dto)
        {
            await _fleetService.AddRecentFuelNote(dto);

            return Created("New note has been added", null);
        }

        [HttpPost("fuel-supply/{fuelSupplyId}/fuel-note")]
        public async Task<IActionResult> AddManuallyFuelNote([FromBody] CreateFuelNoteDto dto, [FromRoute] int fuelSupplyId)
        {
            await _fleetService.AddManuallyFuelNote(dto, fuelSupplyId);

            return Created("New note has been added", null);
        }
    }
}