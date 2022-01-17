using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace RestDemo.Models
{
    public class Farm
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        public string mob_number { get; set; }
        public string  password { get; set; }
}
}