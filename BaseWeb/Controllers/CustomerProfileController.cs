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
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.InkML;

namespace BaseWeb.Controllers
{
    public class CustomerProfileController : Controller
    {
        [Authentication]
        [NoDirectAccess]
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

        [Authentication]
        [NoDirectAccess]
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
                var test = String.IsNullOrEmpty(model.Add3) ? "" : model.Add3;
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
                        Add3 = String.IsNullOrEmpty(model.Add3)?"":model.Add3,
                        Add4 = String.IsNullOrEmpty(model.Add4) ? "" : model.Add4,
                        PostCode = Int32.Parse(model.PostCode),
                        CreatedBy = HttpContext.Session.GetString("UserName"),
                        CreatedDate = DateTime.Now,
                        EditedBy = HttpContext.Session.GetString("UserName"),
                        EditedDate = DateTime.Now
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
        [HttpPost]
        public IActionResult DeleteCust(string CustCode)
        {
            try
            {
                using (var context = new AppDbContext()) {
                    var custProf = context.CustProf.Where(x=>x.CustCode == CustCode).FirstOrDefault();
                    if(custProf != null)
                    {

                        context.CustProf.Remove(custProf);
                        context.SaveChanges();
                    }
                    else
                    {
                        return Json(new { message ="Company not found", success = false });
                    }
                    
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false });
            }
            finally { 
            }
            
        }

        [Authentication]
        [NoDirectAccess]
        public IActionResult ExpiryDate(string CustCode, string CustName)
        {
            var model = new ExpiryDateViewModel();
            model.CustCode = CustCode;
            model.CustName = CustName;

            var list = new SelectList(new[]
            {
                new { ID = "", Name = "" },
                new { ID = "DA", Name = "DA" },
                new { ID = "PC", Name = "PC" }
            },
            "ID", "Name", 1);

            ViewData["list"] = list;
            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> ImportCP(IList<IFormFile> files)
        {
            string filepath = "";

            try
            {
                foreach (IFormFile source in files)
                {
                    string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');

                    filename = this.EnsureCorrectFilename(filename);
                    filepath = WebSession.GetSession(EnumSession.ImportPath) + filename;
                    using (FileStream output = System.IO.File.Create(filepath))
                        await source.CopyToAsync(output);
                }
                DataSet ds = new DataSet();
                ds = ImportExport.ImportExcel(filepath, "CP");
                using (var context = new AppDbContext())
                {
                    var custProf = context.CustProf.ToList();
                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            var custCode = row["Code"].ToString();
                            if (!string.IsNullOrEmpty(custCode) && custProf.Where(m=>m.CustCode == custCode).ToList().Count() == 0 )
                            {
                                var docs = new CustProf()
                                {
                                    CustCode = row["Code"].ToString(),
                                    CustName = row["Company Name"].ToString(),
                                    Email = row["Email Address"].ToString(),
                                    ContactPS = row["Attention"].ToString(),
                                    ContactNo = row["Phone 1"].ToString(),
                                    Add1 = row["Address 1"].ToString(),
                                    Add2 = row["Address 2"].ToString(),
                                    Add3 = row["Address 3"].ToString(),
                                    Add4 = row["Address 4"].ToString(),
                                    PostCode = row["Post Code"].ToString() == ""?0:int.Parse(row["Post Code"].ToString()),
                                    CreatedBy = WebSession.GetSession(EnumSession.UserName),
                                    CreatedDate = DateTime.Now,
                                    EditedBy = WebSession.GetSession(EnumSession.UserName),
                                    EditedDate = DateTime.Now
                                };
                                context.CustProf.Add(docs);
                            }
                        }
                    }
                    context.SaveChanges();

                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddExpiryDate(ExpiryDateViewModel model)
        {
            var expiryDateList = new object();
            try
            {
                using(var context = new AppDbContext())
                {
                    var expDate = new ExpiryDate()
                    {
                        CustCode = model.CustCode,
                        DocType = model.DocType,
                        Description = model.Description,
                        ExpiredDate = model.ExpiredDate,
                        CreatedBy = WebSession.GetSession(EnumSession.UserName),
                        CreatedDate = DateTime.Now,
                        EditedBy = WebSession.GetSession(EnumSession.UserName),
                        EditedDate = DateTime.Now
                    };
                    context.ExpiryDate.Add(expDate);
                    context.SaveChanges();
                }

                using (var context = new AppDbContext())
                {
                    expiryDateList = getExpiryList(context, model.CustCode);
                }
                var lsExDate = JsonConvert.SerializeObject(expiryDateList);
                return Json(new { lsExDate = lsExDate, success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }

            return Json(new { success = true });
        }
        public JsonResult GetExpirylist(string CustCode) {
            var expiryDateList = new object();
            try
            {
                using (var context = new AppDbContext())
                {
                    expiryDateList = getExpiryList(context, CustCode);
                }
                var lsExDate = JsonConvert.SerializeObject(expiryDateList);
                return Json(new { lsExDate = lsExDate, success = true });

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message });
            }
            finally
            {

            }
            
        }
        [HttpPost]
        public IActionResult DeleteExpiryDate(int id,string CustCode)
        {
            var expiryDateList = new object();
            try
            {
                using (var context = new AppDbContext())
                {
                    var expiryDate = context.ExpiryDate.Where(x => x.Id == id).FirstOrDefault();
                    if(expiryDate != null)
                    {

                        context.ExpiryDate.Remove(expiryDate);
                        context.SaveChanges();
                        expiryDateList = getExpiryList(context, CustCode);
                    }
                    else
                    {
                        return Json(new { message ="id not found", success = false });
                    }

                }
                var lsExDate = JsonConvert.SerializeObject(expiryDateList);
                return Json(new { lsExDate = lsExDate, success = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false });
            }
            finally
            {
            }

        }
        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private object getExpiryList(AppDbContext context,string custCode)
        {
            return (from a in context.ExpiryDate
                    where a.CustCode == custCode
                    select new { Id = a.Id,ExDate = a.ExpiredDate, ExpiredDate = a.ExpiredDate.ToString("dd/MM/yyyy"), DocType = a.DocType, Description = a.Description }).OrderBy(m=>m.ExDate).ToList();
        }

    }
}
