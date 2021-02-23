using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CustomerProject.Controllers
{
    public class CustomerController : Controller
    {
        public async Task<ViewResult> Customers()

        {
            //var result1 = Data.Customers.CustomerWIthSqlDataAdapter.GetCustomersFromDBAsync1();
            var result = new List<CustomerProject.Models.Customer>();
            var customerList = await Data.Customers.CutomerLibrary.GetCustomersFromDBAsync();
            foreach (var item in customerList)
            {
                Models.Customer customer = new Models.Customer
                {
                    FirstName = item.FirstName,
                    ID = item.ID,
                    CustomerName = item.CustomerName,
                    Age = item.Age,
                    ImagePath = item.ImagePath,
                    IsActive = item.IsActive
                };

                result.Add(customer);
            }

            ViewBag.CustomerList = result;
            return View();
        }


        [HttpPost]
        public  async Task<ViewResult> Customers(Models.Customer newCustomer)
        {
            Data.Customers.Customer cust = new Data.Customers.Customer();
            Data.Customers.CutomerLibrary.AddCustomer(cust);

            var result = new List<CustomerProject.Models.Customer>();
            var customerList = await Data.Customers.CutomerLibrary.GetCustomersFromDBAsync();
            foreach (var item in customerList)
            {
                Models.Customer customer = new Models.Customer
                {
                    FirstName = item.FirstName,
                    ID = item.ID,
                    CustomerName = item.CustomerName,
                    Age = item.Age,
                    ImagePath = item.ImagePath,
                    IsActive = item.IsActive
                };

                result.Add(customer);
            }

            ViewBag.CustomerList = result;
            return View();
        }


    }
}