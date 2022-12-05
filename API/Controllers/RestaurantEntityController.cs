using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class RestaurantEntity : ControllerBase
{

    private readonly ILogger<RestaurantEntity> _logger;

    // public ReservationController(ILogger<RestaurantEntity> logger)
    // {
    //     _logger = logger;
    // }

    [HttpGet(Name = "getRestaurant")]
    public Tuple<Boolean, int, String, String, String, String> Get(int restaurantId)
    {
        RestaurantFunctions db = new RestaurantFunctions(); 
        db.restaurantDataFetch(); 
        return db.getRestaurant(restaurantId); 
    }

    [HttpPut(Name = "addRestaurant")]
    public void Put(String restaurantName, String location, String operatingHours, String description)
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO restaurant_entity VALUES (DEFAULT,'" + restaurantName + "','" + location + "','" + operatingHours + "','" + description + "');", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }
}
