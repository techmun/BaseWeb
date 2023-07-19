using BaseWeb.Cores;
using BaseWeb.DAL;
using BaseWeb.Data;
using BaseWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Configuration;
using System.Data;

namespace BaseWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration configuration;

        public AuthController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                Decrypt decrypt = new Decrypt();
                Encrypt encrypt = new Encrypt();;
                string enc = encrypt.Encrypted("1234");
                return View();
            }
            else
            {
                return RedirectToAction(controllerName:"Home",actionName:"Index");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserLoginViewModel userLogin)
        {
            if (!ModelState.IsValid)
                return View(userLogin);

            bool isSuccess = isLoginSuccess(ref userLogin);

            if (!isSuccess)
            {
                ModelState.AddModelError("LoginId", "User Not Found");
                return View(userLogin);
            }
            else
            {
                HttpContext.Session.SetString("UserName", userLogin.LoginId);
            }


            return RedirectToAction(controllerName: "Home", actionName: "Index");
        }

        public IActionResult Logout()
        {

            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");

            return RedirectToAction("Login");
        }

        private bool isLoginSuccess(ref UserLoginViewModel userLogin)
        {
            Decrypt decrypt = new Decrypt();
            Encrypt encrypt = new Encrypt();
            string constr = decrypt.Decrypted(configuration.GetConnectionString("Default"));

            LoginDAL dal = new LoginDAL(constr);

            var dt = dal.getUserPwById(userLogin.LoginId);

            if (dt.Rows.Count > 0)
            {
                var pw = dt.AsEnumerable().Select(r => r["Password"]).ToList().FirstOrDefault();
                var ePw = encrypt.Encrypted(userLogin.Password);
                if (pw.ToString() != ePw)
                {

                    userLogin.errMsg = "Login Id or Password is incorrect!";
                    return false;
                }
            }
            else
            {
                ModelState.AddModelError("LoginId", "User Not Found");
                return false;
            }


            return true;
        }
    }
}
