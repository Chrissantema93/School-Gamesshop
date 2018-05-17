using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Explordamweb.Models;

namespace Explordamweb.Views.Shared.Components
{
    public class UserMenuViewComponent : ViewComponent
    {
        public UserMenuViewComponent()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
