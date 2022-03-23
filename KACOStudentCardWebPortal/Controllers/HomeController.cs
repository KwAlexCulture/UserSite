using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KACOStudentCardWebPortal.Models;
using System.Net.Mail;
using BOL;
using BLL;

namespace KACOStudentCardWebPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ContactService _contactService;

        public HomeController(ContactService contactService)
        {
            _contactService = contactService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CovidPredictions()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult OfficeEmployeeEMailContact()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> PostContactRequest(Contact Contact, string FullName, string Email, int PhoneNumber, string MessageHeader, string MessageBody)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Contact contact = new Contact()
                    {
                        ContactFullName = Contact.ContactFullName,
                        ContactNationalIDNo = Contact.ContactNationalIDNo,
                        ContactEmail = Contact.ContactEmail,
                        ContactPhoneNo = Contact.ContactPhoneNo,
                        ContactMessageHeader = Contact.ContactMessageHeader,
                        ContactMessageBody = Contact.ContactMessageBody,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        StatusCode = true,
                    };
                    //MailMessage mail = new MailMessage();
                    //mail.To.Add("info@kwalexculture.org");
                    //mail.From = new MailAddress(Email);
                    //mail.Subject = MessageHeader;
                    //string Body = MessageBody;
                    //mail.Body = "Message from" + FullName + " " + Body + " " + "Phone Number:" + PhoneNumber;
                    //mail.IsBodyHtml = true;
                    //SmtpClient smtp = new SmtpClient();
                    //smtp.Host = "mail.kwalexculture.org";
                    //smtp.Port = 465;
                    //smtp.UseDefaultCredentials = false;
                    ////smtp.Credentials = new System.Net.NetworkCredential("username", "password"); // Enter seders User name and password  
                    //smtp.EnableSsl = true;
                    //smtp.Send(mail);
                    await _contactService.SaveAsync(contact);
                    ViewBag.Success = "true";
                    return Json(new { err = "" });
                }
                else
                {
                    ViewBag.Success = "true";
                    return Json(new { err = "" });
                }
            }
            catch(Exception ex)
            {
                var innerEx = ex.InnerException;
                return Json(new { err = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
