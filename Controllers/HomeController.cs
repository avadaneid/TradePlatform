﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using EntityFramework;
using Validation;
using System.Web.Security;
using System.Windows.Forms;

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
            if (TempData["UserExists"] != null)
            {
                ModelState.AddModelError("UserExists", TempData["UserExists"].ToString());
            }
            return View();
        }    

        [HttpPost]
        public ActionResult InsertUser(Account sign)
        {
            if (Context.VerifyUserName(sign))
            {
                Context.InsertLogIn(sign);
                return RedirectToAction("LogIn");
            }
            else
            {
                TempData["UserExists"] = "Username already exists! Choose another username !";
                return RedirectToAction("SignUp");
            }
           
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("LogIn","Home",null);
        }


    }
}