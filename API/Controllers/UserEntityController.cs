using Microsoft.AspNetCore.Mvc;

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

    [HttpGet(Name = "isValidUser")]
    public Tuple<Boolean, String, String, String, String, Boolean> Get(String userName, String password)
    {
        UserFunctions db = new UserFunctions(); 
        db.userDataFetch(); 
        return db.validateUser(userName, password); 
    }

    [HttpPut(Name = "registerUser")]
    public void Put(String firstName, String lastName, String email, String password)
    {
        UserFunctions db = new UserFunctions(); 
        db.updateUsers(firstName, lastName, email, password); 
    }
}
