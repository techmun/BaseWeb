using BaseWeb.Cores;
using BaseWeb.DAL;
using BaseWeb.Models;
using BaseWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace BaseWeb.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            if (WebSession.GetSession(EnumSession.UserName) == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction(controllerName:"ProcessingList",actionName:"Index");
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
                storeInitSession(userLogin.LoginId);
            }


            return RedirectToAction(controllerName: "ProcessingList", actionName: "Index");
        }

        public IActionResult Logout()
        {

            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");

            return RedirectToAction("Login");
        }

        private bool isLoginSuccess(ref UserLoginViewModel userLogin)
        {
            string constr = ConnStr.connection();

            LoginDAL dal = new LoginDAL(constr);

            var dt = dal.getUserPwById(userLogin.LoginId);

            if (dt.Rows.Count > 0)
            {
                var pw = dt.AsEnumerable().Select(r => r["Password"]).ToList().FirstOrDefault();
                var ePw = Encrypt.Encrypted(userLogin.Password);
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

        private void storeInitSession(string LoginId)
        {
            WebSession.storeSession(EnumSession.UserName, LoginId);
            WebSession.storeSession(EnumSession.ImportPath, @"..\BaseWeb\Files\Import\");
            WebSession.storeSession(EnumSession.ExportPath, @"..\BaseWeb\Files\Export\");
        }
    }
}
