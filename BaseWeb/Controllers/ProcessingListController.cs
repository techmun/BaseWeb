using BaseWeb.Cores;
using Microsoft.AspNetCore.Mvc;
using static BaseWeb.Utilities.ActionFilterConfig;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Data;
using BaseWeb.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;
using BaseWeb.Data;
using BaseWeb.Migrations;
using BaseWeb.ViewModels;
using BaseWeb.DAL;
using AspNetCore;

namespace BaseWeb.Controllers
{
    public class ProcessingListController : Controller
    {
        [Authentication]
        [NoDirectAccess]
        public IActionResult Index()
        {
            var model = new List<ProcListViewModel>();
            try
            {
                using (var context = new AppDbContext())
                {
                    var efProcess = (from jp in context.JobProcess
                                     join cp in context.CustProf on jp.CustCode equals cp.CustCode
                                     
                                     select new
                                     {
                                         jp.CustCode,
                                         cp.CustName,
                                         jp.SerID,
                                         jp.GovDept,
                                         jp.Services,
                                         jp.GovURL,
                                         jp.ExpiryPeriod
                                     } ).GroupBy(m=>m.CustCode).Select(m=>m.ToList()).ToList();
                   
                    foreach(var custList in efProcess)
                    {
                        var vm = new ProcListViewModel();
                        vm.CustCode = custList[0].CustCode;
                        vm.CustName = custList[0].CustName;
                        vm.services = new List<Services>();
                        foreach(var item in custList)
                        {
                            var services = new Services()
                            {
                                SerID = item.SerID,
                                SerName = item.Services,
                                GovURL = item.GovURL,
                                GovDept = item.GovDept,
                                ExpiryPeriod = item.ExpiryPeriod
                            };
                            vm.services.Add(services);
                        }
                        model.Add(vm);
                    }
                }

            }
            catch (Exception ex)
            {

            }finally {
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddEditJob(string QueryType)
        {

            return View("AddEditJob");
        }

        [HttpPost]
        public JsonResult AddEditJob(string queryType, string selComp, string jobList)
        {
            var errMsg = "";
            try
            {
                var jobArr = jobList.Split(',');
                if (queryType == "I")
                {
                    using (var context = new AppDbContext())
                    {
                        foreach (var job in jobArr)
                        {
                        var guid = Guid.NewGuid();
                        var jobProcess = new JobProcess();
                        var document = new DocProcess();
                            var ser = context.Services.Where(m=>m.SerID == job).FirstOrDefault();
                            if (ser != null)
                            {
                                jobProcess = new JobProcess()
                                {
                                    Id = guid,
                                    CustCode = selComp,
                                    SerID = ser.SerID,
                                    GovDept = ser.GovDept,
                                    Services = ser.SerName,
                                    GovURL = ser.GovURL,
                                    ExpiryPeriod = ser.ExpiryPeriod,
                                    CreatedBy = WebSession.GetSession(EnumSession.UserName),
                                    CreatedDate = DateTime.Now,
                                    EditedBy = WebSession.GetSession(EnumSession.UserName),
                                    EditedDate = DateTime.Now

                                };
                               
                            }
                            else
                            {
                                errMsg += job + " services not found ! \n";
                                continue;
                            }

                            var docList= context.Documents.Where(m => m.SerID == job);
                            if (docList != null)
                            {
                                foreach(var doc in docList)
                                {

                                    document = new DocProcess()
                                    {
                                        JobId = guid,
                                        SerID =doc.SerID,
                                        Document = doc.Document,
                                        PreparedBy = doc.PreparedBy,
                                        Mandatory = doc.Mandatory,
                                        UploadDoc = doc.UploadDoc,
                                        DocCount= doc.DocCount,
                                        Step = doc.Step
                                    };

                                    context.DocProcess.Add(document);
                                }
                            }
                            else
                            {
                                errMsg += job + " document not found ! \n";
                                continue;
                            }
                            context.JobProcess.Add(jobProcess);
                        };
                        context.SaveChanges();
                    }
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false });
            }
        }
        public JsonResult InitLoad()
        {
            List<Services> serList = new List<Services>();
            var context = new AppDbContext();
            try
            {
                serList = context.Services.ToList();
                var govList = context.Services.Select(m => m.GovDept).Distinct();
                return Json(new { serList = serList, govList= govList, success = true });
            }
            catch (Exception ex)
            {

                return Json(new { message = ex.Message, success = false });
            }
            finally
            {
            }
        }

        public JsonResult loadCheckListByCode(string code)
        {
            try
            {
                var context = new AppDbContext();
                var docs = context.Documents.Where(m=>m.SerID == code).ToList();
                return Json(new {rows=docs});
            }catch(Exception ex)
            {
                return Json(new { result=false });
            }
        }

        public ActionResult ProcessJob(string CustCode)
        { 
            var model = new JobProcessViewModel();
            var jobList = new List<JobProcess>();
            var docList = new List<DocProcess>();

            var plDAL = new ProcessingListDAL(ConnStr.connection());
            DataSet ds = new DataSet();
            ds = plDAL.getProcessingList(CustCode);
            docList = DataConvertor.CreateListFromTable<DocProcess>(ds.Tables[0]);
            jobList = DataConvertor.CreateListFromTable<JobProcess>(ds.Tables[1]);
            //var grouped = jobList.GroupBy(m => m.Id).ToList();
            //model.docList = docList;

            return View(model);
        }
    }


    
}
