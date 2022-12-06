using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentEntityController : ControllerBase
{

    private readonly ILogger<PaymentEntityController> _logger;

    public PaymentEntityController(ILogger<PaymentEntityController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "getUserPayment")]
    public List<Tuple<Boolean, String, String, String, String, String, int>>  Get(int userId)
    {
        PaymentFunctions db = new PaymentFunctions(); 
        db.paymentDataFetch(); 
        return db.getPayment(userId); 
    }

    [HttpPost(Name = "updatePayment")]
    public void Put(String paymentMethod, String expDate, int CVV, int zipCode, int cardNum, int userId)
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO payment_info_entity VALUES (DEFAULT,'" + paymentMethod + "','" + expDate + "'," + CVV + "," + zipCode + "," + cardNum + "," + userId + ");", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }
}
