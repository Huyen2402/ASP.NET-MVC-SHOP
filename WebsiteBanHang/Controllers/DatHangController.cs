using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class DatHangController : Controller
    {
        // GET: DatHang
        [HttpGet]
        public ActionResult DatHang(int id)
        {
            QuanLyBanHangEntities db = new QuanLyBanHangEntities();
            List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];
            int iduser = (int)Session["idKH"];
            if (listGioHang == null)
            {
                return null;
            }
            else
            {
                DonDatHang ddh = new DonDatHang();
                string ngay = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                ddh.NgayDat = DateTime.Parse(ngay);
                ddh.MaTinhTrangGiaoHang = 5;
                if(id == 1)
                {
                    ddh.HinhThucThanhToan = "COD";
                    ddh.DaThanhToan = false;
                }
                if(id == 2)
                {
                    ddh.HinhThucThanhToan = "MoMo";
                    ddh.DaThanhToan = true;
                }
                if(id == 3)
                {
                    ddh.HinhThucThanhToan = "VNPay";
                    ddh.DaThanhToan = true;
                }
                ddh.UuDai = 0;
                ddh.MaKH = iduser;

                db.DonDatHangs.Add(ddh);

                db.SaveChanges();

                for (var i = 0; i < listGioHang.Count(); i++)
                {
                    ChiTietDonDatHang ct = new ChiTietDonDatHang();
                    ct.MaDDH = ddh.MaDDH;
                
                    ct.MaSP = listGioHang[i].MaSP;
                    ct.TenSP = listGioHang[i].TenSP;
                    ct.SoLuong = listGioHang[i].SoLuong;
                    ct.DonGia = listGioHang[i].Dongia;
                    db.ChiTietDonDatHangs.Add(ct);
                    

                }

                db.SaveChanges();




            }
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang","GioHang");
        }
    }
}