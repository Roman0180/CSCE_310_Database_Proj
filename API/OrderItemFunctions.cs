using System.Collections;
using System.Runtime;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Text.Json;
using System.Globalization;
using System.Text.Json;

namespace API;

public class OrderItem{
   public int menu_item_number;
   public double item_price;
   public int quantity;
   public int order_num;
}

public class OrderItemFunctions
{
    // restaurant of review -> all reviews at that restaurant
    Dictionary<int, List<OrderItem>> orderItem_table = new Dictionary<int, List<OrderItem>>();

    public void orderItemDataFetch()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from order_items_entity", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            OrderItem orderItem = new OrderItem(); 
            orderItem.menu_item_number = reader.GetInt32(1);
            orderItem.item_price = reader.GetDouble(2);
            orderItem.quantity = reader.GetInt32(3);
            orderItem.order_num = reader.GetInt32(4);
            //review.comment_customer_id = reader.GetInt32(8);
            if(orderItem_table.ContainsKey(orderItem.order_num)){
                orderItem_table[orderItem.order_num].Add(orderItem);
            } 
            else{
                List<OrderItem> newList = new List<OrderItem>(); 
                newList.Add(orderItem); 
                orderItem_table.Add(orderItem.order_num, newList);
            }
        }
    }
    public List<Tuple<int, double, int, int>> getOrderItems(int order_num){
        List<Tuple<int, double, int, int>> orderItemData= new List<Tuple<int, double, int, int>>(); 

        if (orderItem_table.ContainsKey(order_num)){
            foreach(var orderItem in orderItem_table[order_num]){
                Tuple<int, double, int, int> r = new Tuple<int, double, int, int>(orderItem.menu_item_number,orderItem.item_price,orderItem.quantity,orderItem.order_num); 
                orderItemData.Add(r); 
            }
            return orderItemData; 
        }
        else{
            return null; 
        }
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


    
}

