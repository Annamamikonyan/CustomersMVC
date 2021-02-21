using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Data.Customers
{ 
    public class Customer
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string FirstName { get; set; }
        public short  Age { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
    }
public class CutomerLibrary
    {
        private static string _connectionString = $"Data Source=DESKTOP-00Q2C43\\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True";
        public static async  Task<List<Customer>> GetCustomersFromDBAsync()
        {
            var result = new List<Customer>();

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = _connectionString;
                await con.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure ;
                command.CommandText = "sp_getCustomers";
                command.Connection = con;
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            //Select ID, CustomerName, FirstName , Age, ImagePath, Active
                            Customer customer = new Customer();
                            customer.FirstName = reader.GetString (2);
                            customer.ID = reader.GetInt32(0);
                            customer.CustomerName = reader.GetString(1);
                            customer.Age =reader.GetByte(3);
                            if (!reader.IsDBNull(4))
                            {
                                customer.ImagePath = reader.GetString(4);
                            }
                            else
                            {
                                customer.ImagePath = "Has no image";
                            }
                            
                            customer.IsActive = reader.GetBoolean(5);
                        
                            result.Add(customer);
                        }                    
                    }
                }
            }

            return result;
        }
    }
}
