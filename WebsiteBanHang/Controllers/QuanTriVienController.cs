using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class QuanTriVienController : Controller
    {
        Entities db = new Entities();
        // GET: QuanTriVien
        public ActionResult Index()
        {
            int? XetDuyet = db.Shops.Where(n=>n.XacNhan == false).ToList().Count;
            int? TongShop = db.Shops.Where(n=>n.XacNhan == true).ToList().Count;
            int? TongDM = db.loaiSanPhams.Where(n=>n.DaXoa == false).ToList().Count;
            int? TongNCC = db.NhaCungCaps.ToList().Count;
            int?  NSX = db.NhaSanXuats.ToList().Count();
            ViewBag.Xet = XetDuyet;
            ViewBag.TongShop = TongShop;
            ViewBag.TongDM = TongDM;
            ViewBag.TongNCC = TongNCC;
            ViewBag.TongNSX = NSX;
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ThanhVien tv)
        {
            ThanhVien check = db.ThanhViens.SingleOrDefault(n=>n.Email == tv.Email && n.MatKhau == tv.MatKhau && n.MaLoaiTV == 2) ;
            if(check == null)
            {

                ViewBag.Mess = "Sai tài khoản hoặc mật khẩu";
                return View();
            }
            ViewBag.Mess = "";
            return RedirectToAction("Index");
        }
        public ActionResult ChoXetDuyet()
        {
            List<Shop> listShop = db.Shops.Where(n => n.XacNhan == false).ToList();
            return View(listShop);
        }
        public ActionResult ThongTinShop(int MaShop)
        {
            Shop shop = db.Shops.SingleOrDefault(n => n.MaShop == MaShop);
            return View(shop);
        }
        public ActionResult TaoFlashSale()
        {
            return View();
        }
    }
}