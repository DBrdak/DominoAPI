using System.Collections.Immutable;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using DominoAPI.Entities;
using DominoAPI.Entities.Fleet;
using DominoAPI.Exceptions;
using DominoAPI.Models.Create.Fleet;
using DominoAPI.Models.Display.Fleet;
using DominoAPI.Models.Query;
using DominoAPI.Models.Update.Fleet;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using UtilityLibrary;

namespace DominoAPI.Services
{
    public interface IFleetService
    {
        Task<IEnumerable<DisplayCarDto>> GetAllCars();

        Task<PagedResult<DisplayFuelSupplyDto>> GetAllFuelSupplies(FuelSuppliesQueryParams query);

        Task<PagedResult<DisplayFuelNoteDto>> GetFuelNotes(int fuelSupplyId, FuelNotesQueryParams query);

        Task AddCar(CreateCarDto dto);

        Task AddFuelSupply(CreateFuelSupplyDto dto);

        Task AddRecentFuelNote(CreateFuelNoteDto dto);

        Task AddManuallyFuelNote(CreateFuelNoteDto dto, int fuelSupplyId);

        Task UpdateCarNote(string note, int carId);

        Task UpdateCar(UpdateCarDto dto, int carId);

        Task UpdateFuelSupply(UpdateFuelSupplyDto dto, int fuelSupplyId);

        Task UpdateFuelNote(UpdateFuelNoteDto dto, int fuelNoteId, int fuelSupplyId);

        Task DeleteCar(int carId);

        Task DeleteFuelSupply(int fuelSupplyId);

        Task DeleteFuelNote(int fuelNoteId, int fuelSupplyId);

        Task DeleteFuelNoteRange(int fuelSupplyId, List<int> fuelNotesId);

        Task DeleteAllFuelNotes(int fuelSupplyId);
    }

    public class FleetService : IFleetService
    {
        private readonly DominoDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<FleetService> _logger;

        public FleetService(IMapper mapper, DominoDbContext dbContext, ILogger<FleetService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<DisplayCarDto>> GetAllCars()
        {
            var cars = await _dbContext.Cars
                .Include(c => c.Shop)
                .AsNoTracking()
                .ToListAsync();

            var dto = _mapper.Map<IEnumerable<DisplayCarDto>>(cars);

            return dto;
        }

        public async Task<PagedResult<DisplayFuelSupplyDto>> GetAllFuelSupplies(FuelSuppliesQueryParams query)
        {
            DateTime.TryParse(query.SearchPhrase, out var dateOfDelivery);
            var list = new List<int>(3) { 1, 2, 3 };
            var baseFuelSupplies = await _dbContext.FuelSupplies
                .AsNoTracking().
                Where(fs => query.SearchPhrase == null
                            || (fs.DateOfDelivery >= dateOfDelivery &&
                                fs.DateOfDelivery < dateOfDelivery.AddDays(1)))
                .ToListAsync();

            baseFuelSupplies = baseFuelSupplies.Sort(query.SortBy, query.SortDirection.ToString()).ToList();

            var fuelSupplies = baseFuelSupplies.GetPage(query.PageSize, query.PageId);

            if (!fuelSupplies.Any())
            {
                throw new NotFoundException("Content not found");
            }

            var dto = _mapper.Map<List<DisplayFuelSupplyDto>>(fuelSupplies);

            var result =
                new PagedResult<DisplayFuelSupplyDto>(dto, baseFuelSupplies.Count, query.PageSize, query.PageId);

            return result;
        }

        public async Task<PagedResult<DisplayFuelNoteDto>> GetFuelNotes(int fuelSupplyId, FuelNotesQueryParams query)
        {
            DateTime.TryParse(query.SearchPhrase, out var dateOfNote);

            var baseFuelNotes = await _dbContext.FuelNotes
                .Include(fn => fn.Car)
                .AsNoTracking()
                .Where(fn => fn.FuelSupplyId == fuelSupplyId)
                .Where(fn => query.SearchPhrase == null ||
                             fn.Car.RegistrationNumber == query.SearchPhrase ||
                             (fn.Date >= dateOfNote && fn.Date < dateOfNote.AddDays(1)))
                .ToListAsync();

            baseFuelNotes = baseFuelNotes.Sort(query.SortBy, query.SortDirection.ToString()).ToList();

            var fuelNotes = baseFuelNotes.GetPage(query.PageSize, query.PageId);

            if (!fuelNotes.Any())
            {
                throw new NotFoundException("Content not found");
            }

            var dto = _mapper.Map<IList<DisplayFuelNoteDto>>(fuelNotes);

            var result = new PagedResult<DisplayFuelNoteDto>(dto, baseFuelNotes.Count, query.PageSize, query.PageId);

            return result;
        }

        public async Task AddCar(CreateCarDto dto)
        {
            var newCar = _mapper.Map<Car>(dto);

            await _dbContext.Cars.AddAsync(newCar);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddFuelSupply(CreateFuelSupplyDto dto)
        {
            var lastfuelSupply = await _dbContext.FuelSupplies
                .OrderByDescending(fs => fs.DateOfDelivery)
                .AsNoTracking()
                .LastOrDefaultAsync();

            if (lastfuelSupply is not null && (dto.DateOfDelivery > DateTime.UtcNow || dto.DateOfDelivery < lastfuelSupply.DateOfDelivery))
            {
                throw new BadRequestException("Wrong date insert");
            }

            var newFuelSupply = _mapper.Map<FuelSupply>(dto);

            await _dbContext.AddAsync(newFuelSupply);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRecentFuelNote(CreateFuelNoteDto dto)
        {
            var lastFuelSupply = await _dbContext.FuelSupplies
                .OrderBy(fs => fs.DateOfDelivery)
                .AsNoTracking()
                .LastOrDefaultAsync();

            var car = await _dbContext.Cars
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == dto.CarId);

            if (lastFuelSupply is null || car is null)
            {
                throw new NotFoundException("Content not found");
            }

            dto.Date = DateTime.Now;

            var newFuelNote = _mapper.Map<FuelNote>(dto);

            newFuelNote.FuelSupplyId = lastFuelSupply.Id;

            lastFuelSupply.CurrentVolume -= newFuelNote.Volume;

            _dbContext.Update(lastFuelSupply);
            await _dbContext.AddAsync(newFuelNote);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddManuallyFuelNote(CreateFuelNoteDto dto, int fuelSupplyId)
        {
            var fuelSupply = await _dbContext.FuelSupplies
                .AsNoTracking()
                .FirstOrDefaultAsync(fs => fs.Id == fuelSupplyId);

            var car = await _dbContext.Cars
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == dto.CarId);

            if (fuelSupply is null || car is null)
            {
                throw new NotFoundException("Content not found");
            }

            var newFuelNote = _mapper.Map<FuelNote>(dto);

            newFuelNote.FuelSupplyId = fuelSupplyId;

            fuelSupply.CurrentVolume -= newFuelNote.Volume;

            await _dbContext.AddAsync(newFuelNote);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCarNote(string note, int carId)
        {
            var car = await _dbContext.Cars
                .FirstOrDefaultAsync(c => c.Id == carId);

            if (car is null)
            {
                throw new NotFoundException("Content not found");
            }

            car.Note = note;

            _dbContext.Cars.Update(car);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCar(UpdateCarDto dto, int carId)
        {
            var car = await _dbContext.Cars
                .FirstOrDefaultAsync(c => c.Id == carId);

            if (car is null)
            {
                throw new NotFoundException("Content not found");
            }

            dto.MapTo(car);

            _dbContext.Cars.Update(car);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateFuelSupply(UpdateFuelSupplyDto dto, int fuelSupplyId)
        {
            var fuelSupply = await _dbContext.FuelSupplies
                .FirstOrDefaultAsync(fs => fs.Id == fuelSupplyId);

            if (fuelSupply is null)
            {
                throw new NotFoundException("Content not found");
            }

            if (dto.DateOfDelivery > DateTime.UtcNow)
            {
                throw new BadRequestException("Wrong date insert");
            }

            dto.MapTo(fuelSupply);

            _dbContext.FuelSupplies.Update(fuelSupply);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateFuelNote(UpdateFuelNoteDto dto, int fuelNoteId, int fuelSupplyId)
        {
            var fuelSupply = await _dbContext.FuelSupplies
                .Include(fs => fs.FuelNotes)
                .AsNoTracking()
                .FirstOrDefaultAsync(fs => fs.Id == fuelSupplyId);

            var fuelNote = fuelSupply.FuelNotes
                .FirstOrDefault(fn => fn.Id == fuelNoteId);

            if (fuelNote is null || fuelSupply is null)
            {
                throw new NotFoundException("Content not found");
            }

            if (dto.Date > DateTime.UtcNow || dto.Date < fuelSupply.DateOfDelivery)
            {
                throw new BadRequestException("Wrong date insert");
            }

            if (dto.CarId is not null)
            {
                var car = await _dbContext.Cars
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == dto.CarId);

                if (car is null)
                {
                    throw new NotFoundException("Content not found");
                }
            }

            dto.MapTo(fuelNote);

            _dbContext.Update(fuelNote);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCar(int carId)
        {
            var car = await _dbContext.Cars
                .Include(c => c.Shop)
                .FirstOrDefaultAsync(c => c.Id == carId);

            if (car is null)
            {
                throw new NotFoundException("Content not found");
            }

            _dbContext.Remove(car);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteFuelSupply(int fuelSupplyId)
        {
            var fuelSupply = await _dbContext.FuelSupplies
                .FirstOrDefaultAsync(fs => fs.Id == fuelSupplyId);

            if (fuelSupply is null)
            {
                throw new NotFoundException("Content not found");
            }

            _dbContext.Remove(fuelSupply);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteFuelNote(int fuelNoteId, int fuelSupplyId)
        {
            var fuelSupply = await _dbContext.FuelSupplies
                .Include(fs => fs.FuelNotes)
                .AsNoTracking()
                .FirstOrDefaultAsync(fs => fs.Id == fuelSupplyId);

            var fuelNote = fuelSupply.FuelNotes
                .FirstOrDefault(fn => fn.Id == fuelNoteId);

            if (fuelNote is null || fuelSupply is null)
            {
                throw new NotFoundException("Content not found");
            }

            _dbContext.Remove(fuelNote);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteFuelNoteRange(int fuelSupplyId, List<int> fuelNotesId)
        {
            var fuelSupply = await _dbContext.FuelSupplies
                .Include(fs => fs.FuelNotes)
                .AsNoTracking()
                .FirstOrDefaultAsync(fs => fs.Id == fuelSupplyId);

            if (fuelSupply is null)
            {
                throw new NotFoundException("Content not found");
            }

            var fuelNotes = new List<FuelNote>();

            foreach (var fuelNoteId in fuelNotesId)
            {
                var fuelNote = fuelSupply.FuelNotes
                    .FirstOrDefault(fn => fn.Id == fuelNoteId);

                if (fuelNote is null)
                {
                    throw new NotFoundException("Content not found");
                }

                fuelNotes.Add(fuelNote);
            }

            _dbContext.RemoveRange(fuelNotes);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAllFuelNotes(int fuelSupplyId)
        {
            var fuelSupply = await _dbContext.FuelSupplies
                .Include(fs => fs.FuelNotes)
                .AsNoTracking()
                .FirstOrDefaultAsync(fs => fs.Id == fuelSupplyId);

            if (fuelSupply is null)
            {
                throw new NotFoundException("Content not found");
            }

            _dbContext.RemoveRange(fuelSupply.FuelNotes);
            await _dbContext.SaveChangesAsync();
        }
    }
}