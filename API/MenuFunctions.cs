using System.Collections;
using System.Runtime;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Text.Json;
using System.Globalization;
using System.Text.Json;

namespace API;

public class Menu{
    public String menuTitle; 
    public int restaurantId; 
    public String menuDescription; 

}
public class MenuFunctions
{
    // restaurantId -> List containing Menu data for each item
    Dictionary<int, List<Menu>> menuTable = new Dictionary<int, List<Menu>>();

    public void MenuDataFetch()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from menu_entity", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            Menu item = new Menu(); 
            item.menuTitle = reader.GetString(1); 
            item.restaurantId = reader.GetInt32(2); 
            item.menuDescription = reader.GetString(3); 
            if(menuTable.ContainsKey(item.restaurantId)){
                menuTable[item.restaurantId].Add(item);
            } 
            else{
                List<Menu> newList = new List<Menu>(); 
                newList.Add(item); 
                menuTable.Add(item.restaurantId, newList);
            }
        }
    }
    public List<Tuple<String, int, String>> getMenuForRestaurant(int restaurantId){
        List<Tuple<String, int, String>> menuItemsForRestaurant = new List<Tuple<String, int, String>>(); 

        if (menuTable.ContainsKey(restaurantId)){
            foreach(var menu in menuTable[restaurantId]){
                Tuple<String, int, String> itemContent = new Tuple<String, int, String>(menu.menuTitle, menu.restaurantId, menu.menuDescription); 
                menuItemsForRestaurant.Add(itemContent); 
            }
            return menuItemsForRestaurant; 
        }
        else{
            return null; 
        }
    }
    
    
}

