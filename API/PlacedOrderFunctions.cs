using System.Collections;
using System.Runtime;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Text.Json;
using System.Globalization;
using System.Text.Json;

namespace API;

public class PlacedOrder{
   public int order_num;
   public int customer_id;
   public DateTime ready_time;
   public bool delivery_flag;
   public DateTime order_date;
   public double order_total;
}
public class PlacedOrderWithNames{
    public int order_num;
    public int customer_id;
    public string first_name;
    public string last_name;
    public int restaurant_id;
    public DateTime ready_time;
    public DateTime order_date;
    public double order_total;
    public string firstLast;
}

public class PlacedOrderFunctions
{
    // restaurant of review -> all reviews at that restaurant
    Dictionary<int, List<PlacedOrder>> placedOrder_table = new Dictionary<int, List<PlacedOrder>>();
    Dictionary<int, List<PlacedOrderWithNames>> placedOrderWithNames_table = new Dictionary<int, List<PlacedOrderWithNames>>();
    int current_order_num;
    public void placedOrderDataFetch()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from placed_order_entity", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            PlacedOrder placedOrder = new PlacedOrder(); 
            placedOrder.order_num = reader.GetInt32(0);
            placedOrder.customer_id = reader.GetInt32(1);
            placedOrder.ready_time = reader.GetDateTime(2); 
            placedOrder.delivery_flag = reader.GetBoolean(3);
            placedOrder.order_date = reader.GetDateTime(4); 
            placedOrder.order_total = reader.GetDouble(5);
            //review.comment_customer_id = reader.GetInt32(8);
            if(placedOrder_table.ContainsKey(placedOrder.order_num)){
                placedOrder_table[placedOrder.order_num].Add(placedOrder);
            } 
            else{
                List<PlacedOrder> newList = new List<PlacedOrder>(); 
                newList.Add(placedOrder); 
                placedOrder_table.Add(placedOrder.order_num, newList);
            }
        }
    }

    public void placedOrderDataFetchVariant()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from ordersWithNames2", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            PlacedOrderWithNames placedOrder = new PlacedOrderWithNames(); 
            placedOrder.order_num = reader.GetInt32(0);
            placedOrder.customer_id = reader.GetInt32(1);
            placedOrder.first_name = reader.GetString(2);
            placedOrder.last_name = reader.GetString(3);
            placedOrder.restaurant_id = reader.GetInt32(4);
            placedOrder.ready_time = reader.GetDateTime(5);
            placedOrder.order_date = reader.GetDateTime(6);
            placedOrder.order_total = reader.GetDouble(7);
            placedOrder.firstLast = placedOrder.first_name + " " + placedOrder.last_name;
            //review.comment_customer_id = reader.GetInt32(8);
            if(placedOrderWithNames_table.ContainsKey(placedOrder.restaurant_id)){
                placedOrderWithNames_table[placedOrder.restaurant_id].Add(placedOrder);
            } 
            else{
                List<PlacedOrderWithNames> newList = new List<PlacedOrderWithNames>(); 
                newList.Add(placedOrder); 
                placedOrderWithNames_table.Add(placedOrder.restaurant_id, newList);
            }
        }
    }

    public List<Tuple<int, int,string, DateTime, DateTime, double>> getPlacedOrderVariant(int restaurant_id){
        List<Tuple<int, int,string, DateTime, DateTime, double>> placedOrderData= new List<Tuple<int, int,string, DateTime, DateTime, double>>(); 

        if (placedOrderWithNames_table.ContainsKey(restaurant_id)){
            foreach(var placedOrder in placedOrderWithNames_table[restaurant_id]){
                Tuple<int, int,string, DateTime, DateTime, double> r = new Tuple<int, int,string, DateTime, DateTime, double>(placedOrder.order_num,placedOrder.customer_id,placedOrder.firstLast,placedOrder.ready_time,placedOrder.order_date,placedOrder.order_total); 
                placedOrderData.Add(r); 
            }
            return placedOrderData; 
        }
        else{
            return null; 
        }
    }



    public List<Tuple<int, int, DateTime, bool, DateTime, double>> getPlacedOrder(int order_num){
        List<Tuple<int, int, DateTime, bool, DateTime, double>> orderItemData= new List<Tuple<int, int, DateTime, bool, DateTime, double>>(); 

        if (placedOrder_table.ContainsKey(order_num)){
            foreach(var placedOrder in placedOrder_table[order_num]){
                Tuple<int, int, DateTime, bool, DateTime, double> r = new Tuple<int, int, DateTime, bool, DateTime, double>(placedOrder.order_num,placedOrder.customer_id,placedOrder.ready_time,placedOrder.delivery_flag,placedOrder.order_date,placedOrder.order_total); 
                orderItemData.Add(r); 
            }
            return orderItemData; 
        }
        else{
            return null; 
        }
    }
    public void updateReadyTime(int order_num){
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        /*
        UPDATE placed_order_entity SET order_total = (SELECT SUM(item_price) FROM order_items_entity WHERE order_num=2) WHERE order_num=2;
        UPDATE placed_order_entity SET order_date = (SELECT NOW()::TIMESTAMP) WHERE order_num =2;
        UPDATE placed_order_entity SET ready_time = (SELECT NOW()::TIMESTAMP + INTERVAL '45 minutes') WHERE order_num =2;
        UPDATE placed_order_entity SET delivery_flag = false WHERE order_num =2;
        */
        NpgsqlCommand command = new NpgsqlCommand("UPDATE placed_order_entity SET ready_time = (SELECT NOW()::TIMESTAMP + INTERVAL '45 minutes') WHERE order_num = "+order_num+";", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }
    public void updateOrderDate(int order_num){
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        /*
        UPDATE placed_order_entity SET order_total = (SELECT SUM(item_price) FROM order_items_entity WHERE order_num=2) WHERE order_num=2;
        UPDATE placed_order_entity SET order_date = (SELECT NOW()::TIMESTAMP) WHERE order_num =2;
        UPDATE placed_order_entity SET ready_time = (SELECT NOW()::TIMESTAMP + INTERVAL '45 minutes') WHERE order_num =2;
        UPDATE placed_order_entity SET delivery_flag = false WHERE order_num =2;
        */
        NpgsqlCommand command = new NpgsqlCommand("UPDATE placed_order_entity SET order_date = (SELECT NOW()::TIMESTAMP) WHERE order_num = "+order_num+";", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }
    public void updateOrderTotal(int order_num){
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        /*
        UPDATE placed_order_entity SET order_total = (SELECT SUM(item_price) FROM order_items_entity WHERE order_num=2) WHERE order_num=2;
        UPDATE placed_order_entity SET order_date = (SELECT NOW()::TIMESTAMP) WHERE order_num =2;
        UPDATE placed_order_entity SET ready_time = (SELECT NOW()::TIMESTAMP + INTERVAL '45 minutes') WHERE order_num =2;
        UPDATE placed_order_entity SET delivery_flag = false WHERE order_num =2;
        */
        NpgsqlCommand command = new NpgsqlCommand("UPDATE placed_order_entity SET order_total = (SELECT SUM(item_price) FROM order_items_entity WHERE order_num= "+order_num+") WHERE order_num="+order_num+";", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }
    public void updateDeliveryFlag(int order_num,bool flag){
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        /*
        UPDATE placed_order_entity SET order_total = (SELECT SUM(item_price) FROM order_items_entity WHERE order_num=2) WHERE order_num=2;
        UPDATE placed_order_entity SET order_date = (SELECT NOW()::TIMESTAMP) WHERE order_num =2;
        UPDATE placed_order_entity SET ready_time = (SELECT NOW()::TIMESTAMP + INTERVAL '45 minutes') WHERE order_num =2;
        UPDATE placed_order_entity SET delivery_flag = false WHERE order_num =2;
        */
        NpgsqlCommand command = new NpgsqlCommand("UPDATE placed_order_entity SET delivery_flag ="+flag+  " WHERE order_num ="+order_num+";", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }
    // public List<Tuple<int, DateTime, int, int>> makeReservations(int reservationPartySize, DateTime reservationDateTime, int restaurantId, int reservationMaker){
    //     List<Tuple<int, DateTime, int, int>> reservationData= new List<Tuple<int, DateTime, int, int>>(); 

    //     if (reservation_table.ContainsKey(restaurantId)){
    //         foreach(var reservation in reservation_table[restaurantId]){
    //             Tuple<int, DateTime, int, int> r = new Tuple<int, DateTime, int, int>(reservation.partySize, reservation.reservationTime, reservation.restaurantId, reservation.customerId); 
    //             reservationData.Add(r); 
    //         }
    //         return reservationData; 
    //     }
    //     else{
    //         return null; 
    //     }
    // }
    // public List<Tuple<int, int, string, DateTime, int, string, int>> makeReview(int order_num, int rating, string text, DateTime date_posted, int restaurantId, string comment, int employee_id) {
    //     List<Tuple<int, int, string, DateTime, int, string, int>> reviewData = new List<Tuple<int, int, string, DateTime, int, string, int>>();

    //     if(review_table.ContainsKey(restaurantId)){
    //         foreach(var review in review_table[restaurantId]){
    //             Tuple<int, int, string, DateTime, int, string, int> r = new Tuple<int, int, string, DateTime, int, string, int>(order_num,rating,text,date_posted,restaurantId,comment,employee_id);
    //             reviewData.Add(r);
    //         }
    //         return reviewData;
    //     }
    //     else{
    //         return null;
    //     }
    // }

    public int grabOrderNum(){
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        
        
        //SELECT MAX(order_num) FROM placed_order_entity;
        NpgsqlCommand command = new NpgsqlCommand("SELECT MAX(order_num) FROM placed_order_entity;", conn);
        //NpgsqlDataReader reader = command.ExecuteReader();
        NpgsqlDataReader reader = command.ExecuteReader();
        int order_num = 0;
        while (reader.Read()){
            order_num = reader.GetInt32(0);
        }
        return order_num;
    }
    
}

