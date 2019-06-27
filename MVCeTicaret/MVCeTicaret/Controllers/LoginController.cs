using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCeTicaret.Models;

namespace MVCeTicaret.Controllers
{
    public class LoginController : Controller
    {
        Context db;
        public LoginController()
        {
            db = new Context();
        }

        [HttpGet]
        public ActionResult Login()
        {
            TemporaryUserData.ReturnUrl = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection frm)
        {
            string kullaniciAdi = frm["username"];
            string sifre = frm["password"];

            Customer customer = db.Customers.FirstOrDefault(x => x.UserName == kullaniciAdi && x.Password == sifre);

            if (customer != null)
            {
                Session["OnlineKullanici"] = customer.UserName;
                TemporaryUserData.UserID = customer.CustomerID;

                return Redirect(TemporaryUserData.ReturnUrl);
            }

            return View();
        }

        public ActionResult Logout()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection frm)
        {
            return View();
        }
    }
}