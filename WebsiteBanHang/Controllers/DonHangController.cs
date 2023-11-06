using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class DonHangController : Controller
    {
        Entities db = new Entities();
        // GET: DonHang

        public ActionResult buttonDonHangPartial()
        {

            return PartialView();
        }
        public ActionResult Index()
        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            List<DonDatHang>  listdh = db.DonDatHangs.Where(n => n.MaKH == tv.MaThanhVien && n.MaTinhTrangGiaoHang == 1).OrderByDescending(p=>p.NgayDat).ToList();


            return View(listdh);
        }

        public ActionResult DHDangVanChuyen()


        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            List<DonDatHang> listdh = db.DonDatHangs.Where(n => n.MaKH == tv.MaThanhVien && n.MaTinhTrangGiaoHang == 2).ToList();

            return View(listdh);
        }

        public ActionResult DHDaNhan()


        {

            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            List<DonDatHang> listdh = db.DonDatHangs.Where(n => n.MaKH == tv.MaThanhVien && n.MaTinhTrangGiaoHang == 3).ToList();

            return View(listdh);
        }

        public ActionResult XacNhanDonHang(string MaDDH)
        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            ThanhVien check = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == tv.MaThanhVien);
            if(check == null)
            {
                Response.StatusCode = 404;
            }
            DonDatHang dhh = db.DonDatHangs.SingleOrDefault(n => n.MaDDH.Equals(MaDDH));
            if(dhh == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                dhh.MaTinhTrangGiaoHang = 3;
                ThongBaoDH newTB = new ThongBaoDH();
                newTB.MaTV = tv.MaThanhVien;
                newTB.MaDH = MaDDH;
                newTB.DaXem = false;
                newTB.ThoiGian = DateTime.Now;
                db.ThongBaoDHs.Add(newTB);
                check.TichDiem = (dhh.TongTienThucTe / 10000) + check.TichDiem;

                db.SaveChanges();
            }
            return RedirectToAction("DHDangVanChuyen", "DonHang");
        }

        public JsonResult HuyDonHang(string MaDDH)
        {

            if (MaDDH == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                
                DonDatHang ddh = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == MaDDH);
                ddh.MaTinhTrangGiaoHang = 4;
                db.SaveChanges();
                try
                {
                    if (ModelState.IsValid)
                    {
                        string email = ddh.ThanhVien.Email;
                        string tinhteang = ddh.TinhTrangGiaoHang.TenTinhTrang;
                        var senderEmail = new MailAddress("huyenb1910384@student.ctu.edu.vn", "Huyen");
                        var receiverEmail = new MailAddress(email, "Receiver");
                        var password = "yyxrbzsfbkrftlny";
                        var sub = "Thông báo tình trạng đơn hàng - Đơn hàng" + tinhteang;
                        var body = "Đơn hàng của bạn vừa bị hủy. ";
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
                            Subject = sub,
                            Body = body
                        })
                        {
                            smtp.Send(mess);
                        }
                    }
                }
                catch (Exception)
                {
                    ViewBag.Error = "Some Error";
                }

            }
            
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult XemChiTietDonHang(string MaDDH)
        {

            List<ChiTietDonDatHang> ctddh = db.ChiTietDonDatHangs.Where(n => n.MaDDH == MaDDH).ToList();
           
            ViewBag.MaDDH = MaDDH;
            
            return View(ctddh);
        }
        public ActionResult XemChiTietDH(string MaDDH)
        {

            List<ChiTietDonDatHang> ctddh = db.ChiTietDonDatHangs.Where(n => n.MaDDH == MaDDH).ToList();
            DonDatHang ddh  = db.DonDatHangs.SingleOrDefault(n=>n.MaDDH == MaDDH);
            ChiTietDonDatHang c = db.ChiTietDonDatHangs.First(n => n.MaDDH == MaDDH);
            KichCo k = db.KichCos.First(n => n.MaSP == c.MaSP);
            ViewBag.k = k.Ten;
            ViewBag.MaDDH = MaDDH;

            return View(ctddh);
        }
        public JsonResult evaluateProduct( int MaCTDDH, string comment, int start)
        {

            ChiTietDonDatHang ct = db.ChiTietDonDatHangs.SingleOrDefault(n => n.MaChiTietDDH1 == MaCTDDH);
            if (ct == null)
            {
                Response.StatusCode = 404;

            }
            ct.DaDanhGia = true;
            BinhLuan bl = new BinhLuan();
            bl.NoiDungBL = comment;
            bl.MaCTDDH = MaCTDDH;
            bl.MaSP = ct.MaSP;
            bl.DaXem = false;
            bl.DanhGia = start;
            bl.NgayTao = DateTime.Now;
            db.BinhLuans.Add(bl);
            db.SaveChanges();

            List<BinhLuan> listCT = db.BinhLuans.Where(n => n.MaSP == ct.MaSP && n.DanhGia != 0).ToList();
            double? sumStar = 0;
            for (int j = 0; j < listCT.Count(); j++)
            {
                sumStar += listCT[j].DanhGia;
            }
            double? tbStar = sumStar / listCT.Count();
            SanPham sp = db.SanPhams.SingleOrDefault(x => x.MaSP == ct.MaSP);
            sp.DanhGia = tbStar;
            db.SaveChanges();



            return Json(new { mess = "success" }, JsonRequestBehavior.AllowGet);
          
        }
        public ActionResult DHDangGiao(string MaDDH)
        {
            List<ChiTietDonDatHang> ctddh = db.ChiTietDonDatHangs.Where(n => n.MaDDH == MaDDH).ToList();
            DonDatHang ddh = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == MaDDH);
            ChiTietDonDatHang c = db.ChiTietDonDatHangs.First(n => n.MaDDH == MaDDH);
            KichCo k = db.KichCos.First(n => n.MaSP == c.MaSP);
            ViewBag.k = k.Ten;
            ViewBag.MaDDH = MaDDH;

            return View(ctddh);

        }
        public ActionResult DHDaHuy()
        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            List<DonDatHang> listdh = db.DonDatHangs.Where(n => n.MaKH == tv.MaThanhVien && n.MaTinhTrangGiaoHang == 4).OrderByDescending(p => p.NgayDat).ToList();


            return View(listdh);
        }



    }
}