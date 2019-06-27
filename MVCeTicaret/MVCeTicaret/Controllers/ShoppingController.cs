using MVCeTicaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCeTicaret.Controllers
{
    public class ShoppingController : Controller
    {
        Context db;
        public ShoppingController()
        {
            db = new Context();
        }
        [HttpPost]
        public ActionResult AddToCart(int id, FormCollection frm)
        {
            if (Session["OnlineKullanici"] == null)
                return RedirectToAction("Login", "Login");

            int miktar = Convert.ToInt32(frm["quantity"]);

            //TODO:CustomerID dinamik hale getirilecek...
            OrderDetail od = db.OrderDetails.Where(x => x.ProductID == id && x.IsCompleted == false && x.CustomerID==3 ).FirstOrDefault();
            
            if (od==null)
            {
                od = new OrderDetail();
                od.CustomerID = 3;//TODO: Dinamik hale getirilecek....
                od.ProductID = id;
                od.OrderDate = DateTime.Now;
                od.IsCompleted = false;
                od.UnitPrice = db.Products.Find(id).UnitPrice;
                od.Discount = db.Products.Find(id).Discount;

                if (db.Products.Find(id).UnitInStock >= miktar)
                    od.Quantity = miktar;
                else
                    od.Quantity = db.Products.Find(id).UnitInStock;

                od.TotalAmount = od.Quantity * od.UnitPrice * (1 - od.Discount);
                db.OrderDetails.Add(od);
            }
            else
            {
               if(db.Products.Find(id).UnitInStock > od.Quantity + miktar)
                {
                    od.Quantity += miktar;
                    od.TotalAmount = od.Quantity * od.UnitPrice * (1 - od.Discount);
                }
            }
            db.SaveChanges();
            //TODO: CustomerID dinamik olarak çekilecek...
            return View(db.OrderDetails.Where(x=>x.CustomerID==3 && x.IsCompleted==false).ToList());
        }

        public ActionResult AddToWishList(int id)
        {
            WishList wl = db.WishLists.FirstOrDefault(x => x.ProductID == id && x.CustomerID == 3 && x.IsActive == true);
            if (wl == null)
            {
                wl = new WishList();
                wl.CustomerID = 3; //TODO: Dinamik hale getirilecek....
                wl.ProductID = id;
                wl.IsActive = true;

                db.WishLists.Add(wl);
                db.SaveChanges();
            }
           

            return View(db.WishLists.Where(x=>x.CustomerID==3 && x.IsActive==true).ToList());
        }
    }
}