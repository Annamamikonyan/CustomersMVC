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
        private static string _bucketURL = "https://qwaszx111.s3.eu-central-1.amazonaws.com/";
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
            return View(new Models.Customer());
        }

        [HttpGet]
        public async Task<ActionResult> MasterDetailsAjaxHandler()      
        {
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
                    ImagePath = _bucketURL + item.ImagePath,
                    IsActive = item.IsActive
                };

                result.Add(customer);
            }         

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
                Data.Customers.Customer dataCust = new Data.Customers.Customer()
                {
                    CustomerName = newCustomer.CustomerName,
                    FirstName = newCustomer.FirstName,
                    Age = newCustomer.Age,
                    IsActive = newCustomer.IsActive,
                };
                if (avatar != null)
                {
                    dataCust.ImagePath = avatar.FileName;
                }
                // Add customer to database
               

                //Add customer avatar to AWS S3 storage
                if (avatar != null)
                {
                    var response =await  Common.Helpers.AwsAdapter.AwsS3FileUpdload(_bucketName, _folderName, avatar);
                    dataCust.ImagePath = _folderName + "/" + avatar.FileName;
                }

                Data.Customers.CutomerLibrary.AddCustomer(dataCust);

                var result = new List<CustomerProject.Models.Customer>();
                var customerList = await Data.Customers.CutomerLibrary.GetCustomersFromDBAsync();
                foreach (var item in customerList)
                {
                    Models.Customer modelCustomer = new Models.Customer
                    {
                        FirstName = item.FirstName,
                        ID = item.ID,
                        CustomerName = item.CustomerName,
                        Age = item.Age,
                        ImagePath = _bucketURL + item.ImagePath,
                        IsActive = item.IsActive                        
                    };
                    //modelCustomer.ImageFullPath = await Common.Helpers.AwsAdapter.AwsS3FileDownload(_bucketName, _folderName, customerList.First().ImagePath);
                    result.Add(modelCustomer);
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

        [HttpPost]
        public async Task<ViewResult> EditCustomer(Models.Customer customer)
        {
            try
            {
                Data.Customers.Customer dataCust = new Data.Customers.Customer()
                {
                    ID = customer.ID ,
                    CustomerName = customer.CustomerName,
                    FirstName = customer.FirstName,
                    Age = customer.Age,
                    IsActive = customer.IsActive,
                };
                //if (avatar != null)
                //{
                //    dataCust.ImagePath = avatar.FileName;
                //}
                // Add customer to database


                //Add customer avatar to AWS S3 storage
                //if (avatar != null)
                //{
                //    var response = await Common.Helpers.AwsAdapter.AwsS3FileUpdload(_bucketName, _folderName, avatar);
                //    dataCust.ImagePath = _folderName + "/" + avatar.FileName;
                //}

                Data.Customers.CutomerLibrary.EditCustomer(dataCust);

                var result = new List<CustomerProject.Models.Customer>();
                var customerList = await Data.Customers.CutomerLibrary.GetCustomersFromDBAsync();
                foreach (var item in customerList)
                {
                    Models.Customer modelCustomer = new Models.Customer
                    {
                        FirstName = item.FirstName,
                        ID = item.ID,
                        CustomerName = item.CustomerName,
                        Age = item.Age,
                        ImagePath = _bucketURL + item.ImagePath,
                        IsActive = item.IsActive
                    };
                    //modelCustomer.ImageFullPath = await Common.Helpers.AwsAdapter.AwsS3FileDownload(_bucketName, _folderName, customerList.First().ImagePath);
                    result.Add(modelCustomer);
                }

                ViewBag.CustomerList = result;
                customer = new CustomerProject.Models.Customer();
                return View(customer );
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return View("Error");
            }
        }

        [HttpDelete]
        public async Task<ViewResult> DeleteCustomer(int id)
        {
            try
            {
                if (id!=0)
                { 
                    Data.Customers.CutomerLibrary.DeleteCustomer(id); 
                }
                
                var result = new List<CustomerProject.Models.Customer>();
                var customerList = await Data.Customers.CutomerLibrary.GetCustomersFromDBAsync();
                foreach (var item in customerList)
                {
                    Models.Customer modelCustomer = new Models.Customer
                    {
                        FirstName = item.FirstName,
                        ID = item.ID,
                        CustomerName = item.CustomerName,
                        Age = item.Age,
                        ImagePath = _bucketURL + item.ImagePath,
                        IsActive = item.IsActive
                    };
                    //modelCustomer.ImageFullPath = await Common.Helpers.AwsAdapter.AwsS3FileDownload(_bucketName, _folderName, customerList.First().ImagePath);
                    result.Add(modelCustomer);
                }

                ViewBag.CustomerList = result;
                var customer = new CustomerProject.Models.Customer();
                return View(customer);
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