using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Data.Customers
{

    public class CutomerLibrary
    {
        private static string _connectionString = $"Data Source=(local)\\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True";
        public static void AddCustomer(Customer customer)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = _connectionString;
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "insert into CUSTOMERS (CustomerName, FirstName, Age, ImagePath, Active) " +
                        "Values (@CustomerName, @FirstName, @Age, @ImagePath, @Active)";
                    command.Parameters.Add("@CustomerName", System.Data.SqlDbType.NVarChar, 100).Value = customer.CustomerName;
                    command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 50).Value = customer.FirstName;
                    command.Parameters.Add("@Age", System.Data.SqlDbType.TinyInt).Value = customer.Age;
                    if (customer.ImagePath == null)
                    {
                        command.Parameters.Add("@ImagePath", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@ImagePath", System.Data.SqlDbType.NVarChar).Value = customer.ImagePath;
                    }

                    command.Parameters.Add("@Active", System.Data.SqlDbType.Bit).Value = customer.IsActive;

                    command.Connection = con;
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {

                throw e;
            }


        }

        public static void EditCustomer(Customer customer)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = _connectionString;
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    command.CommandType = System.Data.CommandType.Text;
                    command.Connection = con;
                    command.Parameters.Add("@ID", System.Data.SqlDbType.Int ).Value = customer.ID ;

                    if (!CheckCustomerIDExistance(customer.ID, command))
                    {
                        throw new Exception("A Customer with ID does not exist");
                    }

                    command.Parameters.Add("@CustomerName", System.Data.SqlDbType.NVarChar, 100).Value = customer.CustomerName;
                    command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 50).Value = customer.FirstName;
                    command.Parameters.Add("@Age", System.Data.SqlDbType.TinyInt).Value = customer.Age;
                    command.CommandText = "Update CUSTOMERS set CustomerName = @CustomerName,  FirstName = @FirstName, Age = @Age  " +
                        " Where ID = @ID";

                    //if (customer.ImagePath == null)
                    //{
                    //    command.Parameters.Add("ImagePath", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;
                    //}
                    //else
                    //{
                    //    command.Parameters.Add("ImagePath", System.Data.SqlDbType.NVarChar).Value = customer.ImagePath;
                    //}

                    //command.Parameters.Add("@Active", System.Data.SqlDbType.Bit).Value = customer.IsActive;

                    command.Connection = con;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception  e)
            {
                throw e;
            }
        }

        public static void DeleteCustomer(int ID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = _connectionString;
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    command.CommandType = System.Data.CommandType.Text;
                    command.Connection = con;
                    command.Parameters.Add("@ID", System.Data.SqlDbType.Int ).Value = ID;
                    if (!CheckCustomerIDExistance(ID, command))
                    {
                        throw new Exception("A Customer with specified ID does not exist");
                    }
                    command.CommandText = "Delete from CUSTOMERS Where ID = @ID ";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool CheckCustomerIDExistance(int ID,SqlCommand command)
        {
            command.CommandText = "Select ID from CUSTOMERS Where ID = @ID";
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                    return true;
                else
                    return false;
            }

        }
        public static async Task<List<Customer>> GetCustomersFromDBAsync()
        {
            var result = new List<Customer>();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = _connectionString;
                    await con.OpenAsync();
                    SqlCommand command = new SqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "sp_getCustomers";
                    command.Connection = con;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                // Select ID, CustomerName, FirstName , Age, ImagePath, Active
                                Customer customer = new Customer();
                                customer.FirstName = reader.GetString(2);
                                customer.ID = reader.GetInt32(0);
                                customer.CustomerName = reader.GetString(1);
                                customer.Age = reader.GetByte(3);
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
            }
            catch (Exception e)
            {
                throw e; ;
            }
            return result;
        }
    }
}
