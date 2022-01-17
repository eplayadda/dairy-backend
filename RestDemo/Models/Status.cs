using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace RestDemo.Models
{
    public class Status
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        public string Result { set; get; }
        public string Message { set; get; }
    }
}