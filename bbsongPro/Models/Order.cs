using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbsongPro.Models
{
    public class Order
    {
        public List<SongPart> SongParts { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public int Id { get; set; }
        public DateTime CreationDate { get; private set; }
        public bool Completed { get; set; }
        public bool PaymentReceived { get; set; }
        public Order(int id, string email, string comment,
            DateTime creationDate,
            bool orderCompleted, bool paymentReceived)
        {
            Id = id;
            Email = email;
            Comment = comment;
            CreationDate = creationDate;
            Completed = orderCompleted;
            PaymentReceived = paymentReceived;
        }

        public Order()
        {

        }




    }
}
