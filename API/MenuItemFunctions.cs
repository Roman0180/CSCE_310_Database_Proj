using System.Collections;
using System.Runtime;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Text.Json;
using System.Globalization;
using System.Text.Json;

namespace API;

public class MenuItem{
    public String itemName; 
    public int menuNum; 
    public String itemSummary; 
    public Double itemPrice; 

}
public class MenuItemFunctions
{
    // menuNum -> List containing MenuItem data for each item
    Dictionary<int, List<MenuItem>> menuItemTable = new Dictionary<int, List<MenuItem>>();

    public void MenuItemDataFetch()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from menu_item_entity", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            MenuItem item = new MenuItem(); 
            item.itemName = reader.GetString(1); 
            item.menuNum = reader.GetInt32(2); 
            item.itemSummary = reader.GetString(3); 
            item.itemPrice = reader.GetDouble(4); 
            if(menuItemTable.ContainsKey(item.menuNum)){
                menuItemTable[item.menuNum].Add(item);
            } 
            else{
                List<MenuItem> newList = new List<MenuItem>(); 
                newList.Add(item); 
                menuItemTable.Add(item.menuNum, newList);
            }
        }
    }
    public List<Tuple<String, int, String, Double>> getMenuItemsFromMenu(int menuNum){
        List<Tuple<String, int, String, Double>> menuItemsForRestaurant = new List<Tuple<String, int, String, Double>>(); 

        if (menuItemTable.ContainsKey(menuNum)){
            foreach(var menuItem in menuItemTable[menuNum]){
                Tuple<String, int, String, Double> itemContent = new Tuple<String, int, String, Double>(menuItem.itemName, menuItem.menuNum, menuItem.itemSummary, menuItem.itemPrice); 
                menuItemsForRestaurant.Add(itemContent); 
            }
            return menuItemsForRestaurant; 
        }
        else{
            return null; 
        }
    }
    
    
}

