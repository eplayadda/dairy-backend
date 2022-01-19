using MongoDB.Driver;
using RestDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestDemo.BusinessLogic
{
    public class InvoiceService
    {
        string mDBName = DairyConstant._dbName;// "learnsmart";
        string mCollectionName = "invoice";
        public Invoice AddInvoice(Invoice invoice)
        {
            #region InsertDetails  
            if (string.IsNullOrEmpty(invoice.Id))
            {
                NewInvoiceEntry(invoice);
                return invoice;
            }
            #endregion
            ///Update Emoloyeee  
            #region updateDetails  
            else
            {
                UpdateInvoice(invoice);
                return invoice;
            }
            #endregion
        }
        void NewInvoiceEntry(Invoice invoice)
        {
            var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(mDBName);
            var collection = database.GetCollection<Invoice>("invoice");
            var plant = collection.Find(Builders<Invoice>.Filter.Where(s => s.farm_id == invoice.farm_id
                                                                                         && s.customer_id == invoice.customer_id
                                                                                         && s.inv_for_month == invoice.inv_for_month
                                                                                         && s.inv_for_year == invoice.inv_for_year)).ToList().FirstOrDefault();
            InvoiceByCustomerID invoiceByCustomer = new InvoiceByCustomerID();
            invoiceByCustomer.customer_id = invoice.customer_id;
            var lastInvoice = getLastInvoiceByCustomer(invoiceByCustomer);

            if (lastInvoice == null)
            {
                CustomerBalanceDetail customerBalanceDetail = GetPrevAmount(invoice.customer_id);
                invoice.advanceAmount = customerBalanceDetail.advance;
                invoice.dueAmount = customerBalanceDetail.due;

            }
            else
            {
                float lastInvoiceDetails = lastInvoice.amount - lastInvoice.payment;
                if (lastInvoiceDetails > 0)
                    invoice.dueAmount = lastInvoiceDetails;
                else
                    invoice.advanceAmount = Math.Abs(lastInvoiceDetails);

            }
            invoice.amount = (invoice.total_ltr_evening + invoice.total_ltr_morning) * invoice.rate_per_ltr + invoice.dueAmount - invoice.advanceAmount;
            if (plant == null)
            {
                collection.InsertOne(invoice);
            }
            else
            {
                invoice.Id = plant.Id;
                UpdateInvoice(invoice);
            }
        }
        void UpdateInvoice(Invoice invoice)
        {
            var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(mDBName);
            var collection = database.GetCollection<Invoice>("invoice");
            var update = collection.FindOneAndUpdateAsync(Builders<Invoice>.Filter.Eq("Id", invoice.Id), Builders<Invoice>.
                Update.Set("customer_id", invoice.customer_id).Set("farm_id", invoice.farm_id).Set("inv_for_month", invoice.inv_for_month).
                Set("inv_for_year", invoice.inv_for_year).Set("inv_generate_date", invoice.inv_generate_date).Set("rate_per_ltr", invoice.rate_per_ltr).
                Set("total_ltr_morning", invoice.total_ltr_morning).Set("total_ltr_evening", invoice.total_ltr_evening).Set("dueAmount", invoice.dueAmount).
                Set("amount", invoice.amount).Set("payment", invoice.payment).Set("advanceAmount", invoice.advanceAmount));
        }

        public CustomerResponceBody getInvoiceByMonth(InvoiceRequestBody invoiceRequestBody)
        {
            try
            {
                if (string.IsNullOrEmpty(invoiceRequestBody.customer_id))
                {
                    // This is for all customer for same farm and month and year
                    var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                    var client = new MongoClient(settings);
                    var database = client.GetDatabase(mDBName);
                    var collection = database.GetCollection<Invoice>(mCollectionName);
                    var plant = collection.Find(Builders<Invoice>.Filter.Where(s => s.farm_id == invoiceRequestBody.farm_id
                                                                                         && s.inv_for_month == invoiceRequestBody.inv_for_month
                                                                                         && s.inv_for_year == invoiceRequestBody.inv_for_year)).ToList();
                    CustomerResponceBody customerResponceBody = new CustomerResponceBody();
                    customerResponceBody.customeList = plant;
                    return customerResponceBody;
                }
                else
                {
                    // this is use for particular customer for same farm and month and year
                    var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                    var client = new MongoClient(settings);
                    var database = client.GetDatabase(mDBName);
                    var collection = database.GetCollection<Invoice>(mCollectionName);
                    var plant = collection.Find(Builders<Invoice>.Filter.Where(s => s.customer_id == invoiceRequestBody.customer_id
                                                                                         && s.inv_for_month == invoiceRequestBody.inv_for_month
                                                                                         && s.inv_for_year == invoiceRequestBody.inv_for_year)).ToList();

                    CustomerResponceBody customerResponceBody = new CustomerResponceBody();
                    customerResponceBody.customeList = plant;
                    return customerResponceBody;
                }
            }
            catch(Exception e)
            {
                return null;
            }
          
        }

        public CustomerResponceBody getAllInvoice(InvoiceByFramID invoiceByFramID)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                var client = new MongoClient(settings);
                var database = client.GetDatabase(mDBName);
                List<string> framCustomer = GetCustomerIDInMyFram(invoiceByFramID.farm_id);
                var collection = database.GetCollection<Invoice>(mCollectionName);
                var plant = collection.Find(Builders<Invoice>.Filter.Where(s => s.farm_id == invoiceByFramID.farm_id
                                                                                   && framCustomer.Contains(s.customer_id))).ToList();
                var res = from element in plant
                          group element by element.customer_id
                into groups
                          select groups.OrderByDescending(p => p.inv_for_year).OrderByDescending(p => p.inv_for_month).First();



                CustomerResponceBody customerResponceBody = new CustomerResponceBody();
                customerResponceBody.customeList = res.ToList();
                return customerResponceBody;
            }
            catch(Exception e)
            {
                return null;
            }
           
        }

        public Invoice getLastInvoiceByCustomer(InvoiceByCustomerID invoiceByCustomerID)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                var client = new MongoClient(settings);
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<Invoice>("invoice");

                var allInvoiceByCustomer = collection.Find(Builders<Invoice>.Filter.Where(s => s.customer_id == invoiceByCustomerID.customer_id
                                                                                    )).ToList();
                var res = allInvoiceByCustomer.OrderByDescending(p => p.inv_for_year).OrderByDescending(p => p.inv_for_month).First();
                return res;
            }
            catch(Exception e)
            {
                return null;
            }
           
        }

        public object addPaymentDetails(PaymentDetailsRequestBody paymentDetailsRequestBody)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                var client = new MongoClient(settings);
                var database = client.GetDatabase(mDBName);

                var collection = database.GetCollection<Invoice>(mCollectionName);
                PaymentDetail paymentDetail = new PaymentDetail();
                paymentDetail.payment_date = paymentDetailsRequestBody.dateTime;
                paymentDetail.payment_amount = paymentDetailsRequestBody.paid_amount;
                var plant = collection.Find(Builders<Invoice>.Filter.Eq("Id", paymentDetailsRequestBody.invoice_id)).FirstOrDefault();
                var payDetils = plant.payment_details;
                payDetils.Add(paymentDetail);
                float totalPayment = plant.payment + paymentDetail.payment_amount;
                var update = collection.FindOneAndUpdate(Builders<Invoice>.Filter.Eq("Id", paymentDetailsRequestBody.invoice_id), Builders<Invoice>.
                    Update.Set("payment_details", payDetils).Set("payment", totalPayment));
                var newplant = collection.Find(Builders<Invoice>.Filter.Eq("Id", paymentDetailsRequestBody.invoice_id)).FirstOrDefault();
                return newplant;
            }
            catch(Exception e)
            {
                return null;
            }
      
        }

        public object IsInvoiceCreated(InvoiceRequestBody invoiceRequestBody)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                var client_inv = new MongoClient(settings);
                var database_inv = client_inv.GetDatabase(mDBName);
                var collection_inv = database_inv.GetCollection<Invoice>(mCollectionName);
                var plant_inv = collection_inv.Find(Builders<Invoice>.Filter.Where(s => s.farm_id == invoiceRequestBody.farm_id)).ToList();
                var res_inv = from element in plant_inv
                              group element by element.customer_id
                into groups
                              select groups.OrderByDescending(p => p.inv_for_month).OrderByDescending(p => p.inv_for_year).First();

                CustomerResponceBody customerResponceBody = new CustomerResponceBody();
                customerResponceBody.customeList = res_inv.ToList();
                List<string> inv_Cust_id = new List<string>();
                foreach (var item in customerResponceBody.customeList)
                {
                    string str = item.customer_id + item.inv_for_month + item.inv_for_year;
                    inv_Cust_id.Add(str);
                }

                OrderRequestBody orderRequestBody = new OrderRequestBody();
                orderRequestBody.customerID = invoiceRequestBody.customer_id;
                orderRequestBody.month = invoiceRequestBody.inv_for_month;
                orderRequestBody.year = invoiceRequestBody.inv_for_year;
                orderRequestBody.farmID = invoiceRequestBody.farm_id;
                var client1 = new MongoClient(settings);
                var database1 = client1.GetDatabase(mDBName);
                var collection1 = database1.GetCollection<Order_info>("order_info");
                var plant1 = collection1.Find(Builders<Order_info>.Filter.Where(s => s.farmID == orderRequestBody.farmID
                                                                                   && (s.o_year < orderRequestBody.year
                                                                                   || (s.o_year == orderRequestBody.year && s.o_month <= orderRequestBody.month)))).ToList();
                Order allOrder = new Order();
                if (plant1.Count > 0)
                {
                    var res = from element in plant1
                              group element by element.o_customer_id
                   into groups
                              select groups.OrderByDescending(p => p.o_month).OrderByDescending(p => p.o_year);

                    List<Order_info> order_Infos = new List<Order_info>();
                    foreach (var item in res)
                    {
                        var order = plant1.Where(s => s.o_customer_id == item.First().o_customer_id
                        && s.o_month == item.First().o_month
                        && s.o_year == item.First().o_year && !inv_Cust_id.Contains(s.o_customer_id + s.o_month + s.o_year));
                        order_Infos.AddRange(order);
                    }
                    allOrder.order_Infos = order_Infos;
                    return allOrder;
                }
                else
                {
                    return allOrder;
                }
            }
            catch(Exception e)
            {
                return null;
            }
         
        }

        public object getPrevInvoice(PrevInvoiceByCustomey pPrevInvoiceByCustomey)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                var client = new MongoClient(settings);
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<Invoice>(mCollectionName);
                var plant = collection.Find(Builders<Invoice>.Filter.Where(s => s.customer_id == pPrevInvoiceByCustomey.customerID
                                                                                  && (s.inv_for_year < pPrevInvoiceByCustomey.year
                                                                                   || (s.inv_for_year == pPrevInvoiceByCustomey.year && s.inv_for_month <= pPrevInvoiceByCustomey.month)))).ToList();
                if (plant.Count > 0)
                {
                    var res = from element in plant
                              group element by element.inv_for_year
                              into groups
                              select groups.OrderByDescending(p => p.inv_for_year).OrderByDescending(p => p.inv_for_month).First();
                    Invoice oInfo = res.First();

                    return oInfo;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        
        }

        public object getNextInvoice(PrevInvoiceByCustomey pPrevInvoiceByCustomey)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                var client = new MongoClient(settings);
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<Invoice>(mCollectionName);
                var plant = collection.Find(Builders<Invoice>.Filter.Where(s => s.customer_id == pPrevInvoiceByCustomey.customerID
                                                                                  && (s.inv_for_year > pPrevInvoiceByCustomey.year
                                                                                   || (s.inv_for_year == pPrevInvoiceByCustomey.year && s.inv_for_month >= pPrevInvoiceByCustomey.month)))).ToList();
                if (plant.Count > 0)
                {
                    var res = from element in plant
                              group element by element.inv_for_year
                               into groups
                              select groups.OrderBy(p => p.inv_for_month).OrderBy(p => p.inv_for_year).First();
                    Invoice oInfo = res.Last();

                    return oInfo;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                return null;
            }
          
        }
        List<string> GetCustomerIDInMyFram(string pFramID)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                var client = new MongoClient(settings);
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<CustomerInfo>("customer_info");
                var plant = collection.Find(Builders<CustomerInfo>.Filter.Where(s => s.farm_id == pFramID && s.c_delete == false)).ToList();
                List<string> customerID = plant.Select(s => s.Id).ToList();
                return customerID;

            }
            catch (Exception e)
            {
                return null;
            }

        }
        public CustomerBalanceDetail GetPrevAmount(string id)
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString(DairyConstant._cunnectionString);
                var client = new MongoClient(settings);
                var database = client.GetDatabase(mDBName);
                var collection = database.GetCollection<CustomerInfo>("customer_info");
                var plant = collection.Find(Builders<CustomerInfo>.Filter.Where(s => s.Id == id)).First();
                CustomerBalanceDetail customerBalanceDetail = new CustomerBalanceDetail();
                customerBalanceDetail.advance = plant.c_preAdvance;
                customerBalanceDetail.due = plant.c_preDue;

                return customerBalanceDetail;
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}