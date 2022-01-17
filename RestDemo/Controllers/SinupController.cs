using MongoDB.Driver;
using RestDemo.BusinessLogic;
using RestDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace RestDemo.Controllers
{

    public class SinupController : ApiController
    {
        string mDBName = DairyConstant._dbName;
        string mCollectionName = "farm";
        [HttpPost]
        public object validateMobile(MobileIfno pMobileIfno)
        {
            try
            {
                var client = new MongoClient("mongodb://localhost/:27017");
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<Farm>(mCollectionName);
                var plant = collection.Find(Builders<Farm>.Filter.Where(s => s.mob_number == pMobileIfno.mob_number)).ToList();
                MobileValidation mobileValidation = new MobileValidation();
                if (plant.Count>0)
                {
                    mobileValidation.isMobileAlreadyRegistered = true;
                    if (pMobileIfno.isForgotPassword)
                    {
                        createOTP(pMobileIfno);
                    }
                    return mobileValidation;
                }
                else
                {
                    mobileValidation.isMobileAlreadyRegistered = false;
                    createOTP(pMobileIfno);
                    return mobileValidation;
                }
               
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


        public void createOTP(MobileIfno pMobileIfno)
        {
          //  OTPService.CreateOTP(pMobileIfno.mob_number);
            OTPService.TestingOTP(pMobileIfno.mob_number);
        }
        [HttpPost]
        public object onSubmitOTP(OtpInfo otpInfo)
        {
            OtpStatus otpStatus = new OtpStatus();
            string dbOTP = "";
            if (OTPService.otpbank.ContainsKey(otpInfo.mob_number))
            {
                dbOTP = OTPService.otpbank[otpInfo.mob_number];
            }
            if (otpInfo.otp == dbOTP)
            {
                otpStatus.isOTPValid = true;
                OTPService.otpbank.Remove(otpInfo.mob_number);
                return otpStatus;
            }
            else
            {
                otpStatus.isOTPValid = false;
                return otpStatus;
            }
          
        }
        [HttpPost]
        public object createPassword(Farm farm)
        {
            var client = new MongoClient("mongodb://localhost/:27017");
            var database = client.GetDatabase(mDBName);
            var collection = database.GetCollection<Farm>(mCollectionName);
            var data = database.GetCollection<Farm>(mCollectionName);

            var plant = collection.Find(Builders<Farm>.Filter.Where(s => s.mob_number == farm.mob_number)).ToList();
            try
            {
                if (plant.Count == 0)
                {
                    data.InsertOne(farm);
                    return farm;
                }
                else
                {
                    var update = data.FindOneAndUpdateAsync(Builders<Farm>.Filter.Eq("mob_number", farm.mob_number), Builders<Farm>.
                        Update.Set("password", farm.password));
                    farm.Id = plant[0].Id;
                    return farm;
                }
            }
            catch(Exception e)
            {
                return BadRequest();
            }
           
        }
       
    }
}
