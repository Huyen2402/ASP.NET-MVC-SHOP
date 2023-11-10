using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using PagedList;
using System.Web.UI.WebControls;
using System.Web.Optimization;

namespace WebsiteBanHang.Controllers
{
    public class SanPhamController : Controller
    {
        Entities db = new Entities();
        // GET: SanPham
        public ActionResult SanPhamStyle1Partial(int? MaLSP)
        {

            if (MaLSP == null)
            {
                Response.StatusCode = 404;
            }

            List<SanPham> listSP = db.SanPhams.Where(m => m.MaLoaiSP == MaLSP && m.DaXoa == false).ToList();
          
            return PartialView(listSP);
        }

        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult XemChitietSP(int? id, int? MaShop)
        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            int? masp = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false && n.MaShop == MaShop);
            if (sp == null)
            {
                return HttpNotFound();
            }

            Random rnd = new Random();
            List<VideoQC> listVideo = db.VideoQCs.ToList();
            VideoQC lastVideo = listVideo.Last();
            int i = rnd.Next(1,lastVideo.IDVideo);
           
            VideoQC vd = db.VideoQCs.SingleOrDefault(n => n.IDVideo == i);
            if(vd != null)
            {
                ViewBag.Video = vd;

            }
            else
            {
                VideoQC vdLast = db.VideoQCs.SingleOrDefault(n => n.IDVideo == lastVideo.IDVideo);
                ViewBag.Video = vd;
            }
            ViewBag.IDUser = Session["idKH"] ;
            ViewBag.ListBL = db.BinhLuans.Where(x => x.MaSP == masp).OrderByDescending(b => b.NgayTao).ToList();
            ViewBag.ListTL = db.TraLoiBinhLuans.ToList();
            ViewBag.listGiamGia = db.GiamGias.Where(n=>n.MaShop == MaShop && n.SL > 0).ToList();
            ViewBag.shop = db.Shops.SingleOrDefault(n => n.MaShop == MaShop);
            ViewBag.listSize = db.KichCos.Where(n => n.MaSP == id).ToList();
            return View(sp);

        }




        [HttpPost]
        public ActionResult XemChitietSP(FormCollection f)
        {
            int id = Int32.Parse(f["MaSP"]);
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;

            return View();
        }

        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX, int? page, int? MaShop)

        {
            //if (Session["TaiKhoan"] != "" && Session["TaiKhoan"] != null)
            //{

            //    return RedirectToAction("Index","Home");
            //}
            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var listSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX && n.DaXoa == false && n.MaShop == MaShop).ToList();
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

            ViewBag.shop = db.Shops.SingleOrDefault(n => n.MaShop == MaShop);
            return View(listSP.OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SanPhamNoiBat()
        {
            
            var listNoiBat = db.SanPhams.OrderByDescending(q => q.SoLanMua);
            return PartialView(listNoiBat);
        }

        public ActionResult Menu2PartialView(int? MaShop)
        {
            ViewBag.MaShop = MaShop;
            List<SanPham> listSP = db.SanPhams.Where(n => n.DaXoa == false).ToList();
            return PartialView(listSP);
        }
        [HttpPost]
        public JsonResult ThemBinhLuan(BinhLuan bl)
        {

            bl.NgayTao = DateTime.Now;
           
            bl.DaXem = false;
            db.BinhLuans.Add(bl);
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == bl.MaSP);
            sp.LuotBinhLuan++;
            db.SaveChanges();

            return Json(new { status = true, TenThanhVien = (Session["TaiKhoan"] as ThanhVien).HoTen }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SuaBinhLuan(int MaBL, string noidung)
        {
            var binhluan = db.BinhLuans.Find(MaBL);
            binhluan.NoiDungBL = noidung;
            db.SaveChanges();

            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LuuGiamGia(int? MaGiamGia)
        {
            if (MaGiamGia == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                GiamGia gg = db.GiamGias.SingleOrDefault(n => n.MaGiamGia == MaGiamGia);
                if (gg == null)
                {
                    Response.StatusCode = 404;
                }
                else
                {
                    ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                    ChiTietGiamGia ctgg = new ChiTietGiamGia();
                    ctgg.MaGiamGia = MaGiamGia;
                    ctgg.MaThanhVien = tv.MaThanhVien;
                    ctgg.DaSuDung = false;
                    gg.SL--;
                    db.ChiTietGiamGias.Add(ctgg);
                    db.SaveChanges();
                    ViewBag.user = Session["TaiKhoan"];
                    ViewBag.listCTGG = db.ChiTietGiamGias.ToList();

                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListSanPham(int? MaLSP)
        {
            ViewBag.MaLSP = MaLSP;
            return View();
         
        }
        public ActionResult ViewAllFlashSale()
        {
            DateTime CurrentDay = DateTime.Now;
            //List<ChiTietFlashSale> listspsale = db.ChiTietFlashSales.ToList();
            //List<SanPham> listSp = new List<SanPham>();
            //foreach(var item in listspsale)
            //{
            //    SanPham sp = db.SanPhams.FirstOrDefault(n => n.MaSP == item.MaSP && n.DaXoa == false);
            //    listSp.Add(sp);
            //}
            //return View(listSp);
            List<SanPham> listSP = new List<SanPham>();

            List<FlashSale> listFlash = db.FlashSales.ToList();
            foreach (FlashSale flashSale in listFlash)
            {
                if (flashSale.NgaySale.Value.Date == CurrentDay.Date && flashSale.NgaySale.Value.Month == CurrentDay.Month && flashSale.NgaySale.Value.Year == CurrentDay.Year)
                {
                    ViewBag.Day = flashSale;

                    List<ChiTietFlashSale> list = db.ChiTietFlashSales.Where(n => n.MaSale == flashSale.MaSale).ToList();
                   
                    for (var i = 0; i < list.Count(); i++)
                    {
                        int? MaSP = list[i].MaSP;
                        SanPham sp = db.SanPhams.Single(n => n.MaSP == MaSP);
                        listSP.Add(sp);

                    }
                   
                }
            }
            return View(listSP);
        }
        public ActionResult SanPhamTuongTuPartial(int MaLSP)
        {

            List<SanPham> listDanhMuc = db.SanPhams.Where(n=>n.MaLoaiSP== MaLSP && n.DaXoa == false).Take(9).ToList();
            return PartialView(listDanhMuc);
        }

        public ActionResult DetailSanPham(int MaLSP) {
            loaiSanPham check = db.loaiSanPhams.SingleOrDefault(n=>n.MaLoaiSP== MaLSP);
            if(check == null)
            {
                Response.StatusCode = 404;
            }
            return PartialView();
        }

    }
}