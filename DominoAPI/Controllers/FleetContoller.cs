using DominoAPI.Models.Create.Fleet;
using DominoAPI.Models.Query;
using DominoAPI.Models.Update.Fleet;
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
        public async Task<IActionResult> GetAllFuelSupplies([FromQuery] QueryParams query)
        {
            var fuelNotes = await _fleetService.GetAllFuelSupplies(query);

            return Ok(fuelNotes);
        }

        [HttpGet("fuel-supply/{fuelSupplyId}/fuel-notes")]
        public async Task<IActionResult> GetFuelNotes([FromRoute] int fuelSupplyId, [FromQuery] QueryParams query)
        {
            var fuelNotes = await _fleetService.GetFuelNotes(fuelSupplyId, query);

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

        [HttpPost("fuel-notes")]
        public async Task<IActionResult> AddRecentFuelNote([FromBody] CreateFuelNoteDto dto)
        {
            await _fleetService.AddRecentFuelNote(dto);

            return Created("New note has been added", null);
        }

        [HttpPost("fuel-supply/{fuelSupplyId}/fuel-notes")]
        public async Task<IActionResult> AddManuallyFuelNote([FromBody] CreateFuelNoteDto dto, [FromRoute] int fuelSupplyId)
        {
            await _fleetService.AddManuallyFuelNote(dto, fuelSupplyId);

            return Created("New note has been added", null);
        }

        [HttpPut("cars/{carId}/notes")]
        public async Task<IActionResult> UpdateCarNote([FromBody] string note, [FromRoute] int carId)
        {
            await _fleetService.UpdateCarNote(note, carId);

            return Created("New note has been added", null);
        }

        [HttpPut("cars/{carId}")]
        public async Task<IActionResult> UpdateCar([FromBody] UpdateCarDto dto, [FromRoute] int carId)
        {
            await _fleetService.UpdateCar(dto, carId);

            return Ok();
        }

        [HttpPut("fuel-supply/{fuelSupplyId}")]
        public async Task<IActionResult> UpdateFuelSupply([FromBody] UpdateFuelSupplyDto dto, [FromRoute] int fuelSupplyId)
        {
            await _fleetService.UpdateFuelSupply(dto, fuelSupplyId);

            return Ok();
        }

        [HttpPut("fuel-supply/{fuelSupplyId}/{fuelNoteId}")]
        public async Task<IActionResult> UpdateFuelNote([FromBody] UpdateFuelNoteDto dto,
            [FromRoute] int fuelNoteId, [FromRoute] int fuelSupplyId)
        {
            await _fleetService.UpdateFuelNote(dto, fuelNoteId, fuelSupplyId);

            return Ok();
        }

        [HttpDelete("cars/{carId}")]
        public async Task<IActionResult> DeleteCar([FromRoute] int carId)
        {
            await _fleetService.DeleteCar(carId);

            return NoContent();
        }

        [HttpDelete("fuel-supply/{fuelSupplyId}")]
        public async Task<IActionResult> DeleteFuelSupply([FromRoute] int fuelSupplyId)
        {
            await _fleetService.DeleteFuelSupply(fuelSupplyId);

            return NoContent();
        }

        [HttpDelete("fuel-supply/{fuelSupplyId}/{fuelNoteId}")]
        public async Task<IActionResult> DeleteFuelNote([FromRoute] int fuelNoteId, [FromRoute] int fuelSupplyId)
        {
            await _fleetService.DeleteFuelNote(fuelNoteId, fuelSupplyId);

            return NoContent();
        }

        [HttpDelete("fuel-supply/{fuelSupplyId}/fuel-notes")]
        public async Task<IActionResult> DeleteFuelNoteRange([FromRoute] int fuelSupplyId, [FromBody] List<int> fuelNotesId)
        {
            await _fleetService.DeleteFuelNoteRange(fuelSupplyId, fuelNotesId);

            return NoContent();
        }

        [HttpDelete("fuel-supply/{fuelSupplyId}/fuel-notes/all")]
        public async Task<IActionResult> DeleteAllFuelNotes([FromRoute] int fuelSupplyId)
        {
            await _fleetService.DeleteAllFuelNotes(fuelSupplyId);

            return NoContent();
        }
    }
}