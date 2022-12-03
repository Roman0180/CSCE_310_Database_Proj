using System.Collections;
using System.Runtime;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Text.Json;
using System.Globalization;
using System.Text.Json;

namespace API;

public class UserObject{
    public String firstName; 
    public String lastName; 
    public String email; 
    public String password; 
    public Boolean userType; 
}
public class UserFunctions
{
    // (username, password) -> userData
    Dictionary<Tuple<String, String>, UserObject> user_table = new Dictionary<Tuple<String, String>, UserObject>();

    public void userDataFetch()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from user_entity", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            UserObject newUser = new UserObject(); 
            newUser.firstName = reader.GetString(1); 
            newUser.lastName = reader.GetString(2); 
            String userName = reader.GetString(3); 
            String password = reader.GetString(4); 
            newUser.email = userName; 
            newUser.password = password;
            Tuple<String, String> loginInfo = new Tuple<String, String>(userName, password); 
            user_table.Add(loginInfo, newUser);  
        }
    }
    public Tuple<Boolean, String, String, String, String, Boolean> validateUser(String userName, String password){
        Tuple<String, String> loginInfo = new Tuple<String, String>(userName, password); 
        
        if (user_table.ContainsKey(loginInfo)){
            Console.WriteLine(loginInfo); 
            Tuple<Boolean, String, String, String, String, Boolean> foundTuple = new Tuple<Boolean, String, String, String, String, Boolean>(true, user_table[loginInfo].firstName, user_table[loginInfo].lastName, user_table[loginInfo].email, user_table[loginInfo].password, user_table[loginInfo].userType); 
            return foundTuple; 
        }
        Tuple<Boolean, String, String, String, String, Boolean> notFound = new Tuple<Boolean, String, String, String, String, Boolean>(false, null, null, null, null, false); 
        return notFound; 
    }
    
    public void updateUsers(String firstName, String lastName, String email, String password){
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO user_entity VALUES (DEFAULT,'" + firstName + "','" + lastName + "','" + email + "','" + password + "', 0);", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }
}
