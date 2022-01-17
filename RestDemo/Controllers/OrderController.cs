using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Driver;
using RestDemo.Models;
using MongoDB.Bson;

namespace RestDemo.Controllers
{
    public class OrderController : ApiController
    {
        string mDBName = DairyConstant._dbName;//"learnsmart";
        string mCollectionName = "order_info";
        [HttpGet]
        public object Get()
        {
            try
            {
                var client = new MongoClient("mongodb://localhost/:27017");
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<Order_info>(mCollectionName).Find(new BsonDocument()).ToList();
                Order allOrder = new Order();
                allOrder.order_Infos = collection;
                return allOrder;
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        [HttpPost]
        public object getOrderByOrderID(OrderByOrderID orderByOrderID)
        {
            try
            {
                var client = new MongoClient("mongodb://localhost/:27017");
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<Order_info>(mCollectionName);
                var plant = collection.Find(Builders<Order_info>.Filter.Where(s => s.Id == orderByOrderID.order_id)).FirstOrDefault();
                return plant;
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }
        [HttpPost]
        public object addOrder(Order_info order_Info)
        {
            try
            {   ///Insert Emoloyeee  
                #region InsertDetails  
                if (string.IsNullOrEmpty(order_Info.Id))
                {
                    var client = new MongoClient("mongodb://localhost/:27017");
                    var database = client.GetDatabase(mDBName);
                    var collection = database.GetCollection<Order_info>(mCollectionName);
                    collection.InsertOne(order_Info);
                    return order_Info;
                }
                #endregion
                ///Update Emoloyeee  
                #region updateDetails  
                else
                {
                    var client = new MongoClient("mongodb://localhost/:27017");
                    var database = client.GetDatabase(mDBName);
                    var collection = database.GetCollection<Order_info>(mCollectionName);
                    var update = collection.FindOneAndUpdateAsync(Builders<Order_info>.Filter.Eq("Id", order_Info.Id), Builders<Order_info>.
                        Update.Set("o_customer_id", order_Info.o_customer_id).Set("o_sift", order_Info.o_sift).Set("o_date", order_Info.o_date).
                        Set("o_month", order_Info.o_month).Set("o_year", order_Info.o_year).Set("o_quantity", order_Info.o_quantity));
                    return order_Info;
                }
                #endregion
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public object getOrderListByMonth(OrderRequestBody orderRequestBody)
        {
            try
            {
                if (orderRequestBody.customerID.Equals(""))
                {
                    // This is for all customer for same farm and month and year
                    var client = new MongoClient("mongodb://localhost/:27017");
                    var database = client.GetDatabase(mDBName);
                    var collection = database.GetCollection<Order_info>(mCollectionName);
                    var plant = collection.Find(Builders<Order_info>.Filter.Where(s => s.farmID == orderRequestBody.farmID
                                                                                         && s.o_month == orderRequestBody.month
                                                                                         && s.o_year == orderRequestBody.year)).ToList();
                    Order allOrder = new Order();
                    allOrder.order_Infos = plant;
                    return allOrder;

                }
                else
                {
                    // this is use for particular customer for same farm and month and year
                    var client = new MongoClient("mongodb://localhost/:27017");
                    var database = client.GetDatabase(mDBName);
                    var collection = database.GetCollection<Order_info>(mCollectionName);
                    var plant = collection.Find(Builders<Order_info>.Filter.Where(s => s.o_customer_id == orderRequestBody.customerID
                                                                                         && s.o_month == orderRequestBody.month
                                                                                         && s.o_year == orderRequestBody.year)).ToList();

                    Order allOrder = new Order();
                    allOrder.order_Infos = plant;
                    return allOrder;

                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        public object getPrevOrder(OrderRequestBody orderRequestBody)
        {
            try
            {
                var client = new MongoClient("mongodb://localhost/:27017");
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<Order_info>(mCollectionName);
                var plant = collection.Find(Builders<Order_info>.Filter.Where(s => s.o_customer_id == orderRequestBody.customerID
                                                                                   &&( s.o_year < orderRequestBody.year
                                                                                   || (s.o_year == orderRequestBody.year && s.o_month <= orderRequestBody.month)))).ToList();
                if (plant.Count > 0)
                {

                    var res = from element in plant
                              group element by element.o_year
                   into groups
                              select groups.OrderByDescending(p => p.o_year).OrderByDescending(p => p.o_month).First();

                    Order_info oInfo = res.First();

                    var orderID = collection.Find(Builders<Order_info>.Filter.Where(s => s.o_customer_id == orderRequestBody.customerID
                                                                                          && s.o_month == oInfo.o_month
                                                                                          && s.o_year == oInfo.o_year)).ToList();

                    Order allOrder = new Order();
                    allOrder.order_Infos = orderID;
                    return allOrder;
                }
                else return null;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public object getNextOrder(OrderRequestBody orderRequestBody)
        {
            try
            {
                var client = new MongoClient("mongodb://localhost/:27017");
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<Order_info>(mCollectionName);
                var plant = collection.Find(Builders<Order_info>.Filter.Where(s => s.o_customer_id == orderRequestBody.customerID
                                                                                   && (s.o_year > orderRequestBody.year
                                                                                   || (s.o_year == orderRequestBody.year && s.o_month >= orderRequestBody.month)))).ToList();
                if (plant.Count > 0)
                {

                    var res = from element in plant
                              group element by element.o_year
                              into groups
                              select groups.OrderBy(p => p.o_month).OrderBy(p => p.o_year).First();
                    Order_info oInfo = res.Last();

                    var orderID = collection.Find(Builders<Order_info>.Filter.Where(s => s.o_customer_id == orderRequestBody.customerID
                                                                                          && s.o_month == oInfo.o_month
                                                                                          && s.o_year == oInfo.o_year)).ToList();

                    Order allOrder = new Order();
                    allOrder.order_Infos = orderID;
                    return allOrder;
                }
                else return null;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public object getPrevOrderByFarmID(OrderRequestBody orderRequestBody)
        {
            try
            {
                var client = new MongoClient("mongodb://localhost/:27017");
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<Order_info>(mCollectionName);
                var plant = collection.Find(Builders<Order_info>.Filter.Where(s => s.farmID == orderRequestBody.farmID
                                                                                   && (s.o_year < orderRequestBody.year
                                                                                   || (s.o_year == orderRequestBody.year && s.o_month <= orderRequestBody.month)))).ToList();
                if (plant.Count > 0)
                {
                    var res = from element in plant
                              group element by element.o_customer_id
                   into groups
                              select groups.OrderByDescending(p => p.o_month).OrderByDescending(p => p.o_year);
                    Order allOrder = new Order();
                    List<Order_info> order_Infos = new List<Order_info>();
                    foreach (var item in res)
                    {
                        var order = plant.Where(s => s.o_customer_id == item.First().o_customer_id
                        && s.o_month == item.First().o_month
                        && s.o_year == item.First().o_year);
                        order_Infos.AddRange(order);
                    }
                    allOrder.order_Infos = order_Infos;
                    return allOrder;
                }
                else return null;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
