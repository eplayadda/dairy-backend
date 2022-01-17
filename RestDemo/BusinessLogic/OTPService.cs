using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace RestDemo.BusinessLogic
{
    public class OTPService
    {
        public static Dictionary<string, string> otpbank = new Dictionary<string, string>();
        public static void CreateOTP(string mob)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://smsapi.edumarcsms.com/api/v1/sendsms");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    OTPCustomer cust = new OTPCustomer();
                    cust.number = new List<string>();

                    cust.apikey = "xxxxxxxxxxxxxxxxxxxxxxx";
                    string otp = RandomNumber();
                    cust.message = "your OTP is"+ otp;
                    cust.senderId = "EDUMRC";

                    cust.number.Add(mob);
                    if (otpbank.ContainsKey(mob))
                        otpbank.Remove(mob);
                    otpbank.Add(mob, otp);
                    string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(cust);

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    ReleaseOTP(mob);
                    Console.WriteLine(result);
                }
            }
            catch (Exception ex)
            {
               // Console.WriteLine(ex.Message);
            }

        }
        static void ReleaseOTP(string mob)
        {
            Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(600000);
                if (otpbank.ContainsKey(mob))
                {
                     otpbank.Remove(mob);
                }
            });
        }
        public class OTPCustomer
        {
            public string apikey;
            public string message;
            public List<string> number;
            public string senderId;
        }
        static string RandomNumber()
        {
            Random random = new Random();
            int otp = random.Next(1000, 9999);
            return otp.ToString();
        }
        public static void TestingOTP(string mob)
        {
            string otp ="0110";
            if (otpbank.ContainsKey(mob))
                otpbank.Remove(mob);
            otpbank.Add(mob, otp);
            ReleaseOTP(mob);
        }
    }
}