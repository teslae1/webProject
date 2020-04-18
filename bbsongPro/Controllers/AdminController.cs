using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bbsongPro.Foundations;
using bbsongPro.Mediators;
using bbsongPro.Models;
using Microsoft.AspNetCore.Mvc;

namespace bbsongPro.Controllers
{
    public class AdminController : Controller
    {
        static User admin;
        public IActionResult Login()
        {
            if (admin == null)
                admin = new UserFoundation().GetUser("admin");
            return View();
        }

        static List<Order> orders;

        [HttpPost]
        public IActionResult Orders(string username, string password)
        {
            //do username password check here
            if (username != null && password != null)
                if (username == admin.Username && password == admin.Password)
                {
                    orders = OrderMediator.GetAllValidOrders();
                    return View(orders);
                }


            return View("Login");
        }

        public IActionResult UpdateOrder(int id)
        {
            OrderMediator.MarkAsComplete(orders.Single
                (order => order.Id == id));

            return View("Orders", orders);
        }


    }
}