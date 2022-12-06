using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Cors;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEntityController : ControllerBase
{

    private readonly ILogger<UserEntityController> _logger;

    public UserEntityController(ILogger<UserEntityController> logger)
    {
        _logger = logger;
    }

    //[EnableCors(origins: "http://127.0.0.1:5500", headers: "GET", methods: "GET")]
    [HttpGet(Name = "isValidUser")]
    public Tuple<Boolean, int, String, String, String, String, Boolean> Get(String userName, String password)
    {
        UserFunctions db = new UserFunctions(); 
        db.userDataFetch(); 
        return db.validateUser(userName, password); 
    }

    [HttpPost(Name = "registerUser")]
    public void Put(String firstName, String lastName, String email, String password)
    {
        UserFunctions db = new UserFunctions(); 
        db.updateUsers(firstName, lastName, email, password); 
    }
}
