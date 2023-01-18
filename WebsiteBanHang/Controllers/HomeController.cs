using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web.Hosting;

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
                Random random = new Random();
                ViewBag.ThongBao = "Thêm thành công";
                tv.MaLoaiTV = 1;
                tv.DaKhoa = false;
                tv.DaXacNhan = false;
                tv.captcha = random.Next(100000,999999);
                db.ThanhViens.Add(tv);
               
                db.SaveChanges();
                ViewBag.ThongBao = "Thêm thành công";
                try
                {
                    if (ModelState.IsValid)
                    {
                        var senderEmail = new MailAddress("huyenb1910384@student.ctu.edu.vn", "Huyen");
                        var receiverEmail = new MailAddress(tv.Email, "Receiver");
                        var password = "yyxrbzsfbkrftlny";
                        string subject = "Xác nhận tài khoản người dùng - Sàn thương mại điện tử Ori Cute";
                        string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Views/EmailTemplates/Customer.cshtml"));
                        body = body.Replace(@"######", @"Chào bạn, đây là mã xác nhận tài khoản của bạn: " + tv.captcha);
                     
                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail.Address, password)
                        };
                        using (var mess = new MailMessage(senderEmail, receiverEmail)
                        {
                            IsBodyHtml = true,
                            Subject = subject,
                            Body = body
                        })
                        {
                            smtp.Send(mess);
                        }
                        return RedirectToAction("XacNhanNguoiDung");
                    }
                }
                catch (Exception)
                {
                    ViewBag.Error = "Some Error";
                }
                return RedirectToAction("XacNhanNguoiDung");
            }
            else
            {
                ViewBag.ThongBao = "Thêm thất bại";
            }


            return View();
        }

        [HttpGet]
        public ActionResult XacNhanNguoiDung()
        {

            return View();
        }

       
        public JsonResult MaXacNhan(int captcha)
        {
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.captcha == captcha);
            if(tv != null)
            {
                tv.DaXacNhan = true;
                ViewBag.mess = "Xác nhận tài khoản thành công";
                ViewBag.status = "success";
                db.SaveChanges();
            }
            else
            {
                ViewBag.mess = "Mã xác nhận không hợp lệ";
                ViewBag.status = "faild";
            }
            return Json(new { mess = ViewBag.mess, status = ViewBag.status }, JsonRequestBehavior.AllowGet);
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
            if(tv != null && tv.MaLoaiTV == 1 && tv.DaXacNhan == true)
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
            List<loaiSanPham> listLSP = db.loaiSanPhams.Where(n => n.DaXoa == false).ToList();
            return View(listLSP);
        }

        public JsonResult DangKyCuaHang(string TenShop, string TaiKhoan, string MatKhau, string DiaChi, int idTinh, int idHuyen, int idXa, int[] arr)
        {
            ViewBag.Error = "";

           
                Shop checkTK = db.Shops.SingleOrDefault(n => n.TaiKhoan.Equals(TaiKhoan));
                Shop checkName = db.Shops.SingleOrDefault(n => n.TenShop.Equals(TenShop));
                Shop shop = new Shop();
            MatHangKinhDoanh kd = new MatHangKinhDoanh();
            kd.DayUpdate = (DateTime)DateTime.Now;
            db.MatHangKinhDoanhs.Add(kd);
            db.SaveChanges();
            if (checkTK == null && checkName == null)
                {
                    Random random = new Random();
                    shop.TaiKhoan = TaiKhoan;
                    shop.Pass = MatKhau;
                    shop.TenShop = TenShop;
                    shop.DiaChi = DiaChi;
                    shop.MaTinh = idTinh;
                    shop.MaHuyen = idHuyen;
                    shop.MaXa = idXa;
                    shop.NgayTao = (DateTime)DateTime.Now;
                    shop.XacNhan = false;
                    shop.captcha = random.Next(100000, 999999);
                    shop.MaMatHang = kd.MaMatHang;
                    db.Shops.Add(shop);
                    db.SaveChanges();
               
                for (int i =0;i< arr.Length; i++)
                {
                    ChiTietMatHangKinhDoanh ct = new ChiTietMatHangKinhDoanh();
                    ct.MaMatHang = kd.MaMatHang;
                    ct.MaLSP = arr[i];
                    db.ChiTietMatHangKinhDoanhs.Add(ct);
                    db.SaveChanges();
                }

               
                    return Json(new { status = true, MaShop = shop.MaShop }, JsonRequestBehavior.AllowGet);
                }
               else if(checkTK != null)
                {
                    return Json(new { status = false, text = "Tên tài khoản đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
               else if(checkName != null)
                {
                    return Json(new { status = false, text = "Tên cửa hàng đã tồn tại" }, JsonRequestBehavior.AllowGet);
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
