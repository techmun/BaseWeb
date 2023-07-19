
using Microsoft.AspNetCore.Mvc;
using static BaseWeb.Utilities.ActionFilterConfig;

namespace BaseWeb.Controllers
{
    public class HomeController : Controller
    {
        
        [Authentication]
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}