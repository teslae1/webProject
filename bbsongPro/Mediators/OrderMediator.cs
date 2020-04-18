using bbsongPro.Foundation;
using bbsongPro.Models;
using System;
using System.Collections.Generic;

namespace bbsongPro.Mediators
{
    /// <summary>
    /// only ever gets called from controller
    /// only ever calls database
    /// handles one order at a time 
    /// returns the order that was uploaded
    /// </summary>
    public static class OrderMediator
    {
        public static Order CurrentOrder = new Order();
        static OrderFoundation orderDb = new OrderFoundation();
        /// <summary>
        /// returns the order which was uploaded
        /// </summary>
        /// <returns></returns>
        public static void UploadCurrentOrder()
        {
            orderDb.Create(CurrentOrder);
        }

        public static void MarkAsComplete(Order order)
        {
            orderDb.MarkAsComplete(order);
                order.Completed = true;
        }
        public static void MarkAsPaymentReceived(Order order)
        {
            orderDb.MarkAsPaymentReceived(order);
            order.PaymentReceived = true;
        }

        public static List<Order> GetAllValidOrders()
        {
            var orders = orderDb.GetAll();
            var sorted = new List<Order>();
            foreach (var order in orders)
                if (order.PaymentReceived)
                    sorted.Add(order);

            return sorted;
        }
    }
}
