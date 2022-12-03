using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class MenuItemEntityController : ControllerBase
{

    private readonly ILogger<MenuItemEntityController> _logger;

    public MenuItemEntityController(ILogger<MenuItemEntityController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "getMenuItems")]
    public List<Tuple<String, int, String, Double>> Get(int menuNum)
    {
        MenuItemFunctions db = new MenuItemFunctions(); 
        db.MenuItemDataFetch(); 
        return db.getMenuItemsFromMenu(menuNum); 
    }

    // [HttpPut(Name = "registerUser")]
    // public void Put(String firstName, String lastName, String email, String password)
    // {
    //     UserFunctions db = new UserFunctions(); 
    //     db.updateUsers(firstName, lastName, email, password); 
    // }
}
