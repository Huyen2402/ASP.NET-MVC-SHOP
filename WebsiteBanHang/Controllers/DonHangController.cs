using System;
using System.Collections.Generic;
using System.Linq;
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
            DonDatHang dhh = db.DonDatHangs.SingleOrDefault(n => n.MaDDH.Equals(MaDDH));
            if(dhh == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                dhh.MaTinhTrangGiaoHang = 3;
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
                List<ChiTietDonDatHang> listCT = db.ChiTietDonDatHangs.Where(n => n.MaDDH == MaDDH).ToList();
                for (var i = 0; i < listCT.Count(); i++)
                {
                    db.ChiTietDonDatHangs.Remove(listCT[i]);
                }
                DonDatHang ddh = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == MaDDH);
                db.DonDatHangs.Remove(ddh);


            }
            db.SaveChanges();
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult XemChiTietDonHang(string MaDDH)
        {

            List<ChiTietDonDatHang> ctddh = db.ChiTietDonDatHangs.Where(n => n.MaDDH == MaDDH).ToList();
           
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



            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
          
        }


    }
}