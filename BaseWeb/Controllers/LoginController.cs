using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using BaseWeb.ViewModels;

namespace BaseWeb.Controllers
{
    public class LoginController : Controller
    {

        //private readonly IConfiguration configuration;

        //public LoginController(IConfiguration _configuration)
        //{
        //    configuration = _configuration;
        //}
        public IActionResult Index()
        {

            //string myDb1ConnectionString = this.configuration.GetConnectionString("Default");
            return View();
        }
        [HttpPost]
        public IActionResult Index(UserLoginViewModel userLogin)
        {
            if (!ModelState.IsValid)
                return View(userLogin); 

            var AuthCntr = new AuthController();

            AuthCntr.StoreSession();

            
            return RedirectToAction(actionName:"Index",controllerName:"Home");
        }
    }
}
