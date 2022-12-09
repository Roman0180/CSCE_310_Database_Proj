using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Npgsql;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewEntityUserController : ControllerBase
{

    private readonly ILogger<ReviewEntityUserController> _logger;

    //[Microsoft.AspNetCore.Cors.EnableCors("AllowSpecificOrigin")]
    //[EnableCors(origins: "https://localhost:7091", headers: "GET", methods: "GET")]
   //[EnableCors("AllowSpecificOrigin")]
    [HttpGet("checkReviews")]
    public List<Tuple<int,string,int,string,DateTime,string,int>> Get(int restaurantId)
    {
        ReviewUserFunctions db = new ReviewUserFunctions(); 
        db.reviewDataFetch(); 
        return db.getReviews(restaurantId); 
    }

    [HttpGet("checkReviewsForUser")]
    public List<Tuple<int,int,int,string,DateTime,string,int>> Get(int customerId, int one)
    {
        ReviewUserFunctions db = new ReviewUserFunctions(); 
        db.reviewDataFetchVariant(); 
        return db.getReviewsByCustomer(customerId); 
    }

    [HttpPost(Name = "createReview")]
    public void Put(int order_num, int rating, string text, DateTime date_posted, int restaurantId)
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO review_entity VALUES (DEFAULT," + order_num + "," + rating + ",'" + text + "','" + date_posted + "'," + restaurantId + ",'None',-1"+ ");", conn);

        NpgsqlDataReader reader = command.ExecuteReader();
    }
    //[Access-Control-Allow-Methods: Put]
    //[EnableCors(origins: "https://localhost:7091", headers: "GET", methods: "GET")]
    [HttpPut(Name = "updateReview")]
    public void Put(int comment_id, int rating, string feedback)
    {
        ReviewUserFunctions db = new ReviewUserFunctions(); 
        db.updateFeedback(comment_id,feedback);
        db.updateRating(comment_id, rating);
        db.updateTime(comment_id);
    }
    [HttpDelete(Name = "delReview")]
    public void Delete(int comment_id)
    {
        ReviewUserFunctions db = new ReviewUserFunctions(); 
        db.deleteComment(comment_id);
    }
}
