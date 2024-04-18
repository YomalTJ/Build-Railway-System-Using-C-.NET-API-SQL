using System.ComponentModel.DataAnnotations;

namespace RailwayAPI.Models
{
    public class TrainDto
    {
        [Required]
        public int TrainId { get; set; }
        public string TrainName { get; set; } = "";
        public string StartLocation { get; set; } = "";
        public string EndLocation { get; set; } = "";
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
        public int Seats { get; set; }
        public decimal Distance { get; set; }
        public decimal Price { get; set; }

    }
}
