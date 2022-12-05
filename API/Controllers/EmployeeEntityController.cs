using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Cors;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeEntityController : ControllerBase
{

    private readonly ILogger<EmployeeEntityController> _logger;

    public EmployeeEntityController(ILogger<EmployeeEntityController> logger)
    {
        _logger = logger;
    }

    //[EnableCors(origins: "http://127.0.0.1:5500", headers: "GET", methods: "GET")]
    [HttpGet("getEmployeeByUserId")]
    public Tuple<Boolean, int, int, int, int> Get(int userId)
    {
        EmployeeFunctions db = new EmployeeFunctions(); 
        db.employeeDataFetch(); 
        return db.validateEmployeeWithUserId(userId); 
    }

    [HttpGet("getEmployeeByEmployeeId")]
    public Tuple<Boolean, int, int, int, int> Get(int employeeId, Boolean flag) //flag is ignored
    {
        EmployeeFunctions db = new EmployeeFunctions(); 
        db.employeeDataFetch(); 
        return db.validateEmployeeWithEmpId(employeeId); 
    }

    [HttpPut("registerEmployee")]
    public void Put(int userId, int restaurantId, int adminFlag)
    {
        EmployeeFunctions db = new EmployeeFunctions(); 
        db.updateEmployees(userId, restaurantId, adminFlag); 
    }
    
}
