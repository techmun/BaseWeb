
using BaseWeb.Data;
using BaseWeb.Migrations;
using BaseWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using static BaseWeb.Utilities.ActionFilterConfig;

namespace BaseWeb.Controllers
{
    public class HomeController : Controller
    {
        
        [Authentication]
        [NoDirectAccess]
        public IActionResult Index()
        {
            var model = new List<ExpDateListViewModel>();
            using(var context = new AppDbContext())
            {
                var grpExp = (from ed in context.ExpiryDate
                              join cp in context.CustProf on ed.CustCode equals cp.CustCode
                              select new { ed, cp.CustName }).OrderBy(m => m.ed.ExpiredDate).ToList();
                              //group ed by new { month = ed.ExpiredDate.Month, year = ed.ExpiredDate.Year } into d
                              //select new { dt = string.Format("{0}/{1}", d.Key.month, d.Key.year) }).ToList();

                var exp = grpExp.Select(m => new { dt = string.Format("{0}/{1}", m.ed.ExpiredDate.Month, m.ed.ExpiredDate.Year)}).GroupBy(m=>m.dt).ToList();

                foreach (var d in exp)
                {
                    var ex = new ExpDateListViewModel();
                    ex.month = d.Key;
                    var date = d.Key.Split('/');
                    var month = date[0];
                    var year = date[1];

                    var expList = grpExp.Where(m => m.ed.ExpiredDate.Month.ToString() == month && m.ed.ExpiredDate.Year.ToString() == year).ToList();
                    var exDateList = new List<ExpiryDateViewModel>();
                    
                    foreach (var ed in expList)
                    {
                        var expListVM = new ExpiryDateViewModel()
                        {
                            CustCode = ed.ed.CustCode,
                            CustName = ed.CustName,
                            ExpiredDate = ed.ed.ExpiredDate,
                            Description = ed.ed.Description,
                            DocType = ed.ed.DocType
                        };
                        exDateList.Add(expListVM);

                    }
                    ex.exDateList = exDateList;
                    model.Add(ex);
                }
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}