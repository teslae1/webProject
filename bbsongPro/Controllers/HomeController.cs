using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using bbsongPro.Models;
using bbsongPro.Mediators;
using Stripe;
using Stripe.Checkout;
using bbsongPro.Aquantainces;
using System;

namespace bbsongPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }




        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PersonalInfo(List<SongPart> songParts)
        {
            //remove nulls from list
            List<SongPart> filteredSongparts = new List<SongPart>();
            foreach (SongPart sp in songParts)
                if (sp.Link != null)
                    filteredSongparts.Add(sp);

            //save to order
            OrderMediator.CurrentOrder.SongParts = filteredSongparts;


            return View();
        }



        [HttpPost]
        public IActionResult DisplayOrderCheckout(string email, string comment)
        {
            OrderMediator.CurrentOrder.Email = email;
            OrderMediator.CurrentOrder.Comment = comment;

            OrderMediator.UploadCurrentOrder();

            var session = GetStripeSession();

            return View(new OrderCheckoutModel(OrderMediator.CurrentOrder, session.Id));
        }
        public Session GetStripeSession()
        {
            StripeConfiguration.ApiKey = "sk_test_ujFk5C8DtykdgFolICYfbSKV00w0VhwNIu";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Name = "Sang",
                        Description = "En sang sammensat af valgte lydklip",
                        Amount = 29900,
                        Currency = "dkk",
                        Quantity = 1
                    }
                },

                SuccessUrl = Url.Action("Success", "Home", null, Request.Scheme),
                CancelUrl = Url.Action("Index", "Home", null, Request.Scheme)
            };


            var service = new SessionService();


            return service.Create(options);
        }



        public IActionResult Success()
        {
            var order = OrderMediator.CurrentOrder;
            OrderMediator.MarkAsPaymentReceived(order);

            var mailService = new EmailService();
            try
            {
                mailService.SendConfirmationMessage(order.Email, order.Id);
            }
            catch (Exception ee)
            {
                mailService.Log("Did not send mail on order_Id: " + order.Id + "\n" + "error message; + " + ee.Message);
            }

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
