using System.Collections;
using System.Runtime;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Text.Json;
using System.Globalization;
using System.Text.Json;

namespace API;

public class Payment{
    public String paymentMethod;
    public String expDate; 
    public String CVV;
    public String zipCode;
    public String cardNum;
    public int userId; 
}
public class PaymentFunctions
{
    // userId -> list of associated payment info
    Dictionary<int, List<Payment>> paymentTable = new Dictionary<int, List<Payment>>();

    internal void paymentDataFetch()
    {
        var cs = "Host=csce-315-db.engr.tamu.edu;Username=csce310_gasiorowski;Password=229001014;Database=csce310_db";
        using var conn = new NpgsqlConnection(cs);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand("SELECT * from payment_info_entity", conn);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            Payment newPayment = new Payment(); 
            newPayment.paymentMethod = reader.GetString(1);
            newPayment.expDate = reader.GetString(2);
            newPayment.CVV = reader.GetString(3);
            newPayment.zipCode = reader.GetString(4);
            newPayment.cardNum = reader.GetString(5);
            newPayment.userId = reader.GetInt32(6); 
            if(paymentTable.ContainsKey(newPayment.userId)){
                paymentTable[newPayment.userId].Add(newPayment);
            } 
            else{
                List<Payment> newList = new List<Payment>(); 
                newList.Add(newPayment); 
                paymentTable.Add(newPayment.userId, newList);
            }
        }
    }
    internal List<Tuple<Boolean, String, String, String, String, String, int>> getPayment(int userId)
    {
        if (paymentTable.ContainsKey(userId)){
            List<Tuple<Boolean, String, String, String, String, String, int>> newList = new List<Tuple<bool, String, String, String, String, String, int>>(); 
            foreach(Payment payment in paymentTable[userId]){
                Tuple<Boolean, String, String, String, String, String, int> foundTuple = new Tuple<Boolean, String, String, String, String, String, int>(true, payment.paymentMethod, payment.expDate, payment.CVV, payment.zipCode, payment.cardNum, payment.userId); 
                newList.Add(foundTuple); 
            }
            return newList;
        }
        List<Tuple<Boolean, String, String, String, String, String, int>> notFound = new List<Tuple<Boolean, String, String, String, String, String, int>>(); 
        Tuple<Boolean, String, String, String, String, String, int> data = new Tuple<Boolean, String, String, String, String, String, int>(false, null, null, null, null, null, 0);
        notFound.Add(data);
        return notFound; 
    }
    
}

