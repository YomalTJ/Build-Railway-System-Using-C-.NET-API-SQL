using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RailwayAPI.Models;
using System.Data.SqlClient;

namespace RailwayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainsController : ControllerBase
    {
        private readonly string connectionString;
        public TrainsController(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? "";
        }
        [HttpPost]
        public IActionResult CreateTrain(TrainDto trainDto)
        {
            try
            {
                using(var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO Trains " +
                        "(TrainId, TrainName, StartLocation, EndLocation, StartTime, EndTime, Seats, Distance, Price) VALUES " +
                        "(@TrainId, @TrainName, @StartLocation, @EndLocation, @StartTime, @EndTime, @Seats, @Distance, @Price)";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TrainId", trainDto.TrainId);
                        command.Parameters.AddWithValue("@TrainName", trainDto.TrainName);
                        command.Parameters.AddWithValue("@StartLocation", trainDto.StartLocation);
                        command.Parameters.AddWithValue("@EndLocation", trainDto.EndLocation);
                        command.Parameters.AddWithValue("@StartTime", trainDto.StartTime);
                        command.Parameters.AddWithValue("@EndTime", trainDto.EndTime);
                        command.Parameters.AddWithValue("@Seats", trainDto.Seats);
                        command.Parameters.AddWithValue("@Distance", trainDto.Distance);
                        command.Parameters.AddWithValue("@Price", trainDto.Price);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Train", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }

            return Ok();
        }
        [HttpGet]
        public IActionResult GetTrains()
        {
            List<Train> trains = new List<Train>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM Trains";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                Train train = new Train();

                                train.TrainId = reader.GetInt32(0);
                                train.TrainName = reader.GetString(1);
                                train.StartLocation = reader.GetString(2);
                                train.EndLocation = reader.GetString(3);
                                train.StartTime = reader.GetString(4);
                                train.EndTime = reader.GetString(5);
                                train.Seats = reader.GetInt32(6);
                                train.Distance = reader.GetDecimal(7);
                                train.Price = reader.GetDecimal(8);

                                trains.Add(train);
                            }
                        }
                    }
                }
            }
            catch( Exception ex )
            {
                ModelState.AddModelError("Train", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }

            return Ok(trains);
        }

        [HttpGet("{TrainId}")]
        public IActionResult GetTrain(int id)
        {
            Train train = new Train();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM Trains WHERE TrainId=@TrainId";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TrainId", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                train.TrainId = reader.GetInt32(0);
                                train.TrainName = reader.GetString(1);
                                train.StartLocation = reader.GetString(2);
                                train.EndLocation = reader.GetString(3);
                                train.StartTime = reader.GetString(4);
                                train.EndTime = reader.GetString(5);
                                train.Seats = reader.GetInt32(6);
                                train.Distance = reader.GetDecimal(7);
                                train.Price = reader.GetDecimal(8);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Train", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }

            return Ok(train);
        }

        [HttpPut("{TrainId}")]
        public IActionResult UpdateTrain(int id, TrainDto trainDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "UPDATE Trains SET TrainId=@TrainId, TrainName=@TrainName, StartLocation=@StartLocation, " +
                        "EndLocation=@EndLocation, StartTime=@StartTime, EndTime=@EndTime, Seats=@Seats, Distance=@Distance, Price=@Price WHERE TrainId=@TrainId";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TrainId", trainDto.TrainId);
                        command.Parameters.AddWithValue("@TrainName", trainDto.TrainName);
                        command.Parameters.AddWithValue("@StartLocation", trainDto.StartLocation);
                        command.Parameters.AddWithValue("@EndLocation", trainDto.EndLocation);
                        command.Parameters.AddWithValue("@StartTime", trainDto.StartTime);
                        command.Parameters.AddWithValue("@EndTime", trainDto.EndTime);
                        command.Parameters.AddWithValue("@Seats", trainDto.Seats);
                        command.Parameters.AddWithValue("@Distance", trainDto.Distance);
                        command.Parameters.AddWithValue("@Price", trainDto.Price);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Train", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpDelete("{TrainId}")]
        public IActionResult DeleteTrain(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM Trains WHERE TrainId=@TrainId";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TrainId", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Train", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok();
        }
    }
}
