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

}
