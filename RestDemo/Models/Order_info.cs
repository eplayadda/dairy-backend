using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace RestDemo.Models
{
    public class Order_info
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        public string o_sift { get; set; }
        public string farmID { get; set; }
        public int o_date { get; set; }
        public int o_month { get; set; }
        public int o_year { get; set; }
        public string o_customer_id { get; set; }
        public float o_quantity { get; set; }
    }

    public class Order
    {
        public List<Order_info> order_Infos;
    }
}