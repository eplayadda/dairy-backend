using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestDemo.Models
{
    public class DairyShop
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        public string h_product_name { get; set; }
        public string h_product_discription { get; set; }
        public string h_product_about { get; set; }
        public string h_product_price { get; set; }
        public string h_product_discount { get; set; }
        public string h_product_marketPrice { get; set; }
        public string h_product_quantity { get; set; }
        public string e_product_name { get; set; }
        public string e_product_discription { get; set; }
        public string e_product_about { get; set; }
        public string e_product_price { get; set; }
        public string e_product_discount { get; set; }
        public string e_product_marketPrice { get; set; }
        public string e_product_quantity { get; set; }

    }

    public class AllProducts
    {
       public List<DairyShop> _allProduts ;
    }
    public class DairyStr
    {
        public string str { get; set; }
    }
}