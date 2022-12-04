using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class MenuEntityController : ControllerBase
{

    private readonly ILogger<MenuEntityController> _logger;

    public MenuEntityController(ILogger<MenuEntityController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "getMenu")]
    public List<Tuple<String, int, String>> Get(int restaurantId)
    {
        MenuFunctions db = new MenuFunctions(); 
        db.MenuDataFetch(); 
        return db.getMenuForRestaurant(restaurantId); 
    }

}
