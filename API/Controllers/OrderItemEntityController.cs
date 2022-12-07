using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderItemEntityUserController : ControllerBase
{

    private readonly ILogger<OrderItemEntityUserController> _logger;


    [HttpGet(Name = "checkOrderItems")]
    public List<Tuple<int, double, int, int>> Get(int order_num)
    {
        OrderItemFunctions db = new OrderItemFunctions(); 
        db.orderItemDataFetch(); 
        return db.getOrderItems(order_num); 
    }

    [HttpPut(Name = "createOrderItem")]
    public void Put(int order_num, int menu_item_number, int quantity)
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO order_items_entity VALUES (DEFAULT," + menu_item_number + ", (SELECT item_price FROM menu_item_entity WHERE item_id= "+menu_item_number+")*"+quantity+", "+ quantity +","+ order_num + ");", conn);
        //INSERT INTO order_items_entity VALUES (DEFAULT, 64, (SELECT item_price FROM menu_item_entity WHERE item_id=64)*1, 1, 1);
        NpgsqlDataReader reader = command.ExecuteReader();
    }
}
