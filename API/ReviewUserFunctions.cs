using System.Collections;
using System.Runtime;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Text.Json;
using System.Globalization;
using System.Text.Json;

namespace API;

public class ReviewWithCustomerID{
    public int commentId;
    public int order_num;
    public int customerId;
    public string firstName;
    public string lastName; 
    public string firstLast;
    public int rating;
    public string text_feedback;
    public DateTime date_posted; 
    public int restaurantId; 
    public string comment;
    public int employee_id;
}

public class ReviewUserFunctions
{
    // restaurant of review -> all reviews at that restaurant
    Dictionary<int, List<ReviewWithCustomerID>> review_table = new Dictionary<int, List<ReviewWithCustomerID>>();
    Dictionary<int, List<ReviewWithCustomerID>> review_table_customer = new Dictionary<int, List<ReviewWithCustomerID>>();
    public void reviewDataFetch()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from reviewWithNames", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            ReviewWithCustomerID review = new ReviewWithCustomerID(); 
            review.commentId = reader.GetInt32(0);
            review.firstName = reader.GetString(1);
            review.lastName = reader.GetString(2);
            review.rating = reader.GetInt32(3);
            review.text_feedback = reader.GetString(4);
            review.date_posted = reader.GetDateTime(5);
            review.comment = reader.GetString(6);
            review.restaurantId = reader.GetInt32(7);
            review.employee_id = reader.GetInt32(8);
            review.firstLast = review.firstName + " " + review.lastName;
            if(review_table.ContainsKey(review.restaurantId)){
                review_table[review.restaurantId].Add(review);
            } 
            else{
                List<ReviewWithCustomerID> newList = new List<ReviewWithCustomerID>(); 
                newList.Add(review); 
                review_table.Add(review.restaurantId, newList);
            }
        }
    }


    public void reviewDataFetchVariant()
        {
            var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
            using var conn = new NpgsqlConnection(cs);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * from reviewWithNamesAndCustomerId", conn);
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read()){
                ReviewWithCustomerID review = new ReviewWithCustomerID(); 
                review.commentId = reader.GetInt32(0);
                review.customerId = reader.GetInt32(1);
                review.order_num = reader.GetInt32(2);
                review.firstName = reader.GetString(3);
                review.lastName = reader.GetString(4);
                review.rating = reader.GetInt32(5);
                review.text_feedback = reader.GetString(6);
                review.date_posted = reader.GetDateTime(7);
                review.comment = reader.GetString(8);
                review.restaurantId = reader.GetInt32(9);
                review.employee_id = reader.GetInt32(10);
                review.firstLast = review.firstName + " " + review.lastName;
                if(review_table_customer.ContainsKey(review.customerId)){
                    review_table_customer[review.customerId].Add(review);
                } 
                else{
                    List<ReviewWithCustomerID> newList = new List<ReviewWithCustomerID>(); 
                    newList.Add(review); 
                    review_table_customer.Add(review.customerId, newList);
                }
            }
        }


    public List<Tuple<int,string,int,string,DateTime,string,int>> getReviews(int restaurantId){
        List<Tuple<int,string,int,string,DateTime,string,int>> reviewData= new List<Tuple<int,string,int,string,DateTime,string,int>>(); 

        if (review_table.ContainsKey(restaurantId)){
            foreach(var review in review_table[restaurantId]){
                Tuple<int,string,int,string,DateTime,string,int> r = new Tuple<int,string,int,string,DateTime,string,int>(review.commentId,review.firstLast,review.rating,review.text_feedback,review.date_posted,review.comment,review.employee_id); 
                reviewData.Add(r); 
            }
            return reviewData; 
        }
        else{
            return null; 
        }
    }

    public List<Tuple<int,int,int,string,DateTime,string,int>> getReviewsByCustomer(int customerId){
        List<Tuple<int,int,int,string,DateTime,string,int>> reviewData= new List<Tuple<int,int,int,string,DateTime,string,int>>(); 

        if (review_table_customer.ContainsKey(customerId)){
            foreach(var review in review_table_customer[customerId]){
                Tuple<int,int,int,string,DateTime,string,int> r = new Tuple<int,int,int,string,DateTime,string,int>(review.commentId,review.order_num,review.rating,review.text_feedback,review.date_posted,review.comment,review.customerId); 
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
    public List<Tuple<int, int, string, DateTime, int, string, int>> makeReview(int order_num, int rating, string text, DateTime date_posted, int restaurantId) {
        List<Tuple<int, int, string, DateTime, int, string, int>> reviewData = new List<Tuple<int, int, string, DateTime, int, string, int>>();

        if(review_table.ContainsKey(restaurantId)){
            foreach(var review in review_table[restaurantId]){
                Tuple<int, int, string, DateTime, int, string, int> r = new Tuple<int, int, string, DateTime, int, string, int>(order_num,rating,text,date_posted,restaurantId,"None",-1);
                reviewData.Add(r);
            }
            return reviewData;
        }
        else{
            return null;
        }
    }

    public void updateRating(int commentId, int rating){
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("UPDATE review_entity SET rating = "+rating+"WHERE comment_id= "+commentId+";", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }

    public void updateFeedback(int commentId, string feedback){
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("UPDATE review_entity SET text_feedback = '"+feedback+"' WHERE comment_id= "+commentId+";", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }

    public void updateTime(int commentId){
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        //UPDATE placed_order_entity SET order_date = (SELECT NOW()::TIMESTAMP) WHERE order_num =1;

        NpgsqlCommand command = new NpgsqlCommand("UPDATE review_entity SET date_posted = (SELECT NOW()::TIMESTAMP) WHERE comment_id = "+commentId+";", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }

    public void deleteComment(int commentId){
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        //UPDATE placed_order_entity SET order_date = (SELECT NOW()::TIMESTAMP) WHERE order_num =1;
        //DELETE FROM review_entity WHERE comment_id = "+commentId+";"
        NpgsqlCommand command = new NpgsqlCommand("DELETE FROM review_entity WHERE comment_id = "+commentId+";", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
    }


    
}

