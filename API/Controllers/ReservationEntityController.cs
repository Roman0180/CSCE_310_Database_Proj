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

    [HttpGet(Name = "checkReservations")]
    public List<Tuple<int, DateTime, int, int>> Get(int restaurantId)
    {
        ReservationFunctions db = new ReservationFunctions(); 
        db.reservationDataFetch(); 
        return db.getReservations(restaurantId); 
    }

    [HttpPut(Name = "createReservation")]
    public void Put(int reservationPartySize, DateTime reservationDateTime, int restaurantId, int reservationMaker)
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO reservation_entity VALUES (DEFAULT," + reservationPartySize + ",'" + reservationDateTime + "'," + restaurantId + "," + reservationMaker + ");", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }
}
