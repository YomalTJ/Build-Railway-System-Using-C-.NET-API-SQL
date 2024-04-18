using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainManage.Models
{
    internal class TrainDto
    {
        [JsonProperty("trainId")]
        public int TrainId { get; set; }

        [JsonProperty("trainName")]
        public string TrainName { get; set; }

        [JsonProperty("startLocation")]
        public string StartLocation { get; set; }

        [JsonProperty("endLocation")]
        public string EndLocation { get; set; }

        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        [JsonProperty("endTime")]
        public string EndTime { get; set; }

        [JsonProperty("seats")]
        public int Seats { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }
    }
}
