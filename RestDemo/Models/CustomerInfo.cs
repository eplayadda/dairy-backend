using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace RestDemo.Models
{
    public class CustomerInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        public string farm_id { get; set; }
        public string c_name { get; set; }
        public string c_mob_number { get; set; }
        public string c_floor { get; set; }
        public string c_building { get; set; }
        public string c_road_number { get; set; }
        public string c_area { get; set; }
        public string c_pin_code { get; set; }
        public string c_landmark { get; set; }
        public string c_image_url { get; set; }
        public string c_sift { get; set; }
        public bool c_active { get; set; }
        public bool c_delete { get; set; }
        public float c_rate { get; set; }
        public float c_preDue { get; set; }
        public float c_preAdvance { get; set; }
        public int rank { get; set; }
        public string language { get; set; }
        public string customerGenerated_date { get; set; }
    }

    public class Customer
    {
        public List<CustomerInfo> customerInfos;
    }
}