using System.Collections;
using System.Runtime;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Text.Json;
using System.Globalization;
using System.Text.Json;

namespace API;

public class Reservation{
    public int reservationId; 
    public int partySize; 
    public DateTime reservationTime; 
    public int restaurantId; 
    public int customerId; 
}

public class ReservationWithNames
{
    public int reservation_id;
    public int reservation_party_size;
    public DateTime reservation_date_time;
    public int restaurant_id;
    public int customer_id;
    public string first_name;
    public string last_name;
    public string firstLast;
}
public class ReservationFunctions
{
    // restaurant of reservation -> all reservations at that restaurant
    Dictionary<int, List<Reservation>> reservationTable = new Dictionary<int, List<Reservation>>();
    Dictionary<int, List<ReservationWithNames>> reservationTableWithNames = new Dictionary<int, List<ReservationWithNames>>();

    public void reservationDataFetch()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from reservation_entity", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            Reservation reservation = new Reservation(); 
            reservation.reservationId = reader.GetInt32(0);
            reservation.partySize = reader.GetInt32(1); 
            reservation.reservationTime = reader.GetDateTime(2); 
            reservation.restaurantId = reader.GetInt32(3); 
            reservation.customerId = reader.GetInt32(4); 
            if(reservationTable.ContainsKey(reservation.restaurantId)){
                reservationTable[reservation.restaurantId].Add(reservation);
            } 
            else{
                List<Reservation> newList = new List<Reservation>(); 
                newList.Add(reservation); 
                reservationTable.Add(reservation.restaurantId, newList);
            }
        }
    }

    public void reservationDataFetchVariant()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from reservationWithNames", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            ReservationWithNames reservation = new ReservationWithNames(); 
            reservation.reservation_id = reader.GetInt32(0);
            reservation.reservation_party_size = reader.GetInt32(1);
            reservation.reservation_date_time =reader.GetDateTime(2);
            reservation.restaurant_id = reader.GetInt32(3);
            reservation.customer_id = reader.GetInt32(4);
            reservation.first_name = reader.GetString(5);
            reservation.last_name = reader.GetString(6);
            reservation.firstLast = reservation.first_name + " " + reservation.last_name;
            if(reservationTableWithNames.ContainsKey(reservation.restaurant_id)){
                reservationTableWithNames[reservation.restaurant_id].Add(reservation);
            } 
            else{
                List<ReservationWithNames> newList = new List<ReservationWithNames>(); 
                newList.Add(reservation); 
                reservationTableWithNames.Add(reservation.restaurant_id, newList);
            }
        }
    }


    public List<Tuple<int,int,DateTime,int, string>> getReservationsWithNames(int restaurantId){
        List<Tuple<int,int,DateTime,int, string>> reservationData= new List<Tuple<int,int,DateTime,int, string>>(); 

        if (reservationTableWithNames.ContainsKey(restaurantId)){
            foreach(var reservation in reservationTableWithNames[restaurantId]){
                Tuple<int,int,DateTime,int, string> r = new Tuple<int,int,DateTime,int, string>(reservation.reservation_id, reservation.reservation_party_size, reservation.reservation_date_time, reservation.customer_id, reservation.firstLast); 
                reservationData.Add(r); 
            }
            return reservationData; 
        }
        else{
            return null; 
        }
    }


    public List<String> getReservations(int restaurantId, DateTime startDate){
        List<Tuple<int, DateTime, int, int>> reservationData= new List<Tuple<int, DateTime, int, int>>(); 
        HashSet<int> existing_reservation_times = new HashSet<int>(); 
        List<String> reservations = new List<String>(); 
        if (reservationTable.ContainsKey(restaurantId)){
            foreach(var reservation in reservationTable[restaurantId]){
                // Tuple<int, DateTime, int, int> r = new Tuple<int, DateTime, int, int>(reservation.partySize, reservation.reservationTime, reservation.restaurantId, reservation.customerId); 
                // reservationData.Add(r); 
                if (reservation.reservationTime.Year == startDate.Year && reservation.reservationTime.Month == startDate.Month && reservation.reservationTime.Day == startDate.Day){
                    existing_reservation_times.Add(reservation.reservationTime.Hour);
                }
            }
            for(int i = 9; i < 12; i++){
                if(! existing_reservation_times.Contains(i)){
                    reservations.Add(i.ToString() + ":00 am"); 
                }
            }
            if(! existing_reservation_times.Contains(12)){
                reservations.Add("12:00 pm"); 
            }
            for(int i = 13; i < 22; i++){
                if(! existing_reservation_times.Contains(i)){
                    reservations.Add((i - 12).ToString() + ":00 pm"); 
                }
            }
            return reservations; 
        }
        else{
            return null; 
        }
    }

    public List<Tuple<int, DateTime, int, int>> makeReservations(int reservationPartySize, DateTime reservationDateTime, int restaurantId, int reservationMaker){
        List<Tuple<int, DateTime, int, int>> reservationData= new List<Tuple<int, DateTime, int, int>>(); 

        if (reservationTable.ContainsKey(restaurantId)){
            foreach(var reservation in reservationTable[restaurantId]){
                Tuple<int, DateTime, int, int> r = new Tuple<int, DateTime, int, int>(reservation.partySize, reservation.reservationTime, reservation.restaurantId, reservation.customerId); 
                reservationData.Add(r); 
            }
            return reservationData; 
        }
        else{
            return null; 
        }
    }

    internal List<Tuple<int, DateTime, int, int>> getReservations(int userId)
    {
        List<Tuple<int, DateTime, int, int>> userReservations = new List<Tuple<int, DateTime, int, int>>(); 
        foreach(List<Reservation> listReservations in reservationTable.Values){
            foreach(Reservation reservation in listReservations){
                if(reservation.customerId == userId){
                    Tuple<int, DateTime, int, int> userDetails = new Tuple<int, DateTime, int, int>(reservation.partySize, reservation.reservationTime, reservation.restaurantId, reservation.reservationId); 
                    userReservations.Add(userDetails); 
                }
            }
            
        }
        return userReservations; 
    }
    public void updateReservation(int reservation_id, int partySize)
{
    var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("UPDATE reservation_entity SET reservation_party_size = "+partySize+"WHERE reservation_id= "+reservation_id+";", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
}

public void deleteReservation(int reservation_id){
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
<<<<<<< HEAD
        NpgsqlCommand command = new NpgsqlCommand("DELETE FROM reservation_entity WHERE reservation_id =" + reservation_id+ ";", conn);
=======
        NpgsqlCommand command = new NpgsqlCommand("DELETE FROM reservation_entity WHERE reservation_id = "+reservation_id+";", conn);
>>>>>>> e5c6c65173200012fae8ffdf97a79523b44c3be4
        NpgsqlDataReader reader = command.ExecuteReader();
    }
    public void changeReservationOwner(int reservation_id, int customer_id)
{
    var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("UPDATE reservation_entity SET customer_id = "+customer_id+"WHERE reservation_id= "+reservation_id+";", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
}
}




