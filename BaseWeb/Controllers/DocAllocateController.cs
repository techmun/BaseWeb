using BaseWeb.Cores;
using BaseWeb.Data;
using BaseWeb.Models;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Http.Headers;

namespace BaseWeb.Controllers
{
    public class DocAllocateController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ImportDA(IList<IFormFile> files)
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
                ds = ImportExport.ImportExcel(filepath, "DA", 2);
                using (var context = new AppDbContext())
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        if (dt.TableName != "Readme")
                        {
                            var dtName = dt.TableName.Split("-");
                            var SerId = dtName[0].ToString();
                            if (SerId.Contains("Sheet"))
                            {
                                var govDept = dtName[1].ToString().Replace(" ", "");
                                var serCount = context.Services.Where(m => m.GovDept == govDept).Count() + 1;
                                SerId = govDept + serCount.ToString().PadLeft(3,'0');
                            }
                            var service = new Services()
                            {
                                SerID = SerId,
                                GovDept = dtName[1].ToString().Replace(" ", ""),
                                SerName = dtName[2].ToString(),
                                CreatedBy = WebSession.GetSession(EnumSession.UserName),
                                CreatedDate = DateTime.Now,
                                EditedBy = WebSession.GetSession(EnumSession.UserName),
                                EditedDate = DateTime.Now
                            };

                            context.Services.Add(service);

                            foreach (DataRow row in dt.Rows)
                            {
                                if (!string.IsNullOrEmpty(row["Item"].ToString()))
                                {
                                    var docs = new Documents()
                                    {
                                        SerID = dtName[0].ToString(),
                                        Document = row["Item"].ToString(),
                                        PreparedBy = row["Client/Runner"].ToString(),
                                        Mandatory = row["Mandatory"].ToString(),
                                        UploadDoc = row["Upload Doc"].ToString(),
                                        CreatedBy = WebSession.GetSession(EnumSession.UserName),
                                        CreatedDate = DateTime.Now,
                                        EditedBy = WebSession.GetSession(EnumSession.UserName),
                                        EditedDate = DateTime.Now,
                                        Step = "1"
                                    };
                                    context.Documents.Add(docs);
                                }


                            }
                        }
                    }
                    context.SaveChanges();

                }

                return Json(new {success=true});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }


    }
}
