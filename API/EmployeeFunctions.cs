using System.Collections;
using System.Runtime;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Text.Json;
using System.Globalization;
using System.Text.Json;

namespace API;

public class Employee{
    public int employeeId; 
    public int userId; 
    public int restaurantId; 
    public int adminFlag; 
}

public class EmployeeWithNames{
    public int employeeId;
    public int customerId;
    public string firstName;
    public string lastName;
    public string firstLast;
    public int restaurantId;
    public int adminFlag;
}

public class EmployeeFunctions
{
    // userId -> employee information
    Dictionary<int, Employee> employeeTable = new Dictionary<int, Employee>();
    Dictionary<int, List<EmployeeWithNames>> employeeWithNamesTable = new Dictionary<int, List<EmployeeWithNames>>();
    internal void employeeDataFetch()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from restaurant_employee_entity", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            Employee newUser = new Employee(); 
            newUser.employeeId = reader.GetInt32(0); 
            newUser.userId = reader.GetInt32(1); 
            newUser.restaurantId = reader.GetInt32(2); 
            newUser.adminFlag = reader.GetInt32(3); 
            employeeTable.Add(newUser.userId, newUser);  
        }
    }

    public void employeeDataFetchVariant()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from employeeWithNames", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            EmployeeWithNames newUser = new EmployeeWithNames(); 
            newUser.employeeId = reader.GetInt32(0); 
            newUser.customerId = reader.GetInt32(1); 
            newUser.firstName = reader.GetString(2);
            newUser.lastName = reader.GetString(3);
            newUser.restaurantId = reader.GetInt32(4); 
            newUser.adminFlag = reader.GetInt32(5); 
            newUser.firstLast = newUser.firstName + " " + newUser.lastName;
            //employeeTable.Add(newUser.restaurantId, newUser);  
            if(employeeWithNamesTable.ContainsKey(newUser.restaurantId)){
                    employeeWithNamesTable[newUser.restaurantId].Add(newUser);
                } 
                else{
                    List<EmployeeWithNames> newList = new List<EmployeeWithNames>(); 
                    newList.Add(newUser); 
                    employeeWithNamesTable.Add(newUser.restaurantId, newList);
                }
        }
    }


    public List<Tuple<int,int,string,int>> getEmployees(int restaurantId){
        List<Tuple<int,int,string,int>> employeeData= new List<Tuple<int,int,string,int>>(); 

        if (employeeWithNamesTable.ContainsKey(restaurantId)){
            foreach(var employee in employeeWithNamesTable[restaurantId]){
                Tuple<int,int,string,int> r = new Tuple<int,int,string,int>(employee.employeeId,employee.customerId,employee.firstLast,employee.adminFlag); 
                employeeData.Add(r); 
            }
            return employeeData; 
        }
        else{
            return null; 
        }
    }



    internal Tuple<Boolean, int, int, int, int> validateEmployeeWithUserId(int userId){
        
        if (employeeTable.ContainsKey(userId)){
            Tuple<Boolean, int, int, int, int> foundTuple = new Tuple<Boolean, int, int, int, int>(true, employeeTable[userId].employeeId, employeeTable[userId].userId, employeeTable[userId].restaurantId, employeeTable[userId].adminFlag); 
            return foundTuple; 
        }
        Tuple<Boolean, int, int, int, int> notFound = new Tuple<Boolean, int, int, int, int>(false, 0, 0, 0, 0); 
        return notFound; 
    }
    
    internal void updateEmployees(int userId, int restaurantId, int adminFlag){
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO restaurant_employee_entity VALUES (DEFAULT," + userId + "," + restaurantId + "," + adminFlag + ");", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }

    internal Tuple<bool, int, int, int, int> validateEmployeeWithEmpId(int employeeId)
    {
        foreach (var employee in employeeTable.Values){
            if(employee.employeeId == employeeId){
                Tuple<bool, int, int, int, int> foundTuple = new Tuple<bool, int, int, int, int>(true, employee.employeeId, employee.userId, employee.restaurantId, employee.adminFlag); 
                return foundTuple; 
            }
            
        }
        Tuple<Boolean, int, int, int, int> notFound = new Tuple<Boolean, int, int, int, int>(false, 0, 0, 0, 0); 
        return notFound; 
    }
}

