using System.Collections;
using System.Runtime;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Text.Json;
using System.Globalization;
using System.Text.Json;

namespace API;

public class Reservation{
    public int partySize; 
    public DateTime reservationTime; 
    public int restaurantId; 
    public int customerId; 
}
public class ReservationFunctions
{
    // restaurant of reservation -> all reservations at that restaurant
    Dictionary<int, List<Reservation>> reservationTable = new Dictionary<int, List<Reservation>>();

    public void reservationDataFetch()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from reservation_entity", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            Reservation reservation = new Reservation(); 
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


    
}

