using BaseWeb.Cores;
using BaseWeb.DAL;
using BaseWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static BaseWeb.Utilities.ActionFilterConfig;
using System.Data;
using BaseWeb.Data;
using BaseWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseWeb.Controllers
{
    public class CustomerProfileController : Controller
    {
        [Authentication]
        public IActionResult Index()
        {
            return View();
        }

        [NoDirectAccess]
        public JsonResult GetCustProfList(string filter)
        {
            List<CustProf> selCustProf = new List<CustProf>();
            var CustProfDAL = new CustProfDAL(ConnStr.connection());
            DataTable dt;
            try
            {
                 if (string.IsNullOrEmpty(filter))
                    filter = "";

                dt = CustProfDAL.getCustProfByFilter(filter);

                var lsCustProf = JsonConvert.SerializeObject(dt);
                return Json(new { lsCustProf = lsCustProf, success = true });
            }
            catch (Exception ex)
            {

                return Json(new { message = ex.Message, success = false });
            }
            finally
            {
                
                selCustProf = null;
            }
        }

        [HttpGet]
        public IActionResult AddEdit(string QueryType)
        {

            return View("AddEdit");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddEdit(AddEditCustProfViewModel model, string queryType)
        {
            if (!ModelState.IsValid)
                return View("AddEdit", model);

            //var CustProfDAL = new CustProfDAL(ConnStr.connection());
            try
            {
                using (var context = new AppDbContext())
                {
                    var CustProf = new CustProf()
                    {
                        CustCode = model.CustCode,
                        CustName = model.CustName,
                        ContactPS = model.ContactPS,
                        ContactNo = model.ContactNo,
                        Email = model.Email,
                        Add1 = model.Add1,
                        Add2 = model.Add2,
                        Add3 = model.Add3,
                        Add4 = model.Add4,
                        PostCode = model.PostCode,
                        CreatedBy = HttpContext.Session.GetString("UserName"),
                        EditedBy = HttpContext.Session.GetString("UserName")
                    };

                    context.CustProf.Add(CustProf);
                    context.SaveChanges();
                }

                    //CustProfDAL.addCustProf(model, queryType, HttpContext.Session.GetString("UserName"));

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false });
            }
            finally
            {
            }

        }
    }
}
