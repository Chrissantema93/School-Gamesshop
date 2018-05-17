using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Explordamweb.Models;

namespace Explordamweb.Views.Shared.Components
{
    public class AdminMenuViewComponent : ViewComponent
    {
        public AdminMenuViewComponent()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
