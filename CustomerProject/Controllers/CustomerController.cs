using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

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
        public  async Task<ViewResult> Customers(Models.Customer newCustomer, HttpPostedFileBase avatar)
        {
            Data.Customers.Customer cust = new Data.Customers.Customer() 
            { };



            String folderPath = HostingEnvironment.MapPath("~/App_Data/Images") ;
            Directory.CreateDirectory(folderPath);

            string avatarPath = Path.Combine(folderPath, avatar.FileName);
            Data.Customers.CutomerLibrary.AddCustomer(cust);

            if (!System.IO.File.Exists(avatarPath))
            {
               avatar.SaveAs(avatarPath);
            }

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