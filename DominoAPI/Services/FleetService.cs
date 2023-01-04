﻿using System.Reflection;
using AutoMapper;
using DominoAPI.Entities;
using DominoAPI.Entities.Fleet;
using DominoAPI.Models.Create.Fleet;
using DominoAPI.Models.Display.Fleet;
using DominoAPI.Models.Update.Fleet;
using Microsoft.EntityFrameworkCore;

namespace DominoAPI.Services
{
    public interface IFleetService
    {
        Task<IEnumerable<DisplayCarDto>> GetAllCars();

        Task<IEnumerable<DisplayFuelSupplyDto>> GetAllFuelSupplies();

        Task<DisplayFuelSupplyDto> GetFuelNotes(int fuelSupplyId);

        Task AddCar(CreateCarDto dto);

        Task AddFuelSupply(CreateFuelSupplyDto dto);

        Task AddRecentFuelNote(CreateFuelNoteDto dto);

        Task AddManuallyFuelNote(CreateFuelNoteDto dto, int fuelSupplyId);

        Task UpdateCarNote(string note, int carId);

        Task UpdateCar(UpdateCarDto dto, int carId);

        Task UpdateFuelSupply(UpdateFuelSupplyDto dto, int fuelSupplyId);
    }

    public class FleetService : IFleetService
    {
        private readonly DominoDbContext _dbContext;
        private readonly IMapper _mapper;

        public FleetService(IMapper mapper, DominoDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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

        public async Task<IEnumerable<DisplayFuelSupplyDto>> GetAllFuelSupplies()
        {
            var fuelSupplies = await _dbContext.FuelSupplies
                .AsNoTracking()
                .ToListAsync();

            var dto = _mapper.Map<List<DisplayFuelSupplyDto>>(fuelSupplies);

            return dto;
        }

        public async Task<DisplayFuelSupplyDto> GetFuelNotes(int fuelSupplyId)
        {
            var fuelSupply = await _dbContext.FuelSupplies
                .Include(fs => fs.FuelNotes)
                .ThenInclude(fn => fn.Car)
                .AsNoTracking()
                .FirstOrDefaultAsync(fs => fs.Id == fuelSupplyId);

            if (fuelSupply == null)
            {
                throw new Exception();
            }

            var dto = _mapper.Map<DisplayFuelSupplyDto>(fuelSupply);

            return dto;
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
                throw new Exception();
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

            if (lastFuelSupply is null)
            {
                throw new Exception();
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

            if (fuelSupply is null)
            {
                throw new Exception();
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
                throw new Exception();
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
                throw new Exception();
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
                throw new Exception();
            }

            if (dto.DateOfDelivery > DateTime.UtcNow)
            {
                throw new Exception();
            }

            dto.MapTo(fuelSupply);

            _dbContext.FuelSupplies.Update(fuelSupply);
            await _dbContext.SaveChangesAsync();
        }
    }
}