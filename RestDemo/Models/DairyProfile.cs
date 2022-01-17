using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestDemo.Models
{
    public class DairyProfile
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        public string farm_id { get; set; }
        public string mob_number { get; set; }
        public string dairy_name { get; set; }
        public string owner_name { get; set; }
        public string address { get; set; }
        public int logo_id { get; set; }
    }

    public class DairyByFramID
    {
        public string farm_id;
    }
}