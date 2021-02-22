using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Customers
{
    public class CustomerWIthSqlDataAdapter
    {
        private static string _connectionString = $"Data Source=JTPC-42\\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True";

        public static List<Customer> GetCustomersFromDBAsync1()
        {
            var result = new List<Customer>();
            var queryString = "Select * from CUSTOMERS";
            
            using (SqlConnection connection =new SqlConnection(_connectionString))
            {
                DataSet dataSet=new DataSet("CUSTOMERS"); 
                SqlDataAdapter adapter = new SqlDataAdapter();                
                adapter.SelectCommand = new SqlCommand(queryString, connection);
                adapter.Fill(dataSet);

                foreach (DataRow  row in dataSet.Tables[0].Rows)
                {
                    var cust = new Customer()
                    {
                        ID = (Int32)row[0],
                        CustomerName = (String)row[1]                                                             
                    };
                    result.Add(cust);
                    row[1] = (string)row[1] + "111";
                    dataSet.AcceptChanges();

                }
                dataSet.AcceptChanges();
                adapter.Update(dataSet.Tables[0]);
            }
            return result;
        }
    }
}
