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
    public List<Tuple<int, DateTime, int, int>> getReservations(int restaurantId){
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

