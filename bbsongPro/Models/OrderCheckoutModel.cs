using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbsongPro.Models
{
    public class OrderCheckoutModel
    {
        public Order Order;
        public string SessionId;
        public OrderCheckoutModel(Order order, string sessionId)
        {
            Order = order;
            SessionId = sessionId;
        }
    }
}
