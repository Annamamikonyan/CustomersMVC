﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Customers
{
    public class Customer
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string FirstName { get; set; }
        public short Age { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
    }
}
