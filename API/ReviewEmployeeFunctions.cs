using System.Collections;
using System.Runtime;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Text.Json;
using System.Globalization;
using System.Text.Json;

namespace API;

public class Review{
    public int order_num; 
    public int rating;
    public string text_feedback;
    public DateTime date_posted; 
    public int restaurantId; 
    public string comment;
    public int employee_id;
}

public class ReviewEmployeeFunctions
{
    // restaurant of review -> all reviews at that restaurant
    Dictionary<int, List<Review>> review_table = new Dictionary<int, List<Review>>();

    public void reviewDataFetch()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from review_entity", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            Review review = new Review(); 
            review.order_num = reader.GetInt32(1);
            review.rating = reader.GetInt32(2);
            review.text_feedback = reader.GetString(3);
            review.date_posted = reader.GetDateTime(4); 
            review.restaurantId = reader.GetInt32(5); 
            review.comment = reader.GetString(6);
            review.employee_id = reader.GetInt32(7);
            //review.comment_customer_id = reader.GetInt32(8);
            if(review_table.ContainsKey(review.restaurantId)){
                review_table[review.restaurantId].Add(review);
            } 
            else{
                List<Review> newList = new List<Review>(); 
                newList.Add(review); 
                review_table.Add(review.restaurantId, newList);
            }
        }
    }
    public List<Tuple<int, int, string, DateTime, int, string, int>> getReviews(int restaurantId){
        List<Tuple<int, int, string, DateTime, int, string, int>> reviewData= new List<Tuple<int, int, string, DateTime, int, string, int>>(); 

        if (review_table.ContainsKey(restaurantId)){
            foreach(var review in review_table[restaurantId]){
                Tuple<int, int, string, DateTime, int, string, int> r = new Tuple<int, int, string, DateTime, int, string, int>(review.order_num,review.rating, review.text_feedback, review.date_posted, review.restaurantId, review.comment, review.employee_id); 
                reviewData.Add(r); 
            }
            return reviewData; 
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
    // public List<Tuple<int, int, string, DateTime, int, string, int>> makeComment(int order_num, int rating, string text, DateTime date_posted, int restaurantId, string comment, int employee_id) {
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
    //}
    public void updateComment(int comment_id, string comment){
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("UPDATE review_entity SET comment = '"+comment+"'WHERE comment_id= "+comment_id+";", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }
    public void updateCommentEmployee(int comment_id, int employee_id){
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("UPDATE review_entity SET employee_id = "+employee_id+"WHERE comment_id= "+comment_id+";", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }

    
}

