using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationEntityController : ControllerBase
{

    private readonly ILogger<ReservationEntityController> _logger;

    // public ReservationController(ILogger<ReservationEntityController> logger)
    // {
    //     _logger = logger;
    // }

    [HttpGet("checkReservations")]
    public List<String> Get(int restaurantId, DateTime startDate)
    {
        ReservationFunctions db = new ReservationFunctions(); 
        db.reservationDataFetch(); 
        return db.getReservations(restaurantId, startDate); 
    }

    [HttpGet("getAllReservations")]
    public List<Tuple<int, DateTime, int>> Get(int userId)
    {
        ReservationFunctions db = new ReservationFunctions(); 
        db.reservationDataFetch(); 
        return db.getReservations(userId); 
    }

    [HttpPost("createReservation")]
    public void Put(int reservationPartySize, DateTime reservationDateTime, int restaurantId, int reservationMaker)
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO reservation_entity VALUES (DEFAULT," + reservationPartySize + ",'" + reservationDateTime + "'," + restaurantId + "," + reservationMaker + ");", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }
}
