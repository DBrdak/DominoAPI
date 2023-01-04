using DominoAPI.Entities.Fleet;
using DominoAPI.Entities.Shops;
using System.ComponentModel.DataAnnotations;

namespace DominoAPI.Models.Update.Fleet
{
    public class UpdateCarDto
    {
        public string? RegistrationNumber { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }

        [MaxLength(30)]
        public string? Description { get; set; }

        [MaxLength(30)]
        public string? Note { get; set; }

        public int? Mileage { get; set; }
        public int? ShopId { get; set; }
    }
}