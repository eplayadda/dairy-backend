using MongoDB.Driver;
using RestDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using RestDemo.BusinessLogic;

namespace RestDemo.Controllers
{
   
    public class CustomerController : ApiController
    {
       
        string mCollectionName = "customer_info";
        string mDBName =  DairyConstant._dbName;//"learnsmart";
        [HttpGet]
        public object getAllCustomer()
        {
            try
            {

                var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                var client = new MongoClient(settings);
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<CustomerInfo>(mCollectionName).Find(new BsonDocument()).ToList();
                Customer allCustomer = new Customer();
                allCustomer.customerInfos = collection;
                return allCustomer;
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        [HttpPost]
        public object getCustomerByFarmID(CustomerByFramID customerByFramID)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                var client = new MongoClient(settings);
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<CustomerInfo>(mCollectionName);
                var plant = collection.Find(Builders<CustomerInfo>.Filter.Where(s => s.farm_id == customerByFramID.farm_id
                && s.c_delete == false)).ToList();
                Customer allCustomer = new Customer();
                allCustomer.customerInfos = plant;
                return allCustomer;

            }
            catch (Exception)
            {
                return BadRequest();
            }
           
        }
        [HttpPost]
        public object addCustomerByFarmID(CustomerInfo customerInfo)
        {
            try
            {   ///Insert Emoloyeee  
                #region InsertDetails  
                if (string.IsNullOrEmpty(customerInfo.Id ))
                {
                    var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                    var client = new MongoClient(settings);
                    var database = client.GetDatabase(mDBName);
                    var collection = database.GetCollection<CustomerInfo>(mCollectionName);
                    customerInfo.rank = CreateCustomerIndex(customerInfo.farm_id);
                  //  var newCustomer;
                    collection.InsertOne(customerInfo);
                    InvoiceService invoiceService = new InvoiceService();
                    CustomerService customerService = new CustomerService();
                    Invoice invoice = customerService.GetInvoice(customerInfo);
                    invoiceService.AddInvoice(invoice);
                    return customerInfo;
                }
                #endregion
                ///Update Emoloyeee  
                #region updateDetails  
                else
                {
                    var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                    var client = new MongoClient(settings);
                    var database = client.GetDatabase(mDBName);
                    var collection = database.GetCollection<CustomerInfo>(mCollectionName);
                    var update = collection.FindOneAndUpdateAsync(Builders<CustomerInfo>.Filter.Eq("Id", customerInfo.Id), Builders<CustomerInfo>.
                        Update.Set("farm_id", customerInfo.farm_id).Set("c_name", customerInfo.c_name).Set("c_mob_number", customerInfo.c_mob_number).
                        Set("c_floor", customerInfo.c_floor).Set("c_building", customerInfo.c_building).Set("c_road_number", customerInfo.c_road_number).
                        Set("c_area", customerInfo.c_area).Set("c_pin_code", customerInfo.c_pin_code).Set("c_landmark", customerInfo.c_landmark).
                        Set("c_image_url", customerInfo.c_image_url).Set("c_delete", customerInfo.c_delete).Set("c_rate", customerInfo.c_rate)
                        .Set("c_preDue", customerInfo.c_preDue).Set("c_preAdvance", customerInfo.c_preAdvance).Set("language", customerInfo.language).Set("c_active", customerInfo.c_active));
                    return customerInfo;
                }
                #endregion
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

      
        int CreateCustomerIndex(string pFarmID)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                var client = new MongoClient(settings);
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<CustomerInfo>(mCollectionName);
                var plant = collection.Find(Builders<CustomerInfo>.Filter.Where(s => s.farm_id == pFarmID)).ToList();
                return plant.Count;
            }
            catch (Exception)
            {
                return -1;
            }
        }
      
    }
}
