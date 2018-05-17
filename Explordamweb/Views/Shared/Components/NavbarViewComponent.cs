using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Explordamweb.Models;

namespace Explordamweb.Views.Shared.Components
{
    public class NavbarViewComponent : ViewComponent
    {
        public NavbarViewComponent()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
