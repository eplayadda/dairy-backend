using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace RestDemo.Models
{
    public class Invoice
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        public string customer_id { get; set; }
        public string farm_id { get; set; }
        public int inv_for_month { get; set; }
        public int inv_for_year { get; set; }
        public string inv_generate_date { get; set; }
        public int rate_per_ltr { get; set; }
        public float total_ltr_morning { get; set; }
        public float total_ltr_evening { get; set; }
        public float advanceAmount { get; set; }
        public float dueAmount { get; set; }
        public float amount { get; set; }
        public float payment { get; set; }
        public List<PaymentDetail> payment_details = new List<PaymentDetail>();
    }
    public class PaymentDetail
    {
        public string payment_date { get; set; }
        public float payment_amount { get; set; }
    }
    public class CustomerResponceBody
    {
        public List<Invoice> customeList { get; set; }
    }

    public class CustomerBalanceDetail
    {
        public float advance;
        public float due;
    }
}