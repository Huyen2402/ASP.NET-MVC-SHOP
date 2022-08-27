using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        
        public ActionResult Index()
        {
            var listKH = from kh in db.KhachHangs select kh;
            
            return View(listKH);
        }

        public ActionResult Index1()
        {
            var listKH = from kh in db.KhachHangs select kh;
            return View(listKH);
        }
    }
}