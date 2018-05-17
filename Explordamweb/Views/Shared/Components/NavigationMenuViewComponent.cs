using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Explordamweb.Models;


namespace Explordamweb.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IGamesRepository repository;
        public NavigationMenuViewComponent(IGamesRepository repo)
        {
            repository = repo;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Games
            .Select(x => x.Genre)
            .Distinct()
            .OrderBy(x => x));
        }
    }
}