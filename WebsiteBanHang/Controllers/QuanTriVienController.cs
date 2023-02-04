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
            Nullable< int> MaMatHang = shop.MaMatHang;
            ViewBag.listCTKD = db.ChiTietMatHangKinhDoanhs.Where(n => n.MaMatHang == MaMatHang).ToList();


            return View(shop);
        }
        public ActionResult TaoFlashSale()
        {
            return View();
        }

      

        public JsonResult TaoFlashSaleAjax(int? value, DateTime? datee, DateTime? end)
        {
            int? val = value == null ? value = null : value;
            DateTime? date = datee == null ? datee = null : datee;
            if(date != null && end != null)
            {
                FlashSale newfl = new FlashSale();
                newfl.NgaySale = date;
                newfl.EndTime = end;
                db.FlashSales.Add(newfl);
                db.SaveChanges();
            }
            else
            {
                DateTime now = (DateTime)DateTime.Now;
                FlashSale newfl = new FlashSale();
                for (int i=0; i< val;i++)
                {
                   
                    newfl.NgaySale = now;
                    newfl.EndTime = now.AddDays(1);
                    db.FlashSales.Add(newfl);
                    db.SaveChanges();
                    now = now.AddDays(1);


                }
            }

            return Json("alo");
        }

        public JsonResult XetDuyetShop(int MaShop)
        {
            Shop s = db.Shops.SingleOrDefault(n => n.MaShop == MaShop);
            if(s == null)
            {
                Response.StatusCode = 404;

            }
            else
            {
                s.XacNhan = true;
                return Json(new {mess = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { mess = "fail" }, JsonRequestBehavior.AllowGet);
        }

       
    }
}