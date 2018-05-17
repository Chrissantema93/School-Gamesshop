using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Explordamweb.Infrastructure;
using Explordamweb.Models;
using Explordamweb.Models.ViewModels;

namespace Explordamweb.Controllers
{
    public class CartController : Controller
    {
        private IGamesRepository repository;
        private Cart cart;
        public CartController(IGamesRepository repo, Cart cartService)
        {
            repository = repo;
            cart = cartService;
        }
        public IActionResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToActionResult AddToCart(int id, string returnUrl)
        {
            Games game = repository.Games
            .FirstOrDefault(p => p.ID == id);
            if (game != null)
            {
                cart.AddItem(game, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToActionResult RemoveFromCart(int id,
        string returnUrl)
        {
            Games game = repository.Games
            .FirstOrDefault(p => p.ID == id);
            if (game != null)
            {
                cart.RemoveLine(game);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
    }
}