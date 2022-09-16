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
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
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
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            if (sp == null)
            {
                return HttpNotFound();
            }

            List<BinhLuan> listBL = db.BinhLuans.Where(n => n.MaSP == id).OrderByDescending(b=>b.NgayTao).ToList();
            List<TraLoiBinhLuan> listTraLoi = db.TraLoiBinhLuans.ToList();
            ViewBag.listTraLoi = listTraLoi;
            ViewBag.listBL = listBL;

            return View(sp);

        }

    
        [HttpPost]
        public ActionResult XemChitietSP(FormCollection f)
        {
            int id = Int32.Parse(f["MaSP"]);
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            if (tv != null)
            {
                BinhLuan newBL = new BinhLuan();
                newBL.MaSP = Int32.Parse(f["MaSP"]);
                newBL.NoiDungBL = f["NoiDungBL"];
                newBL.MaThanhVien = tv.MaThanhVien;
                newBL.NgayTao = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                db.BinhLuans.Add(newBL);
                db.SaveChanges();
                //string url = "<script> window.location.href = 'http://localhost:62979/SanPham/XemChiTietSP/" + id.ToString() + "'" + ";</script>";
                //return Content(url);
                return RedirectToAction("XemChitietSP", "SanPham", new { id = Int32.Parse(f["MaSP"]) });
                //'http://localhost:62979/SanPham/XemChiTietSP/'

            }
            else
            {
                List<BinhLuan> listBL = db.BinhLuans.Where(n => n.MaSP == id).OrderByDescending(b => b.NgayTao).ToList();
                ViewBag.listBL = listBL;
                ViewBag.ThongBao = "Vui lòng đăng nhập để bình luận";
                return View(sp);
                //return Content("Vui lòng đăng nhập để bình luận");


            }




        }

      


        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX, int? page)

        {
            //if (Session["TaiKhoan"] != "" && Session["TaiKhoan"] != null)
            //{

            //    return RedirectToAction("Index","Home");
            //}
            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var listSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX).ToList();
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


            return View(listSP.OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SanPhamNoiBat()
        {
            int n = 2;
            var listNoiBat = db.SanPhams.OrderByDescending(q => q.SoLanMua).Take(n) ;
            return PartialView(listNoiBat);
        }

       public ActionResult Menu2PartialView()
        {
            List<SanPham> listSP = db.SanPhams.Where(n => n.DaXoa == false).ToList();
            return PartialView(listSP);
        }


    }
}