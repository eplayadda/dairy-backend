using MongoDB.Driver;
using RestDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RestDemo.Controllers
{
    public class DairyProfileController : ApiController
    {
        string mDBName = DairyConstant._dbName;
        string mCollectionName = "dairy_profile";
        [HttpPost]
        public object createDairyProfile(DairyProfile pDairyProfile)
        {
            if (string.IsNullOrEmpty(pDairyProfile.Id))
            {
                var client = new MongoClient("mongodb://localhost/:27017");
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<DairyProfile>(mCollectionName);
                collection.InsertOne(pDairyProfile);
                return pDairyProfile;
            }
            else
            {
                var client = new MongoClient("mongodb://localhost/:27017");
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<DairyProfile>(mCollectionName);
                var update = collection.FindOneAndUpdateAsync(Builders<DairyProfile>.Filter.Eq("Id",pDairyProfile.Id), Builders<DairyProfile>.
                    Update.Set("mob_number", pDairyProfile.mob_number).Set("dairy_name", pDairyProfile.dairy_name)
                    .Set("owner_name", pDairyProfile.owner_name).Set("logo_id", pDairyProfile.logo_id).Set("address", pDairyProfile.address)
                    .Set("farm_id", pDairyProfile.farm_id));
                return pDairyProfile;
            }
        }

        [HttpPost]
        public object getDairyByFarmID(DairyByFramID pDairyByFramID)
        {
            try
            {
                var client = new MongoClient("mongodb://localhost/:27017");
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<DairyProfile>(mCollectionName);
                var plant = collection.Find(Builders<DairyProfile>.Filter.Where(s => s.farm_id == pDairyByFramID.farm_id)).FirstOrDefault();
                return plant;

            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}
