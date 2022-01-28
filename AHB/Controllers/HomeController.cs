using Microsoft.AspNetCore.Mvc;

namespace AHB
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
