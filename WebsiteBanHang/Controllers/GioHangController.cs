using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class GioHangController : Controller
    {

        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: GioHang
        public ActionResult XemGioHang()
        {
            List<GioHang> listGioHang = SessionGioHang();
            ViewBag.TongSoLuong = TongSL();
            ViewBag.TongTien = TongTien();
            return View(listGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSL();
            ViewBag.TongTien = TongTien();
            return PartialView();
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

        public ActionResult ThemGioHang(int? MaSP, string strURL)
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
                List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];
               
                if (listGioHang != null)
                {
                    // check sản phẩm đã tồn tại hay chưa?
                    GioHang gh = listGioHang.SingleOrDefault(n => n.MaSP == MaSP);
                    //TH1 sản phẩm đã tồn tại 
                    if (gh != null)
                    {
                        gh.SoLuong++;
                        gh.ThanhTien = gh.SoLuong * gh.Dongia;
                        return Redirect(strURL);
                    }
                    else
                    {
                        
                        GioHang newgh = new GioHang((int)MaSP);
                        if (newgh.SoLuong < sp.SoLuongTon)
                        {
                            //Sau khi tạo xong thì add item vào listGioHang đã tạo truo72c đó
                            listGioHang.Add(newgh);
                            Session["GioHang"] = listGioHang;
                            return Redirect(strURL);
                        }
                        else
                        {
                            return View("ThongBao");

                        }
                    }
                   
                }
                else
                {
                    listGioHang = new List<GioHang>();
                    GioHang newgh = new GioHang((int)MaSP);
                    if (newgh.SoLuong < sp.SoLuongTon)
                    {
                        //Sau khi tạo xong thì add item vào listGioHang đã tạo truo72c đó
                        listGioHang.Add(newgh);
                        Session["GioHang"] = listGioHang;
                        return Redirect(strURL);
                    }
                    else
                    {
                        return View("ThongBao");

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

        public ActionResult XoaGioHang(int MaSP, string strURL)
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
            return Redirect(strURL);
        }

        public ActionResult ThemSl(int MaSP, string strURL)
        {
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
                return Redirect(strURL);
            }
        }


        public ActionResult GiamSL(int MaSP, string strURL)
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
                        return Redirect(strURL);
                    }
                    else
                    {
                        sp.SoLuong--;
                        sp.ThanhTien = sp.SoLuong * sp.Dongia;
                    }
                }
                return Redirect(strURL);
            }
        }
    }
}