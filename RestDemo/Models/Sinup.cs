using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestDemo.Models
{
    public class Sinup
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        public string mob_number { get; set; }
        public string password { get; set; }
    }
    public class MobileIfno
    {
        public string mob_number { get; set; }
        public bool isForgotPassword { get; set; }
    }

    public class MobileValidation
    {
        public bool isMobileAlreadyRegistered;
    }
    public class FarmAvailability
    {
        public bool isFarmAvailable;
    }

    public class OtpInfo
    {
        public string mob_number { get; set; }
        public string otp { get; set; }
    }
    public class OtpStatus
    {
        public bool isOTPValid;
    }


}