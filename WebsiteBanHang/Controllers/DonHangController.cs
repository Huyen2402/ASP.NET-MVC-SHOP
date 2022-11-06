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
        public ActionResult Index(int? MaTV)
        {
            
           List<DonDatHang>  listdh = db.DonDatHangs.Where(n => n.MaKH == MaTV && n.MaTinhTrangGiaoHang == 5).ToList();

            return View(listdh);
        }

        public ActionResult DHDangVanChuyen()


        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            List<DonDatHang> listdh = db.DonDatHangs.Where(n => n.MaKH == tv.MaThanhVien && n.MaTinhTrangGiaoHang == 6).ToList();

            return View(listdh);
        }

        public ActionResult DHDaNhan()


        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            List<DonDatHang> listdh = db.DonDatHangs.Where(n => n.MaKH == tv.MaThanhVien && n.MaTinhTrangGiaoHang == 7).ToList();

            return View(listdh);
        }
    }
}