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
        public ActionResult ThemSanPham( SanPham sp , HttpPostedFileBase HinhAnh, HttpPostedFileBase HinhAnh1, HttpPostedFileBase HinhAnh2, HttpPostedFileBase HinhAnh3)
        {

            // list nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.loaiSanPhams, "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX");



            // upload file hình ảnh

            
                if (HinhAnh.ContentLength > 0 && HinhAnh1.ContentLength > 0 && HinhAnh2.ContentLength > 0 && HinhAnh3.ContentLength > 0)
            {
                string fileName = Path.GetFileName(HinhAnh.FileName);
                string fileName1 = Path.GetFileName(HinhAnh1.FileName);
                string fileName2 = Path.GetFileName(HinhAnh2.FileName);
                string fileName3 = Path.GetFileName(HinhAnh3.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                string path1 = Path.Combine(Server.MapPath("~/Content/images"), fileName1);
                string path2 = Path.Combine(Server.MapPath("~/Content/images"), fileName2);
                string path3 = Path.Combine(Server.MapPath("~/Content/images"), fileName3);
                if (System.IO.File.Exists(path) && System.IO.File.Exists(path1) && System.IO.File.Exists(path2) && System.IO.File.Exists(path3))
                    {

                        ViewBag.Message = "File đã tồn tại";
                        return View();
                    }
                    else
                    {
                        HinhAnh.SaveAs(path);
                    HinhAnh1.SaveAs(path1);
                    HinhAnh2.SaveAs(path2);
                    HinhAnh3.SaveAs(path3);
                    sp.HinhAnh = fileName;
                    sp.HinhAnh1 = fileName1;
                    sp.HinhAnh2 = fileName2;
                    sp.HinhAnh3 = fileName3;

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


        [HttpGet]
        public ActionResult SuaSanPham(int? id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult SuaSanPham(FormCollection f)
        {
            return View();
        }
    }
}