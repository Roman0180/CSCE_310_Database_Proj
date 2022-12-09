using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewEntityEmployeeController : ControllerBase
{

    private readonly ILogger<ReviewEntityEmployeeController> _logger;


    // [HttpGet(Name = "checkRestaurantReviews")]
    // public List<Tuple<int,string,int,string,DateTime,string,int>> Get(int restaurantId)
    // {
    //     ReviewEmployeeFunctions db = new ReviewEmployeeFunctions(); 
    //     db.reviewDataFetch(); 
    //     return db.getReviews(restaurantId); 
    // }

    [HttpPost(Name = "updateComment")]
    public void Put(int comment_id, string comment,int employee_id)
    {
        // var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        // using var conn = new NpgsqlConnection(cs);
        // conn.Open();
        // NpgsqlCommand command = new NpgsqlCommand("UPDATE review_entity SET comment = '"+comment+"'WHERE comment_id= "+comment_id+";", conn);
        // NpgsqlDataReader reader = command.ExecuteReader();
        // NpgsqlCommand command2 = new NpgsqlCommand("UPDATE review_entity SET employee_id = "+employee_id+"WHERE comment_id= "+comment_id+";", conn);
        // NpgsqlDataReader reader2 = command2.ExecuteReader();
        ReviewEmployeeFunctions db = new ReviewEmployeeFunctions(); 
        db.updateComment(comment_id,comment);
        db.updateCommentEmployee(comment_id,employee_id);
       
    }
    // [HttpPut(Name = "updateEmployeeId")]
    // public void Put(int comment_id, int employee_id)
    // {
    //     var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
    //     using var conn = new NpgsqlConnection(cs);
    //     conn.Open();
    //     NpgsqlCommand command = new NpgsqlCommand("UPDATE review_entity SET employee_id = "+employee_id+"WHERE comment_id= "+comment_id+";", conn);
    //     NpgsqlDataReader reader = command.ExecuteReader();
        
        
    // }
}
