using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Driver;
using RestDemo.Models;
using MongoDB.Bson;
using RestDemo.BusinessLogic;

namespace RestDemo.Controllers
{
    public class InvoiceController : ApiController
    {
        [HttpPost]
        public object addInvoice(Invoice invoice)
        {
            try
            {   ///Insert Emoloyeee  
                InvoiceService invoiceService = new InvoiceService();
                return invoiceService.AddInvoice(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
      
        [HttpPost]
        public object getInvoiceByMonth(InvoiceRequestBody invoiceRequestBody)
        {
            try
            {
                InvoiceService invoiceService = new InvoiceService();
                return invoiceService.getInvoiceByMonth(invoiceRequestBody);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
       
        [HttpPost]
        public object getAllInvoice(InvoiceByFramID invoiceByFramID)
        {
            try
            {

                InvoiceService invoiceService = new InvoiceService();
                return invoiceService.getAllInvoice(invoiceByFramID);

            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
   
        [HttpPost]
        public Invoice getLastInvoiceByCustomer(InvoiceByCustomerID invoiceByCustomerID)
        {
            try
            {
                InvoiceService invoiceService = new InvoiceService();
                return invoiceService.getLastInvoiceByCustomer(invoiceByCustomerID);
            }
            catch (Exception)
            {
                return null;
            }
        }
        [HttpPost]
        public object addPaymentDetails(PaymentDetailsRequestBody paymentDetailsRequestBody)
        {
            try
            {
                InvoiceService invoiceService = new InvoiceService();
                return invoiceService.addPaymentDetails(paymentDetailsRequestBody);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public object IsInvoiceCreated(InvoiceRequestBody invoiceRequestBody)
        {
            try
            {
                InvoiceService invoiceService = new InvoiceService();
                return invoiceService.IsInvoiceCreated(invoiceRequestBody);
            }
            catch(Exception e)
            {
                return null;
            }
            
        }

      
        [HttpPost]
        public object getPrevInvoice(PrevInvoiceByCustomey pPrevInvoiceByCustomey)
        {
            try
            {
                InvoiceService invoiceService = new InvoiceService();
                return invoiceService.getPrevInvoice(pPrevInvoiceByCustomey);

            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public object getNextInvoice(PrevInvoiceByCustomey pPrevInvoiceByCustomey)
        {
            try
            {
                InvoiceService invoiceService = new InvoiceService();
                return invoiceService.getNextInvoice(pPrevInvoiceByCustomey);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
