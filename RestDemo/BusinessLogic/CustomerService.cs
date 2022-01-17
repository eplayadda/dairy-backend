using RestDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestDemo.BusinessLogic
{
    public class CustomerService
    {
        public Invoice GetInvoice(CustomerInfo customerInfo)
        {
            Invoice invoice = new Invoice();
            invoice.customer_id = customerInfo.Id;
            invoice.farm_id = customerInfo.farm_id;
            invoice.inv_for_month = 1;
            invoice.inv_for_year = 2000;
            try
            {
                DateTime _dateTime = DateTime.Parse(customerInfo.customerGenerated_date);
                _dateTime =_dateTime.AddMonths(-1);
                invoice.inv_for_month = _dateTime.Month;
                invoice.inv_for_year = _dateTime.Year;
                return invoice;
            }
            catch (Exception e)
            {
                return invoice;
            }

        }
    }
}