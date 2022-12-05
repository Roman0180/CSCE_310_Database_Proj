using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewEntityController : ControllerBase
{

    private readonly ILogger<ReviewEntityController> _logger;


    [HttpGet(Name = "checkReviews")]
    public List<Tuple<int, int, string, DateTime, int, string, int>> Get(int restaurantId)
    {
        ReviewFunctions db = new ReviewFunctions(); 
        db.reviewDataFetch(); 
        return db.getReviews(restaurantId); 
    }

    [HttpPut(Name = "createReview")]
    public void Put(int order_num, int rating, string text, DateTime date_posted, int restaurantId, string comment, int employee_id)
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO review_entity VALUES (DEFAULT," + order_num + "," + rating + ",'" + text + "','" + date_posted + "'," + restaurantId + ",'" +comment+ "'," +employee_id+ ");", conn);

        NpgsqlDataReader reader = command.ExecuteReader();
    }
}
