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
using Newtonsoft.Json;
using Validation;

namespace TradingPlatform.Controllers
{

    [Authorize]
    public class UserController : Controller
    {    
        public static string username { get; set; }
        public static string password { get; set; }
        public LogInModel LogInModel { get; set; }
        public ActionResult SesionCompany(LogInModel lm)
        {
            username = lm.UserName;
            password = lm.Password;

            IEnumerable<CompanyFinancialDetails> co = Context.FindCompanyFinancialDetails(username);
            Company company = Context.FindCompany(lm.UserName);

            ViewBag.CFD_List = co;
            ViewBag.Company = company;
            return View();
        }

        public ActionResult SesionIndividual(LogInModel lm)
        {
            IEnumerable<Company> comp = Context.FindListedCompanies();
            ViewBag.ListedCompanies = comp;          

            if (lm.UserName != null)
            {
                TempData["LoginModel"] = lm;
            }
           
            Individual individual = Context.FindIndividual(lm.UserName);

            return View(individual);
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
  
        public ActionResult CompanyDetails(Transaction t)
        {    
            Company company = Context.FindCompany(t.CUI);
            ViewBag.ASK = Context.FindASKforListing(t.CUI);
            ViewBag.BID = Context.FindBIDforListing(t.CUI);
            ViewBag.Transaction = t;
            return View();
        }
      
        [HttpPost]
        public void BUY(Transaction t)
        {
            TransactionValidation.ValidateBUY(t);          
        }

        [HttpPost]
        public void SELL(Transaction t)
        {
            TransactionValidation.ValidateSELL(t);         
        }

        public ActionResult UpdateBID(Term Term)
        {        
            Context.UpdateBID(Term);
            return RedirectToAction("SesionIndividual", TempData["LoginModel"]);
        }


        public ActionResult UpdateASK(Term Term)
        {
            //Context.UpdateASK(Term);          
            return RedirectToAction("SesionIndividual", TempData["LoginModel"]);
        }

        public PartialViewResult RenderGraph(long cui)
        {
            ViewBag.Cui = cui;
            return PartialView("RenderGraph");
        }

        public ActionResult GraphData(long cui)
        {
            List<TransactionReport> tr = Context.TransactionReport(cui);
            return Json(tr.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult PartialViewBID(long cnp)
        {
            List<BID> lstBD = Context.FindBIDsForIndividual(cnp);
            ViewBag.BIDList = lstBD;
            return PartialView("PartialViewBID");
        }


        public PartialViewResult PartialViewASK(long cnp)
        {
            List<ASK> lstask = Context.FindASKsForIndividual(cnp);
            ViewBag.ASKList = lstask;
            return PartialView("PartialViewASK");
        }

        public ActionResult PortofolioGraph(long cnp)
        {                        
            List<Portofolio> port = Context.FindPortofolios(cnp);

            return Json(port.ToArray(), JsonRequestBehavior.AllowGet);           
        }
    }
}