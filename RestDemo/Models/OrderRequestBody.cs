﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace RestDemo.Models
{
    public class OrderRequestBody
    {
        public string farmID { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string customerID { get; set; }
        
    }
}