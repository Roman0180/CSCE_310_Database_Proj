using System;
using Microsoft.Data.SqlClient;
using System.Text;

namespace API
{
    public class DatabaseQuery
    {
        public void init()
        {
            try 
            { 
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ""; 

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");
                    Console.WriteLine("=========================================\n");

                    String sql = "SELECT * FROM sys.tables";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("starting");
                            while (reader.Read())
                            {
                                Console.WriteLine("executing commands");
                                Console.WriteLine("{0}", reader.GetString(0));
                            }
                            Console.WriteLine("done");
                        }
                    }                    
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }
    }
}