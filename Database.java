import java.io.*;
import java.sql.*;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Comparator;
import java.util.Hashtable;
import java.util.Map;


public class Database {

    public void getData(String tableName){
        Connection conn = null;

        //String dbConnectionString = "jdbc:postgresql://csce-315-db.engr.tamu.edu/" + dbName;
        String userName = "csce310_gasiorowski";
        String userPassword = "";
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
            ResultSet result = stmt.executeQuery(sqlStatement);
            while(result.next()){
                System.out.println(result.getString(2));  
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
    }

    public void writeData(String tableName, ArrayList<?> inputs, boolean isQuery, boolean executeStatement, boolean getMenuTable, boolean getInventoryTable, String startDate, String endDate) {
        Connection conn = null;
        String teamNumber = "16";
        String sectionNumber = "910";
        String dbName = "csce315" + sectionNumber + "_" + teamNumber + "db";
        String dbConnectionString = "jdbc:postgresql://csce-315-db.engr.tamu.edu/" + dbName;
        String userName = "csce315" + sectionNumber + "_" + teamNumber + "user";
        String userPassword = "password";

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
            String sqlStatement = "";
            // Insert the values into the appropriate table
            if (tableName == "MenuTable") {
                if(executeStatement){
                    sqlStatement = (String)inputs.get(0); 
                }
                else if(getMenuTable){
                    sqlStatement = "select * from menutable;"; 
                }
                // this branch is taken when we want to add data to the MenuTable
                else if (!isQuery) {
                    // inputs[0] = orderID (int), inputs[1] = itemName (text), inpus[2] = itemDes
                    // (text), inputs[3] = itemPrice (Float)
                    sqlStatement = "INSERT INTO " + tableName + " VALUES (" + inputs.get(0) + ",'" + inputs.get(1)
                            + "','"
                            + inputs.get(2) + "'," + inputs.get(3) + ");";
                    System.out.println(sqlStatement);
                }
                // this branch is taken when we want to query data from the MenuTable
                else {
                    sqlStatement = (String) inputs.get(0);
                    System.out.println(sqlStatement);
                }

            } else if (tableName == "InventoryTable") {
                if(executeStatement){
                    sqlStatement = (String)inputs.get(0); 
                }
                else if(getInventoryTable){
                    sqlStatement = "select * from inventorytable;"; 
                }
                else if (!isQuery && inputs != null) {
                    sqlStatement = "INSERT INTO " + tableName + " VALUES ('" + inputs.get(0) + "','" + inputs.get(1)
                            + "',"
                            + inputs.get(2) + "," + inputs.get(3) + "," + inputs.get(4) + ",'" + inputs.get(5) + "',"
                            + inputs.get(6) + "," + inputs.get(7) + ",'" + inputs.get(8) + "'," + inputs.get(10) + ",'"
                            + inputs.get(11) + "','" + inputs.get(9);
                    System.out.println(sqlStatement);
                }
                else if(!isQuery && inputs == null){
                    // sqlStatement = updateCommands.get(updateCommands.size() - 1); 
                    System.out.println(sqlStatement); 
                }
                else {
                    if (inputs == null) {
                        // the case when we are not printing the inventory, but we want to get specific
                        // quantities from the inventory
                        sqlStatement = "select individual_quantity  from inventorytable where individual_quantity != '0';";
                    }
                    else{
                        // this is for when we want to directly change data values in the inventory
                        sqlStatement = (String)inputs.get(0); 
                    }

                }

            } else if (tableName == "OrdersTable") {
                if (!isQuery) {
                    sqlStatement = "INSERT INTO " + tableName + " VALUES ('" + inputs.get(0) + "'," + inputs.get(1)
                            + ",'" // fix single quotes for inserting orders from the GUI
                            + inputs.get(2) + "',"
                            + inputs.get(3) + ");";
                    System.out.println(sqlStatement);
                } 
                else if(startDate == null && endDate == null){
                    sqlStatement = "select * from OrdersTable;";
                }
                else {
                        sqlStatement = "select * from orderstable where order_date between '" + startDate + "' and '" + endDate + "';";
                }

            } else if (tableName.equals("OrderContentTable")) {
                if (!isQuery) {
                    sqlStatement = "INSERT INTO " + tableName + " VALUES (" + inputs.get(0) + inputs.get(1) + ","
                            + inputs.get(2) + "," + inputs.get(3) + "," + inputs.get(4) + "," + inputs.get(5) + ","
                            + inputs.get(6) + "," + inputs.get(7) + "," + inputs.get(8) + "," + inputs.get(9) + ","
                            + inputs.get(10) + "," + inputs.get(11) + "," + inputs.get(12) + ");";
                } else {
                    // every item in inputs represents an [order number, order quantity] pair
                    // for each order number, query the order content and multiply by order quantity
                    sqlStatement = "select * from OrderContentTable where order_item = " + inputs.get(0) + ';';

                }

            } else if (tableName.equals("Ingredients")) {
                if (!isQuery) {
                    sqlStatement = "INSERT INTO " + tableName + " VALUES ('" + inputs.get(0) + "'," + inputs.get(1)
                            + "," + inputs.get(5) + ","
                            + inputs.get(6) + "," + inputs.get(7) + "," + inputs.get(16) + "," + inputs.get(17) + ","
                            + inputs.get(18) + ");";
                    System.out.println(sqlStatement);
                } else {
                    sqlStatement = "select * from Ingredients;";
                }

            }
            System.out.println("--------------------Query Results--------------------");
            if (isQuery) {
                ResultSet result = stmt.executeQuery(sqlStatement);
                if (tableName.equals("OrdersTable")) {
                    while (result.next()) {
                        ArrayList<Integer> orderValues = new ArrayList<>();
                        Integer orderID = result.getInt("order_number");
                        orderValues.add(orderID);
                        Integer orderQuant = result.getInt("order_quantity");
                        orderValues.add(orderQuant);
                        // orderData.add(orderValues);
                    }
                    
                } else if (tableName.equals("OrderContentTable")) {
                    while (result.next()) {
                        ArrayList<Integer> food_quantities = new ArrayList<>();
                        Integer chicken_fingers = (result.getInt("chicken_fingers")) * (Integer) inputs.get(1);
                        food_quantities.add(chicken_fingers);

                        Integer toast = (result.getInt("toast")) * (Integer) inputs.get(1);
                        food_quantities.add(toast);

                        Integer fries = (result.getInt("fries")) * (Integer) inputs.get(1);
                        food_quantities.add(fries);

                        Integer potato_sld = (result.getInt("potato_salad")) * (Integer) inputs.get(1);
                        food_quantities.add(potato_sld);

                        Integer sauce = (result.getInt("sauce")) * (Integer) inputs.get(1);
                        food_quantities.add(sauce);

                        Integer bacon_strip = (result.getInt("bacon_strip")) * (Integer) inputs.get(1);
                        food_quantities.add(bacon_strip);

                        Integer cheese = (result.getInt("cheese")) * (Integer) inputs.get(1);
                        food_quantities.add(cheese);

                        Integer bottle_dr = (result.getInt("bottle_drink")) * (Integer) inputs.get(1);
                        food_quantities.add(bottle_dr);
                    }
                } else if (tableName.equals("Ingredients")) {
                    // represents in the index in the order contents table
                    int index = 0;
                    while (result.next()) {
                    }

                    

                } else if (tableName.equals("InventoryTable")) {
                    if(getInventoryTable){
                        while(result.next()){
                            ArrayList<String> rowOfInventoryTable = new ArrayList<>(); 
                            rowOfInventoryTable.add(result.getString("individual_quantity")); 
                            rowOfInventoryTable.add(result.getString("fill_levels"));
                            rowOfInventoryTable.add(result.getString("item_name")); 
                        }
                    }
                    

                }
            } else {
                var result = stmt.executeUpdate(sqlStatement);
                
            }

        } catch (Exception e) {
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
    }

}