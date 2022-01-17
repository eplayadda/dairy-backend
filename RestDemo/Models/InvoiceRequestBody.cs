using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestDemo.Models
{
    public class InvoiceRequestBody
    {
        public string customer_id { get; set; }
        public string farm_id { get; set; }
        public int inv_for_month { get; set; }
        public int inv_for_year { get; set; }
    }
    public class PaymentDetailsRequestBody
    {
        public string invoice_id { get; set; }
        public string dateTime { get; set; }
        public float paid_amount { get; set; }

    }
}