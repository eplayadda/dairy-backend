using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using RestDemo.Models;
//using RestDemo.Class;

namespace RestDemo.Controllers
{
    public class LoginController : ApiController
    {
        private static readonly HttpClient client = new HttpClient();
        string mCollectionName = "farm";
        string mDBName = DairyConstant._dbName;//"learnsmart";
        [HttpPost]
        public object getLogin(Farm login_credentials)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                var client = new MongoClient(settings);
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<Farm>(mCollectionName);
                var plant = collection.Find(Builders<Farm>.Filter.Where(s => s.mob_number == login_credentials.mob_number && s.password == login_credentials.password)).FirstOrDefault();
                if (plant != null)
                {
                    return plant;
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }
    }
}

