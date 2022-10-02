﻿using System;
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
                        //return Redirect(strURL);
                        //return PartialView("GioHangPartial");
                        return RedirectToAction("GioHangPartial");
                    }
                    else
                    {
                        
                        GioHang newgh = new GioHang((int)MaSP);
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
                    GioHang newgh = new GioHang((int)MaSP);
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

        public ActionResult CapNhatGioHang()
        {
            
            return RedirectToAction("GioHangPartial");
        }
    }
}