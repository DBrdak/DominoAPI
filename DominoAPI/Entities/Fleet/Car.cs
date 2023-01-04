using System.ComponentModel.DataAnnotations;
using DominoAPI.Entities.Shops;

namespace DominoAPI.Entities.Fleet
{
    public class Car
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

        [MaxLength(30)]
        public string? Description { get; set; }

        [MaxLength(30)]
        public string? Note { get; set; }

        public int? Mileage { get; set; }

        public IEnumerable<FuelNote> FuelNotes { get; set; }
        public virtual Shop? Shop { get; set; }
    }
}