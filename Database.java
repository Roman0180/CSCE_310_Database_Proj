import java.io.*;
import java.sql.*;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.Hashtable;
import java.util.List;
import java.util.Map;


public class Database {

    public Map<String, List<Object>> getData(String tableName){
        Connection conn = null;
        Map<String, List<Object>> table_data = new HashMap<String, List<Object>>(); 
        //String dbConnectionString = "jdbc:postgresql://csce-315-db.engr.tamu.edu/" + dbName;
        String userName = "csce310_gasiorowski";
        String userPassword = "229001014";
        String dbConnectionString = "jdbc:postgresql://csce-315-db.engr.tamu.edu/"  + "csce310_db"; 
        System.out.println(tableName);
        // Connecting to the database
        try {
            conn = DriverManager.getConnection(dbConnectionString, userName, userPassword);
        } catch (Exception e) {
            e.printStackTrace();
            System.err.println(e.getClass().getName() + ": " + e.getMessage());
            System.exit(0);
        }
        System.out.println("Opened database successfully");
        try {
            Statement stmt = conn.createStatement();
            String sqlStatement = "select * from " + tableName + ";";
            ResultSet rs = stmt.executeQuery(sqlStatement);
            
            

            ResultSetMetaData metaData = rs.getMetaData();
            Integer columnCount = metaData.getColumnCount();
            while (rs.next()) {
  
                List<Object> row_data = new ArrayList<Object>(); 
                
                for(int i = 2; i <= columnCount; i++){
                    row_data.add(rs.getObject(i)); 
                    System.out.println(rs.getObject(i)); 
                }
                table_data.put(rs.getString(1), row_data);

            }
        }
        catch (Exception e) {
            e.printStackTrace();
            System.err.println(e.getClass().getName() + ": " + e.getMessage());
            System.exit(0);
        }
        // closing the connection
        try {
            conn.close();
            System.out.println("Connection Closed.");
        } catch (Exception e) {
            System.out.println("Connection NOT Closed.");
        }
        for(List<Object> list : table_data.values()){
            for(Object var : list){
                System.out.println(var);
            }
        }
        return table_data; 
    }
}