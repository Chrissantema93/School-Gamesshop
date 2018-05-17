using Microsoft.AspNetCore.Mvc;
namespace Explordamweb.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error() => View();
    }
}
