using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using PagedList;
using System.Web.UI.WebControls;
using System.Web.Optimization;

namespace WebsiteBanHang.Controllers
{
    public class SanPhamController : Controller
    {
        Entities db = new Entities();
        // GET: SanPham
        public ActionResult SanPhamStyle1Partial()
        {
            return PartialView();
        }

        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult XemChitietSP(int? id)
        {
            int? masp = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            if (sp == null)
            {
                return HttpNotFound();
            }

            ViewBag.ListBL = db.BinhLuans.Where(x => x.MaSP == masp).OrderByDescending(b=>b.NgayTao).ToList();
            ViewBag.ListTL = db.TraLoiBinhLuans.ToList();

            return View(sp);

        }


        [HttpPost]
        public ActionResult XemChitietSP(FormCollection f)
        {
            int id = Int32.Parse(f["MaSP"]);
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;

            return View();
        }

        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX, int? page, int? MaShop)

        {
            //if (Session["TaiKhoan"] != "" && Session["TaiKhoan"] != null)
            //{

            //    return RedirectToAction("Index","Home");
            //}
            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var listSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX && n.DaXoa == false && n.MaShop == MaShop).ToList();
            if (listSP.Count() == 0)
            {
                return HttpNotFound();
            }

            //phan trang
            int pageSize = 6;
            int pageNumber = 1;
            if (page != null)
            {
                pageNumber = page.Value;
            }
           
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;

            ViewBag.shop = db.Shops.SingleOrDefault(n=>n.MaShop == MaShop);
            return View(listSP.OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SanPhamNoiBat()
        {
            int n = 2;
            var listNoiBat = db.SanPhams.OrderByDescending(q => q.SoLanMua).Take(n);
            return PartialView(listNoiBat);
        }

        public ActionResult Menu2PartialView(int? MaShop)
        {
            ViewBag.MaShop = MaShop;    
            List<SanPham> listSP = db.SanPhams.Where(n => n.DaXoa == false).ToList();
            return PartialView(listSP);
        }
        [HttpPost]
        public JsonResult ThemBinhLuan(BinhLuan bl)
        {

            bl.NgayTao = DateTime.Now;
            bl.MaThanhVien = (Session["TaiKhoan"] as ThanhVien).MaThanhVien;
            db.BinhLuans.Add(bl);
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == bl.MaSP);
            sp.LuotBinhLuan++;
            db.SaveChanges();

            return Json(new { status = true, TenThanhVien = (Session["TaiKhoan"] as ThanhVien).HoTen }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SuaBinhLuan(int MaBL, string noidung)
        {
            var binhluan = db.BinhLuans.Find(MaBL);
            binhluan.NoiDungBL = noidung;
            db.SaveChanges();

            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }
    }
}