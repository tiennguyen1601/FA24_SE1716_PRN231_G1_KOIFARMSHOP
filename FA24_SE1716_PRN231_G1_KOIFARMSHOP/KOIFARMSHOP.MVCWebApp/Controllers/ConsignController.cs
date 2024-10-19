using Microsoft.AspNetCore.Mvc;

namespace KOIFARMSHOP.MVCWebApp.Controllers
{
    public class ConsignController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Token"] = HttpContext.Session.GetString("Token");
            return View();
        }
    }
}
