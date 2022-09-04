using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;

namespace WebsiteBanHang.Controllers
{
    public class HomeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Home
        public ActionResult Index()
        {
            // Lấy list điện thoại
            int numberDT = 4;
            var listDT = db.SanPhams.OrderByDescending(n => n.NgayCapNhat).Take(numberDT);
            ViewBag.listDT = listDT;
            //Lấy list máy tính bảng
            var listMTB = db.SanPhams.Where(n => n.MaLoaiSP == 1);
            ViewBag.listMTB = listMTB;

            //lấy list Laptop
            int numberLT = 6;
            var listLT = db.SanPhams.OrderByDescending(x=>x.DonGia).Take(numberLT);
            ViewBag.listLT = listLT;


            return View();
        }
       
        public ActionResult MenuPartial()
        {
            var listSP = db.SanPhams.Where(n=>n.DaXoa == false).ToList();
            return PartialView(listSP);
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(CauHoi());
            return View();

        }

        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {

            ViewBag.CauHoi = new SelectList(CauHoi());
            if(this.IsCaptchaValid("Captcha is not valid"))
            {
                ViewBag.ThongBao = "Thêm thành công";
                db.ThanhViens.Add(tv);
               
                db.SaveChanges();
                return View();
            }

            ViewBag.ThongBao = "Thêm thất bại";
            return View();
        }

        public List<string> CauHoi()
        {
            List<string> list = new List<string>();
            list.Add("Màu yêu thích của bạn là gì?");
            list.Add("Môn học thích của bạn là gì?");
            list.Add("Bạn thích học môn gì nhất?");
            return list;
        }
        
       
        [HttpPost]
        public ActionResult DangNhap(string TaiKhoan, string MatKhau)
        {
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == TaiKhoan && n.MatKhau == MatKhau );
            if(tv != null && tv.MaLoaiTV == 2)
            {
                Session["TaiKhoan"] = tv;
                Session["idKH"] = tv.MaThanhVien;
                return Content(" <script>window.location.href = 'http://localhost:62979/Home/Index';</script>");
            }
            if(tv != null && tv.MaLoaiTV == 1)
            {
                Session["TaiKhoan"] = tv;
                Session["Quyen"] = "Admin";
                return Content(" <script>window.location.href = 'http://localhost:62979/Admin/XemSanPham';</script>");
            }
            return Content("Sai tài khoản hoặc mật khẩu");
        }
        
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index");
        }


        

    }
}
