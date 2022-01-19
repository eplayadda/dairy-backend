using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestDemo.Models
{
    public class DairyConstant
    {
       // public static string _cunnectionString = "mongodb://localhost/:27017";
        public static string _dbName = "demo";
        public static string _cunnectionString = "mongodb+srv://chandan:chandan123@cluster0.oscgg.mongodb.net/"+ _dbName + "?retryWrites=true&w=majority";
    }
}