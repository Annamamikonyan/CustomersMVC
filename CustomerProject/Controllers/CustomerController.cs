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
        private static string _bucketName = "qwaszx111";
        private static string _folderName = "ProjectImages";
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
            await Common.Helpers.AwsAdapter.AwsS3FileDownload(_bucketName, _folderName, customerList.First().ImagePath);

            ViewBag.CustomerList = result;
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> MasterDetailsAjaxHandler()      
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

            await Common.Helpers.AwsAdapter.AwsS3FileDownload(_bucketName, _folderName, customerList.First().ImagePath );

            return Json(new
            {               
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public  async Task<ViewResult> Customers(Models.Customer newCustomer, HttpPostedFileBase avatar)
        {
            try
            {
                Data.Customers.Customer cust = new Data.Customers.Customer()
                {
                    CustomerName = newCustomer.CustomerName,
                    FirstName = newCustomer.FirstName,
                    Age = newCustomer.Age,
                    IsActive = newCustomer.IsActive,
                };
                if (avatar != null)
                {
                    cust.ImagePath = avatar.FileName;
                }
                // Add customer to database
                Data.Customers.CutomerLibrary.AddCustomer(cust);

                //Add customer avatar to AWS S3 storage
                if (avatar != null)
                {
                    var response = Common.Helpers.AwsAdapter.AwsS3FileUpdload(_bucketName, _folderName, avatar);
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
                        //avatar =  Common.Helpers.AwsAdapter.AwsS3FileDownload(_bucketName, _folderName)
                    };

                    result.Add(customer);
                }

                
                ViewBag.CustomerList = result;
                newCustomer = new CustomerProject.Models.Customer();
                return View(newCustomer);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return View("Error");
            }
                         

        }


    }
}


//String folderPath = HostingEnvironment.MapPath("~/App_Data/Images") ;
//Directory.CreateDirectory(folderPath);
// string avatarPath = Path.Combine(folderPath, avatar.FileName);
//if (!System.IO.File.Exists(avatarPath))
//{
//   avatar.SaveAs(avatarPath);
//}