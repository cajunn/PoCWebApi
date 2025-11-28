using Microsoft.AspNetCore.Mvc;

namespace PoCWebApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
