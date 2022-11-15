using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        Entities db= new Entities();
        public ActionResult XemShop(int? MaShop)
        {
            if(MaShop == null)
            {
                Response.StatusCode = 404;
            }
           Shop shop = db.Shops.SingleOrDefault(n=>n.MaShop == MaShop);
            ViewBag.list = db.SanPhams.Where(n => n.MaShop == MaShop && n.DaXoa == false).ToList();

            return View(shop);
        }

       
    }
}