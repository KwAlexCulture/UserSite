using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace KACOStudentCardWebPortal.Models
{
    //[HttpPost]
    //public ActionResult SendEmail(string receiver, string subject, string message)
    //{
    //            var senderEmail = new MailAddress("jamilmoughal786@gmail.com", "Jamil");
    //            var receiverEmail = new MailAddress(receiver, "Receiver");
    //            var password = "Your Email Password here";
    //            var sub = subject;
    //            var body = message;
    //            var smtp = new SmtpClient
    //            {
    //                Host = "smtp.gmail.com",
    //                Port = 587,
    //                EnableSsl = true,
    //                DeliveryMethod = SmtpDeliveryMethod.Network,
    //                UseDefaultCredentials = false,
    //                Credentials = new NetworkCredential(senderEmail.Address, password)
    //            };
    //            using (var mess = new MailMessage(senderEmail, receiverEmail)
    //            {
    //                Subject = subject,
    //                Body = body
    //            })
    //            {
    //                smtp.Send(mess);
    //            }

    //}
    //return true;
}
