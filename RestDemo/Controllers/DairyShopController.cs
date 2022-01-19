using MongoDB.Bson;
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
    public class DairyShopController : ApiController
    {
        string mCollectionName = "dairy_shop";
        string mDBName = DairyConstant._dbName;//"learnsmart";
        [HttpPost]
        public object getProducts(DairyStr str)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                var client = new MongoClient(settings);
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<DairyShop>(mCollectionName).Find(new BsonDocument()).ToList();
                AllProducts allProducts = new AllProducts();
                allProducts._allProduts = collection;
                return allProducts;
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        public object addProducts(DairyShop pDairyShop)
        {
            try
            {   ///Insert Emoloyeee  
                #region InsertDetails  
                if (string.IsNullOrEmpty(pDairyShop.Id))
                {
                    var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                    var client = new MongoClient(settings);
                    var database = client.GetDatabase(mDBName);
                    var collection = database.GetCollection<DairyShop>(mCollectionName);
                    collection.InsertOne(pDairyShop);
                    return pDairyShop;
                }
                #endregion
                ///Update Emoloyeee  
                #region updateDetails  
                else
                {
                    var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                    var client = new MongoClient(settings);
                    var database = client.GetDatabase(mDBName);
                    var collection = database.GetCollection<DairyShop>(mCollectionName);
                    var update = collection.FindOneAndUpdateAsync(Builders<DairyShop>.Filter.Eq("Id", pDairyShop.Id), Builders<DairyShop>.
                        Update.Set("h_product_name", pDairyShop.h_product_name).Set("h_product_discount", pDairyShop.h_product_discount).
                        Set("h_product_about", pDairyShop.h_product_about).Set("h_product_price", pDairyShop.h_product_price).
                        Set("h_product_discount", pDairyShop.h_product_discount).Set("h_product_marketPrice", pDairyShop.h_product_marketPrice).
                        Set("h_product_quantity", pDairyShop.h_product_quantity)
                        .Set("e_product_name", pDairyShop.e_product_name).Set("e_product_discription", pDairyShop.e_product_discription).
                        Set("e_product_about", pDairyShop.e_product_about).Set("e_product_price", pDairyShop.e_product_price).
                        Set("e_product_discount", pDairyShop.e_product_discount).Set("e_product_marketPrice", pDairyShop.e_product_marketPrice).
                        Set("e_product_quantity", pDairyShop.e_product_quantity));
                    return pDairyShop;
                }
                #endregion
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
