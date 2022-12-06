﻿using System;
using System.Collections.Generic;
using System.IO;
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

       public ActionResult CheckAvtShop(int? MaShop)
        {
            Shop s = db.Shops.SingleOrDefault(n => n.MaShop == MaShop);
            if(s != null)
            {
                ViewBag.MaShop = MaShop;
                if(s.avt != null)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("DangNhapCuaHang","Home");
        }

        [HttpPost]
        public ActionResult UpAvt(HttpPostedFileBase avt, Shop s)
        {
            int MaShop = s.MaShop;
            Shop check = db.Shops.SingleOrDefault(n=>n.MaShop== MaShop);
            if(check != null)
            {
                if (avt != null && avt.ContentLength > 0)
                {
                    //Get file name
                    var fileName = Path.GetFileName(avt.FileName);
                    //Get path
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);

                    //Check exitst
                    if (!System.IO.File.Exists(path))
                    {
                        //Add image into folder
                        avt.SaveAs(path);

                        check.avt = avt.FileName;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Admin");
                    }


                }
                else
                {
                    return RedirectToAction("DangNhapCuaHang", "Home");
                }
            }
          

            return RedirectToAction("DangNhapCuaHang", "Home");
        }

        public JsonResult NewProduct(int? MaShop)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<SanPham> listNew = db.SanPhams.Where(x=>x.MaShop == MaShop && x.DaXoa == false).OrderByDescending(n => n.NgayCapNhat).ToList();
            
            //return Json( new { status = true,  } , JsonRequestBehavior.AllowGet);
            return Json(listNew, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BestSell(int? MaShop)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<SanPham> bestSell = db.SanPhams.Where(x => x.MaShop == MaShop && x.DaXoa == false).OrderByDescending(n => n.SoLanMua).ToList();

            //return Json( new { status = true,  } , JsonRequestBehavior.AllowGet);
            return Json(bestSell, JsonRequestBehavior.AllowGet);
        }

        public JsonResult filterProduct(int MaShop, int val)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if(val == 1)
            {
                List<SanPham> listFilter = db.SanPhams.Where(x => x.DaXoa == false && x.MaShop == MaShop).OrderBy(n => n.DonGia).ToList();
                return Json(listFilter, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<SanPham> listFilter = db.SanPhams.Where(x => x.DaXoa == false && x.MaShop == MaShop).OrderByDescending(n => n.DonGia).ToList();
                return Json(listFilter, JsonRequestBehavior.AllowGet);
            }
        }
    }
}