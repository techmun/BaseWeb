using Microsoft.AspNetCore.Mvc;
using static BaseWeb.Utilities.ActionFilterConfig;

namespace BaseWeb.Controllers
{
    public class ProcessingListController : Controller
    {
        [Authentication]
        [NoDirectAccess]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult _da()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult AddEditDA(string QueryType)
        {

            return View("AddEditDA");
        }
    }
}
