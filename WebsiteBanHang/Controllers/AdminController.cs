using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class AdminController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult XemSanPham()
        {

            List<SanPham> list = db.SanPhams.ToList();
            return View(list);
        }


        [HttpGet]
        public ActionResult ThemSanPham()
        {

            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.loaiSanPhams, "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX");
            return View();
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemSanPham( SanPham sp , HttpPostedFileBase HinhAnh)
        {

            // list nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.loaiSanPhams, "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaCungCaps, "MaNSX", "TenNSX");

            

            // upload file hình ảnh
            if (HinhAnh.ContentLength > 0)
            {
                string fileName = Path.GetFileName(HinhAnh.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                
                if(System.IO.File.Exists(path))
                {

                    ViewBag.Message = "File đã tồn tại";
                    return View();
                }
                else
                {
                    HinhAnh.SaveAs(path);
                    sp.HinhAnh = fileName;
                    
                }


                db.SanPhams.Add(sp);
                db.SaveChanges();

                return RedirectToAction("XemSanPham", "Admin");

            }
            else
            {
                ViewBag.Message = "Có lỗi";
            }
            return View();

            

        }
    }
}