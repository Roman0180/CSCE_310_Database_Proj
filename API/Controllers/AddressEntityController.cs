using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Cors;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AddressEntity : ControllerBase
{

    private readonly ILogger<AddressEntity> _logger;

    public AddressEntity(ILogger<AddressEntity> logger)
    {
        _logger = logger;
    }

    //[EnableCors(origins: "http://127.0.0.1:5500", headers: "GET", methods: "GET")]
    [HttpGet("getAddress")]
    public Tuple<Boolean, int, int, int, int> Get(int userId)
    {
        EmployeeFunctions db = new EmployeeFunctions(); 
        db.employeeDataFetch(); 
        return db.validateEmployeeWithUserId(userId); 
    }


    [HttpPost("addAddress")]
    public void Put(int userId, int restaurantId, int adminFlag)
    {
        EmployeeFunctions db = new EmployeeFunctions(); 
        db.updateEmployees(userId, restaurantId, adminFlag); 
    }
    
}
