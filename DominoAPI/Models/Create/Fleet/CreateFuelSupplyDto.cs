using System.ComponentModel.DataAnnotations;

namespace DominoAPI.Models.Create.Fleet
{
    public class CreateFuelSupplyDto
    {
        [Required]
        public DateTime DateOfDelivery { get; set; }

        [Required]
        public int DeliveryVolume { get; set; }

        [Required]
        public float Price { get; set; }
    }
}