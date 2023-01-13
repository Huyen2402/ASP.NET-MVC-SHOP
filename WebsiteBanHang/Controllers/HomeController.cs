using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.IO;

namespace WebsiteBanHang.Controllers
{
    public class HomeController : Controller
    {
        Entities db = new Entities();
        // GET: Home
        public ActionResult Index()
        {
            // Lấy list điện thoại
            int numberDT = 4;
            var listDT = db.SanPhams.Where(b=>b.DaXoa == false).OrderByDescending(n => n.NgayCapNhat).Take(numberDT);
            ViewBag.listDT = listDT;
            //Lấy list máy tính bảng
            var listMTB = db.SanPhams.Where(n => n.MaLoaiSP == 1);
            ViewBag.listMTB = listMTB;

            //lấy list Laptop
            int numberLT = 6;
            var listLT = db.SanPhams.Where(b=>b.DaXoa == false).OrderByDescending(x=>x.DonGia).Take(numberLT);
            ViewBag.listLT = listLT;


            return View();
        }
       
        public ActionResult MenuPartial()
        {
            var listLSP = db.loaiSanPhams.Where(n=>n.DaXoa == false).ToList();
            return PartialView(listLSP);
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            
            ViewBag.MaTinh = new SelectList(db.Tinhs, "MaTinh", "TenTinh");
            ViewBag.MaHuyen = new SelectList(db.Huyens, "MaHuyen", "TenHuyen");
            ViewBag.MaXa = new SelectList(db.Xas, "MaXa", "TenXa");
            ViewBag.CauHoi = new SelectList(CauHoi());
            return View();

        }

        public ActionResult LayHuyen(int idTinh)
        {
            List<Huyen> listHuyen = db.Huyens.Where(n => n.MaTinh == idTinh).ToList();
            ViewBag.MaHuyen = new SelectList(listHuyen, "MaHuyen", "TenHuyen");
            return PartialView("LayHuyen");
        }

        public ActionResult LayXa(int idHuyen)
        {
            List<Xa> listXa = db.Xas.Where(n => n.MaHuyen == idHuyen).ToList();
            ViewBag.MaXa = new SelectList(listXa, "MaXa", "TenXa");
            return PartialView("LayXa");
        }

        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            ViewBag.MaTinh = new SelectList(db.Tinhs, "MaTinh", "TenTinh");

            ViewBag.CauHoi = new SelectList(CauHoi());
            if(this.IsCaptchaValid("Captcha is not valid"))
            {
                ViewBag.ThongBao = "Thêm thành công";
                tv.MaLoaiTV = 2;
                tv.DaKhoa = false;
                db.ThanhViens.Add(tv);
               
                db.SaveChanges();
                ViewBag.ThongBao = "Thêm thành công";
                return View();
            }
            else
            {
                ViewBag.ThongBao = "Thêm thất bại";
            }

            
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
            if(tv != null && tv.MaLoaiTV == 1)
            {
                Session["TaiKhoan"] = tv;
                Session["idKH"] = tv.MaThanhVien;
                Session["MaTinh"] = tv.MaTinh;
                Session["MaHuyen"] = tv.MaHuyen;
                Session["MaXa"] = tv.MaXa;
                return Content(" <script>window.location.href = 'http://localhost:62979/Home/Index';</script>");
            }
            if(tv != null && tv.MaLoaiTV == 3)
            {
                Session["TaiKhoan"] = tv;
                Session["Quyen"] = "Admin";
                return Content(" <script>window.location.href = 'http://localhost:62979/Admin/Index';</script>");
            }
            return Content("Sai tài khoản hoặc mật khẩu");
        }
        
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CapNhatThongTin(int? MaTV)
        {
            

            // tìm tên trong csdl
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n=>n.MaThanhVien == MaTV);
            ViewBag.MaTinh = new SelectList(db.Tinhs, "MaTinh", "TenTinh", tv.MaTinh);
           
            if (tv == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                return View(tv);
            }
            return View();
        }

        [HttpPost]
        public ActionResult CapNhatThongTin(ThanhVien tv)
        {
            if (tv == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                ThanhVien check = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == tv.MaThanhVien);
                if (check == null)
                {
                    Response.StatusCode = 404;
                }
                else
                {
                    check.MaThanhVien = tv.MaThanhVien;
                    check.HoTen = tv.HoTen;
                    check.SDT = tv.SDT;
                    check.MaHuyen = tv.MaHuyen;
                    check.MaTinh = tv.MaTinh;
                    check.MaXa = tv.MaXa;
                    check.MatKhau = tv.MatKhau;
                    check.DiaChi = tv.DiaChi;
                    check.DaKhoa = false;
                    db.SaveChanges();
                   
                    return RedirectToAction("Index","Home");

                }

            }
            return View(tv);

        }

        public ActionResult ProductFlashSale()
        {
            DateTime CurrentDay = DateTime.Now;
            List<FlashSale> listFlash = db.FlashSales.ToList();
            foreach (FlashSale flashSale in listFlash)
            {
                if (flashSale.NgaySale.Value.Date <= CurrentDay.Date && flashSale.NgaySale.Value.Month <= CurrentDay.Month && flashSale.NgaySale.Value.Year <= CurrentDay.Year)
                {
                    ViewBag.Day = flashSale;
                   
                    List<ChiTietFlashSale> list = db.ChiTietFlashSales.Where(n=>n.MaSale == flashSale.MaSale).ToList();
                    List<SanPham> listSP = new List<SanPham>();
                    for (var i =0; i<list.Count(); i++)
                    {
                        int? MaSP = list[i].MaSP;
                       SanPham sp = db.SanPhams.Single(n=>n.MaSP== MaSP);
                        listSP.Add(sp);
                       
                    }
                    return PartialView(listSP);
                }
            }

            return PartialView(null);

        }

        public ActionResult SanPhamGoiY()
        {

            List<SanPham> listSP = db.SanPhams.Where(n=>n.DaXoa == false).Take(20).ToList();
            return PartialView(listSP);
        }

        [HttpGet]
        public ActionResult DangKyCuaHang()
        {

            ViewBag.MaTinh = new SelectList(db.Tinhs, "MaTinh", "TenTinh");
            ViewBag.MaHuyen = new SelectList(db.Huyens, "MaHuyen", "TenHuyen");
            ViewBag.MaXa = new SelectList(db.Xas, "MaXa", "TenXa");
            return View();
        }

        public JsonResult DangKyCuaHang(Shop shop)
        {
            ViewBag.Error = "";

            if (shop != null)
            {
                Shop checkTK = db.Shops.SingleOrDefault(n => n.TaiKhoan.Equals(shop.TaiKhoan));
                Shop checkName = db.Shops.SingleOrDefault(n => n.TenShop.Equals(shop.TenShop));
               
               if (checkTK == null && checkName == null)
                {
                    shop.NgayTao = (DateTime)DateTime.Now;
                    shop.XacNhan = false;
                    db.Shops.Add(shop);
                    db.SaveChanges();
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
               else if(checkTK != null)
                {
                    return Json(new { status = false, text = "Tên tài khoản đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
               else if(checkName != null)
                {
                    return Json(new { status = false, text = "Tên cửa hàng đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
              
               
               
                
            }
          
                return Json(new { status = false, text = "Có lỗi xảy ra" }, JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult DangNhapCuaHang()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapCuaHang(string TaiKhoan, string Pass)
        {

            Shop checkShop = db.Shops.SingleOrDefault(n => n.TaiKhoan.Equals(TaiKhoan) && n.Pass.Equals(Pass));
            if (checkShop == null){
                Response.StatusCode = 404;
            }
    
            Session["CuaHang"] = checkShop;
          
            return RedirectToAction("CheckAvtShop", "Shop", new {@MaShop = checkShop.MaShop});
        }

        public ActionResult ModalLoadPartial()
        {
            return PartialView();
        }



    }
}
