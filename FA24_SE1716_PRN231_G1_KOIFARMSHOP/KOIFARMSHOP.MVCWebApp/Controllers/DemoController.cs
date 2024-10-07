using Microsoft.AspNetCore.Mvc;

namespace KOIFARMSHOP.MVCWebApp.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
