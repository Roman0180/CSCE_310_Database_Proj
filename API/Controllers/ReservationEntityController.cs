using System.Collections;
using Microsoft.AspNetCore.Mvc;

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

    // [HttpPut(Name = "registerUser")]
    // public void Put(String firstName, String lastName, String email, String password)
    // {
    //     ReservationFunctions db = new ReservationFunctions(); 
    //     db.updateUsers(firstName, lastName, email, password); 
    // }
}
