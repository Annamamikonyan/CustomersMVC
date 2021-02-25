using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerProject.Models
{
    public class Customer
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter your Full name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Please enter your First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your age")]
        public short Age { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public HttpPostedFileBase avatar { get; set; }        
    }
}