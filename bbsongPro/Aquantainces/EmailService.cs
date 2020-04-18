using bbsongPro.Foundations;
using bbsongPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace bbsongPro.Aquantainces
{
    public class EmailService
    {
        User email;
        public EmailService()
        {
            var db = new UserFoundation();
            email = db.GetUser("email");
        }
        public void Log(string message)
        {
            Send(email.Username, message, "ErrorLog " + DateTime.Now);
        }

        public void SendConfirmationMessage(string to, int order_Id)
        {
            Send(to, "Tak for din bestilling! \n" +
                " Dit ordrenr- " + order_Id + "\n\n" +
                "Mvh. bbsongspro", "Ordrebekræftelse");
        }

        void Send(string to, string messageToSend, string subject)
        {


            var mail = new MailMessage();
            mail.From = new MailAddress(email.Username);
            mail.To.Add(new MailAddress(to));
            mail.Subject = subject;
            mail.Body = messageToSend;


            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(email.Username, email.Password);


            client.Send(mail);
        }

    }
}
