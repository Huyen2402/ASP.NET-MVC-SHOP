using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class GioHangController : Controller
    {

        Entities db = new Entities();
        // GET: GioHang
        public ActionResult XemGioHang()
        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            if (tv != null)
            {
                ViewBag.list = db.ChiTietGiamGias.Where(n => n.MaThanhVien == tv.MaThanhVien).ToList();

                ViewBag.listgg = db.GiamGias.ToList();
                List<GioHang> listGioHang = SessionGioHang();
                List<ShopGioHang> listShopGioHang = SessionShopGioHang();
                ViewBag.listShopGioHang = listShopGioHang;
                ViewBag.TongSoLuong = TongSL();
                ViewBag.TongTien = TongTien();
                return View(listGioHang);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSL();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        public List<ShopGioHang> SessionShopGioHang()
        {
            List<ShopGioHang> listShopGioHang = (List<ShopGioHang>)Session["ShopGioHang"];
            if (listShopGioHang == null)
            {
                listShopGioHang = new List<ShopGioHang>();
                Session["ShopGioHang"] = listShopGioHang;
            }
            return listShopGioHang;
        }
        public List<GioHang> SessionGioHang()
        {
            List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];
            if (listGioHang == null)
            {
                listGioHang = new List<GioHang>();
                Session["GioHang"] = listGioHang;
            }
            return listGioHang;
        }
       
        public ActionResult ThemGioHang(int? MaSP, decimal Gia, int MaKichCo)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                //List<GioHang> listGioHang = SessionGioHang();
                List<ShopGioHang> listShopGioHang = (List<ShopGioHang>)Session["ShopGioHang"];
                List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];

                if (listGioHang != null)
                {
                    // check sản phẩm đã tồn tại hay chưa?
                    GioHang gh = listGioHang.SingleOrDefault(n => n.MaSP == MaSP);
                    //TH1 sản phẩm đã tồn tại 
                    if (gh != null)
                    {
                        gh.SoLuong++;
                        gh.ThanhTien = gh.SoLuong * gh.GiaHienTai;
                        //return Redirect(strURL);
                        //return PartialView("GioHangPartial");
                        return RedirectToAction("GioHangPartial");
                    }
                    else
                    {

                        GioHang newgh = new GioHang((int)MaSP, Gia, MaKichCo);
                        if (newgh.SoLuong < sp.SoLuongTon)
                        {
                            //Sau khi tạo xong thì add item vào listGioHang đã tạo truo72c đó
                            listGioHang.Add(newgh);
                            Session["GioHang"] = listGioHang;
                            //return PartialView("GioHangPartial");
                            return RedirectToAction("GioHangPartial");
                        }
                        else
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                        }
                    }


                }
                else
                {
                    listGioHang = new List<GioHang>();
                    GioHang newgh = new GioHang((int)MaSP, Gia, MaKichCo);
                    if (newgh.SoLuong < sp.SoLuongTon)
                    {
                        //Sau khi tạo xong thì add item vào listGioHang đã tạo truo72c đó
                        listGioHang.Add(newgh);
                        Session["GioHang"] = listGioHang;
                        //return PartialView("GioHangPartial");
                        return RedirectToAction("GioHangPartial");
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                    }
                }

            }



        }
      

        public int TongSL()
        {
            List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];
            if (listGioHang == null)
            {
                return 0;
            }
            int sl =  listGioHang.Sum(n => n.SoLuong);
            return sl;

        }

        public decimal TongTien()
        {
            List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];
            if (listGioHang == null)
            {
                return 0;
            }
            decimal TongDonGia = listGioHang.Sum(n => n.ThanhTien);
            return TongDonGia;

        }

        public ActionResult XoaGioHang(int MaSP)
        {

            // Kiem tra xem co ton tai list gio hang chua
            List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];
            // neu gio hang trong thi thoi
            if (listGioHang == null)
            {
                return null;
            }
            //nguoc lai, neu ton tai gio hang thi kiem tra xem mat hang muon xoa có nam trong gio hang khong
            GioHang gh = listGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            //neu san pham khong ton tai trong gio hang
            if (gh == null)
            {
                return null;
            }
            //nguoc lai, san pham ton tai trong gio hang, thi remove no khoi list san pham trong gio
            listGioHang.Remove(gh);
            ViewBag.listGioHang = listGioHang;
            ViewBag.TongSoLuong = TongSL();
            ViewBag.TongTien = TongTien();
            return RedirectToAction("ThemSlPartialView");
        }

        public ActionResult ThemSl(int MaSP)
        {
            int id = MaSP;
            List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];
            if(listGioHang == null)
            {
                return null;
            }
            else
            {
                GioHang sp = listGioHang.SingleOrDefault(n => n.MaSP == MaSP);
                if(sp != null)
                {
                    SanPham check = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
                    if (sp.SoLuong < check.SoLuongTon)
                    {
                        sp.SoLuong++;
                        sp.ThanhTien = sp.SoLuong * sp.Dongia;
                    }
                    else
                    {
                        return null;
                    }
                }
                //string url = "<script>window.location.href = 'http://localhost:62979/GioHang/ThemSl?MaSP="+id.ToString() +"'" + ";</script>";
                //return Content(url);
                return RedirectToAction("ThemSlPartialView");
            }
        }

        public ActionResult ThemSlPartialView()
        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            ViewBag.list = db.ChiTietGiamGias.Where(n => n.MaThanhVien == tv.MaThanhVien && n.DaSuDung == false).ToList();
           
            ViewBag.listShop = db.Shops.ToList();
            ViewBag.listgg = db.GiamGias.ToList();
            ViewBag.TongSoLuong = TongSL();
            ViewBag.TongTien = TongTien();
            ViewBag.listGioHang = Session["GioHang"];
            return PartialView("ThemSlPartialView");
        }


        public ActionResult GiamSL(int MaSP)
        {
           
            List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];
            if (listGioHang == null)
            {
                return null;
            }
            else
            {
                GioHang sp = listGioHang.SingleOrDefault(n => n.MaSP == MaSP);
                if (sp != null)
                {
                    
                    if (sp.SoLuong == 1)
                    {
                        listGioHang.Remove(sp);
                        return RedirectToAction("ThemSlPartialView");
                    }
                    else
                    {
                        sp.SoLuong--;
                        sp.ThanhTien = sp.SoLuong * sp.Dongia;
                    }
                }
                return RedirectToAction("ThemSlPartialView");
            }
        }
        public ActionResult UpdateQuantity(int MaSP, int SL)
        {

            List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];
            
            if (listGioHang == null)
            {
                return null;
            }
            else
            {
                GioHang sp = listGioHang.SingleOrDefault(n => n.MaSP == MaSP);
                if (sp != null)
                {
                    int? makc = sp.KichCo.MaKichCo;
                    KichCo checkSp = db.KichCos.SingleOrDefault(n => n.MaKichCo == makc);
                    if(checkSp.SL >= SL)
                    {
                        sp.SoLuong = SL;
                        sp.ThanhTien = sp.SoLuong * sp.Dongia;
                        return RedirectToAction("ThemSlPartialView");
                    }
                    return Json(new { mess = "fail" }, JsonRequestBehavior.AllowGet);
                    
                    
                }
                return Json(new { mess = "fail" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CapNhatGioHang()
        {
            
            return RedirectToAction("GioHangPartial");
        }
        public ActionResult ListGiamGia(int MaShop)
        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            List<ChiTietGiamGia> list = new List<ChiTietGiamGia> ();
            List<GiamGia> listGG = db.GiamGias.Where(n=>n.MaShop == MaShop).ToList();
            List<ChiTietGiamGia> listCTGG = db.ChiTietGiamGias.Where(n => n.MaThanhVien == tv.MaThanhVien).ToList();
            foreach (var item in listGG)
            {
                foreach (var item1 in listCTGG)
                {
                    if(item.MaGiamGia == item1.MaGiamGia)
                    {
                        list.Add(item1);
                    }
                }
            }
            var v = list.Select(x => new
            {
                MaGiamGia = x.MaGiamGia,
                MaCTGiamGia = x.MaCTGiamGia,
                SoTien = x.GiamGia.SoTien,
                TenGiamGia = x.GiamGia.TenGiamGia
            });
           

            return Json(new { data = v }, JsonRequestBehavior.AllowGet);
        }
    }
}