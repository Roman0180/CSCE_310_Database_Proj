using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class PlacedOrderEntityController : ControllerBase
{

    private readonly ILogger<PlacedOrderEntityController> _logger;


    [HttpGet(Name = "checkPlacedOrder")]
    public List<Tuple<int, int, DateTime, bool, DateTime, double>> Get(int order_num)
    {
        PlacedOrderFunctions db = new PlacedOrderFunctions(); 
        db.placedOrderDataFetch(); 
        return db.getPlacedOrder(order_num); 
    }

    [HttpPost(Name = "createPlacedOrder")]
    public void Post(int customer_id, float total)
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        //INSERT INTO placed_order_entity VALUES(DEFAULT, 2, NULL, NULL, NULL, NULL);
        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO placed_order_entity VALUES (DEFAULT," + customer_id + ",'2000/01/01',false,'2000/01/01'," + total + ");", conn);
        
        NpgsqlDataReader reader = command.ExecuteReader();
    }
    [HttpPut("updatePlacedOrder")]
    public void Put(int order_num,bool flag)
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        PlacedOrderFunctions db = new PlacedOrderFunctions();
        db.updateDeliveryFlag(order_num,flag); 
        db.updateOrderDate(order_num);
        db.updateReadyTime(order_num);
        //db.updateOrderTotal(order_num);
        //using var conn = new NpgsqlConnection(cs);
        //conn.Open();
        //INSERT INTO placed_order_entity VALUES(DEFAULT, 2, NULL, NULL, NULL, NULL);
        //NpgsqlCommand command = new NpgsqlCommand("INSERT INTO placed_order_entity VALUES (DEFAULT," + customer_id + ",'2000/01/01',false,'2000/01/01',-1.0 );", conn);
        
        //NpgsqlDataReader reader = command.ExecuteReader();
    }
    [HttpGet("grabLatestOrder")]
    public int Get()
    {
        PlacedOrderFunctions db = new PlacedOrderFunctions(); 
        int order_num = db.grabOrderNum();
        return order_num;
    }
}
