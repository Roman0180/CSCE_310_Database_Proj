using System.Collections;
using System.Runtime;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Text.Json;
using System.Globalization;
using System.Text.Json;

namespace API;

public class Restaurant{
    public int restaurantId; 
    public String restaurantName; 
    public String location; 
    public String operatingHours; 
    public String description; 
}
public class RestaurantFunctions
{
    // restaurantId -> restaurant data
    Dictionary<int, Restaurant> restaurantTable = new Dictionary<int, Restaurant>();

    internal void restaurantDataFetch()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from restaurant_entity", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            Restaurant restaurant = new Restaurant(); 
            restaurant.restaurantId = reader.GetInt32(0); 
            restaurant.restaurantName = reader.GetString(1); 
            restaurant.location = reader.GetString(2); 
            restaurant.operatingHours = reader.GetString(3); 
            restaurant.description = reader.GetString(4); 
            restaurantTable.Add(restaurant.restaurantId, restaurant); 
        }
    }
    internal Tuple<Boolean, int, String, String, String, String> getRestaurant(int restaurantId){
        
        if (restaurantTable.ContainsKey(restaurantId)){
            Tuple<Boolean, int, String, String, String, String>  foundTuple = new Tuple<Boolean, int, String, String, String, String> (true, restaurantTable[restaurantId].restaurantId, restaurantTable[restaurantId].restaurantName, restaurantTable[restaurantId].location, restaurantTable[restaurantId].operatingHours, restaurantTable[restaurantId].description); 
            return foundTuple; 
        }
        Tuple<Boolean, int, String, String, String, String>  notFound = new Tuple<Boolean, int, String, String, String, String> (false, 0, null, null, null, null); 
        return notFound; 
    }
    

 
}

