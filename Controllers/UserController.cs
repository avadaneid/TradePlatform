using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using FileProcessing;
using EntityFramework;

namespace TradingPlatform.Controllers
{

    [Authorize]
    public class UserController : Controller
    {    
        public static string username { get; set; }
        public static string password { get; set; }
        public ActionResult SesionCompany(LogInModel lm)
        {
            username = lm.UserName;
            password = lm.Password;

            IEnumerable<CompanyFinancialDetails> co = Context.CompanyFinancialDetails(username);
            Company company = Context.FindCompany(lm.UserName);

            ViewBag.CFD_List = co;
            ViewBag.Company = company;
            return View();
        }

        public ActionResult SesionIndividual(LogInModel lm)
        {
            return View(Context.FindListedCompanies(lm.UserName));
        }

        [HttpPost]
        public ActionResult UploadCompanyDetails(HttpPostedFileBase file)
        {
            if(file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
            }

            FileProcessing.FileProcessing.Read(username.ToString());

            return RedirectToAction("SesionCompany", new LogInModel { UserName = username.ToString() });
            
        }

        [HttpPost]
        public ActionResult ListCompany(Listing l)
        {
            Context.UpdateCompanyForListing(username, l);
            return RedirectToAction("SesionCompany", new LogInModel { UserName = username.ToString() });           
        }


        public ActionResult CompanyDetails(Company c)
        {
            MessageBox.Show($"{c.CUI}");
            return View();
        }

    }
}