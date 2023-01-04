using System.ComponentModel.DataAnnotations;

namespace DominoAPI.Models.Display.Fleet
{
    public class DisplayCarDto
    {
        public string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
        public uint? Mileage { get; set; }

        public int? ShopNumber { get; set; }
    }
}