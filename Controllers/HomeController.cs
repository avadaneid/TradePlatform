using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using EntityFramework;
using Validation;
using System.Web.Security;

namespace TradingPlatform.Controllers
{
    public class HomeController : Controller
    {
        #region Default
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        [HttpPost]
        public ActionResult Authenticate(LogInModel lim)
        {
            LogInValidation.Validate(lim.UserName, lim.Password);
            if (!LogInValidation.ValidCredentials)
            {

                TempData["CustomError"] = "Username or password invalid !";
                return RedirectToAction("LogIn");
            }
            else
            {
                FormsAuthentication.SetAuthCookie(lim.UserName, false);

                if (LogInValidation.AccountType == AccountType.Company)
                {
                    return RedirectToAction("SesionCompany", "User", lim);
                }
                else
                {
                    return RedirectToAction("SesionIndividual", "User", lim);
                }
            }
        }

        public ActionResult LogIn()
        {
            if (TempData["CustomError"] != null)
            {
                ModelState.AddModelError("CustomError", TempData["CustomError"].ToString());
            }
            return View();
        }    

        public ActionResult SignUp()
        {
            return View();
        }    

        [HttpPost]
        public ActionResult InsertUser(Account sign)
        {          
            Context.InsertLogIn(sign);
            return RedirectToAction("LogIn"); 
        }



    }
}