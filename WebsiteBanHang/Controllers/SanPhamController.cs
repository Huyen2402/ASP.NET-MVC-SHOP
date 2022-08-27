using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using PagedList;

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
            return View(sp);
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
            var listNoiBat = db.SanPhams.Take(n);
            return PartialView(listNoiBat);
        }


    }
}