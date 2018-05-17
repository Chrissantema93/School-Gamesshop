using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Explordamweb.Models;
using Explordamweb.Models.ViewModels;
using System.Net;
using System.Net.Mail;

namespace Explordamweb.Controllers
{
    public class HomeController : Controller
    {
        private IGamesRepository repository;

        public HomeController(IGamesRepository repo)
        {
            repository = repo;
        }

        public IActionResult Home()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Contact()
        {
            return View(new EmailForm());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailForm model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("gameshopproject56@hotmail.com"));  // replace with valid value 
                message.From = new MailAddress("gameshopproject56@hotmail.com");  // replace with valid value
                message.Subject = "Your email subject";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "gameshopproject56@hotmail.com",  // replace with valid value
                        Password = "Crack1slekker"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Succes");
                }
            }
            return View(model);
        }

        public IActionResult Succes()
        {
            return View();
        }

    }
}


