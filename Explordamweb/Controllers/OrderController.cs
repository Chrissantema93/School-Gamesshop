using Microsoft.AspNetCore.Mvc;
using Explordamweb.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System;

namespace Explordamweb.Controllers

{
    public class OrderController : Controller
    {
        private UserManager<AppUser> userManager;
        private IOrderRepository repository;
        private Cart cart;
        public OrderController(UserManager<AppUser> usrMgr, IOrderRepository repoService, Cart cartService)
        {
            userManager = usrMgr;
            repository = repoService;
            cart = cartService;
        }

        [Authorize]
        public IActionResult List() =>
            View(repository.Orders.Where(o => !o.Shipped));
        [HttpPost]
        [Authorize]
        public IActionResult MarkShipped(int orderID)
        {
            Order order = repository.Orders
            .FirstOrDefault(o => o.OrderID == orderID);
            if (order != null)
            {
                order.Shipped = true;
                repository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public IActionResult Checkout() => View(new CheckoutModel());

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel checkout)
        {

            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {

                checkout.Order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(checkout.Order);

                bool mailsend;
                


                try
                {
                    string items = "<table style='font-family: arial, sans-serif;border-collapse: collapse;width: 100%;'><tr style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'> <td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'><b>Picture</b></td> <td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'><b>Name</b></td> <td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'><b>Quantity</b></td> <td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'><b>Price</b></td> </tr>";
                    decimal total = 0;
                    foreach (var game in checkout.Order.Lines)
                    {
                        items += "<tr style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'><td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'><img style='width:150px' src='"+ game.Game.HeaderImage +"' /></td><td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'>" + game.Game.QueryName + "</td><td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'>"+ game.Quantity +"</td><td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'> " + (game.Quantity * game.Game.PriceFinal) + ",-</td></tr>";
                        total += (game.Quantity * game.Game.PriceFinal);
                    }
                    items += "<tr style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'><td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'></td><td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'></td><td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'><b>Total</b></td><td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'>"+total+",-</td></tr></table>";


                    var body = "<div>" +
                        "<div style='width:100%;height:50px;background-color:#1a75ff;text-align:center'>" +
                            "<h2 style='font-size:40px'> Your Order </h2>" +
                        "</div>" +
                        "<hr/>" +
                        "<center><div style='background-color: #f0f0f0; width:750px;'>" +
                            "<br>" +
                            "<table style='font-family: arial, sans-serif;border-collapse: collapse;width: 100%;' border='0'>" +
                                "<tr style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'>" +
                                    "<td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'>" +
                                        "<b>OrderID:</b>" +
                                    "</td>" +
                                    "<td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'>" +
                                        checkout.Order.OrderID +
                                    "</td>" +
                                "</tr>" +
                                "<tr style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'>" +
                                    "<td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'>" +
                                        "<b>Date of order:</b>" +
                                    "</td>" +
                                    "<td style='border: 1px solid #e6e6e6; text - align: left; padding: 8px'>" +

                                        checkout.Order.Orderdate +

                                    "</td>" +
                                "</tr>" +
                            "</table>" +
                            "<br>" +
                            "<br>" +
                            "<br>" +
                            "<br>" +
                                items +
                            "<br>" +
                            "<br>" +
                            "<br>" +
                            "<p style='font-size:1.25rem;text-align:center'>Thank you for ordering at GameShop, till next time!</p>" +
                            "<br>" +
                        "</div></center>" +
                    "</div>";


                    var message = new MailMessage();
                    message.To.Add(new MailAddress(checkout.Order.Email));
                    message.From = new MailAddress("gameshopproject56@hotmail.com");
                    message.Subject = "Your order, with ID: " + checkout.Order.OrderID + " from GameShop";
                    message.Body = string.Format(body, "GameShop", "gameshopproject56@hotmail.com", items);
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "gameshopproject56@hotmail.com",
                            Password = "**classified**"
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp-mail.outlook.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);
                    }

                    mailsend = true;
                }
                catch
                {
                    mailsend = false;
                }

                if (mailsend == true)
                {
                    repository.EditStatus(checkout.Order);
                }


                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(checkout);
            }
        }
        public IActionResult Completed()
        {
            cart.Clear();
            return View();
        }
    }
}