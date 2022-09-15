using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class AdminController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult XemSanPham()
        {
            //if (Session["Quyen"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            List<SanPham> list = db.SanPhams.Where(n=>n.DaXoa == false).ToList();
            return View(list);
        }


        [HttpGet]
        public ActionResult ThemSanPham()
        {

            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.loaiSanPhams, "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX");
            return View();
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemSanPham(SanPham sp, HttpPostedFileBase[] HinhAnh)
        {

            // list nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.loaiSanPhams, "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX");



            // upload file hình ảnh
            for (int i = 0; i < HinhAnh.Length; i++)
            {
                //Check content image
                if (HinhAnh[i] != null && HinhAnh[i].ContentLength > 0)
                {
                    //Get file name
                    var fileName = Path.GetFileName(HinhAnh[i].FileName);
                    //Get path
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    
                    //Check exitst
                    if (!System.IO.File.Exists(path))
                    {
                        //Add image into folder
                        HinhAnh[i].SaveAs(path);
                       


                    }

                    
                }
              
            }

            sp.HinhAnh = HinhAnh[0].FileName;
            sp.HinhAnh1 = HinhAnh[1].FileName;
            sp.HinhAnh2 = HinhAnh[2].FileName;
            sp.HinhAnh3 = HinhAnh[3].FileName;

            db.SanPhams.Add(sp);
            db.SaveChanges();

            return RedirectToAction("XemSanPham", "Admin");

            //    if (HinhAnh.ContentLength > 0 && HinhAnh1.ContentLength > 0 && HinhAnh2.ContentLength > 0 && HinhAnh3.ContentLength > 0)
            //{
            //    string fileName = Path.GetFileName(HinhAnh.FileName);
            //    string fileName1 = Path.GetFileName(HinhAnh1.FileName);
            //    string fileName2 = Path.GetFileName(HinhAnh2.FileName);
            //    string fileName3 = Path.GetFileName(HinhAnh3.FileName);
            //    string path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
            //    string path1 = Path.Combine(Server.MapPath("~/Content/images"), fileName1);
            //    string path2 = Path.Combine(Server.MapPath("~/Content/images"), fileName2);
            //    string path3 = Path.Combine(Server.MapPath("~/Content/images"), fileName3);
            //    if (System.IO.File.Exists(path) && System.IO.File.Exists(path1) && System.IO.File.Exists(path2) && System.IO.File.Exists(path3))
            //        {

            //            ViewBag.Message = "File đã tồn tại";
            //            return View();
            //        }
            //        else
            //        {
            //            HinhAnh.SaveAs(path);
            //        HinhAnh1.SaveAs(path1);
            //        HinhAnh2.SaveAs(path2);
            //        HinhAnh3.SaveAs(path3);
            //        sp.HinhAnh = fileName;
            //        sp.HinhAnh1 = fileName1;
            //        sp.HinhAnh2 = fileName2;
            //        sp.HinhAnh3 = fileName3;

            //    }


            //        db.SanPhams.Add(sp);
            //        db.SaveChanges();

            //        return RedirectToAction("XemSanPham", "Admin");

            //    }
            //    else
            //    {
            //        ViewBag.Message = "Có lỗi";
            //    }








        }


        [HttpGet]
        public ActionResult SuaSanPham(int? MaSP)
        {


            if (MaSP == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                // tìm sản phẩm co trong csdl không ?
                SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
                // nếu có thì trả ra
                if (sp == null)
                {
                    Response.StatusCode = 404;
                }
                else
                {
                    ViewBag.HinhAnh = sp.HinhAnh;
                    ViewBag.HinhAnh1 = sp.HinhAnh1;
                    ViewBag.HinhAnh2 = sp.HinhAnh2;
                    ViewBag.HinhAnh3 = sp.HinhAnh3;
                    ViewBag.MoTa = sp.MoTa;
                    ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sp.MaNCC);
                    ViewBag.MaLoaiSP = new SelectList(db.loaiSanPhams, "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
                    ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", sp.MaNSX);
                    return View(sp);
                }

            }
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSanPham(SanPham sp, HttpPostedFileBase[] HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.loaiSanPhams, "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX");

            SanPham check = db.SanPhams.SingleOrDefault(n => n.MaSP == sp.MaSP);
            if (check != null)
            {
                for (int i = 0; i < HinhAnh.Length; i++)
                {
                    //Check content image
                    if (HinhAnh[i] != null && HinhAnh[i].ContentLength > 0)
                    {
                        //Get file name
                        var fileName = Path.GetFileName(HinhAnh[i].FileName);
                        //Get path
                        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                        //Check exitst
                        if (!System.IO.File.Exists(path))
                        {
                            //Add image into folder
                            HinhAnh[i].SaveAs(path);
                        }
                    }
                }

                if (HinhAnh[0] != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/Content/images/" + check.HinhAnh));
                    check.HinhAnh = HinhAnh[0].FileName;
                }
                if (HinhAnh[1] != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/Content/images/" + check.HinhAnh1));
                    check.HinhAnh1 = HinhAnh[1].FileName;
                }
                if (HinhAnh[2] != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/Content/images/" + check.HinhAnh2));
                    check.HinhAnh2 = HinhAnh[2].FileName;
                }
                if (HinhAnh[3] != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/Content/images/" + check.HinhAnh3));
                    check.HinhAnh3 = HinhAnh[3].FileName;
                }

                check.LuotBinhChon = sp.LuotBinhChon;
                check.LuotBinhLuan = sp.LuotBinhLuan;
                check.DaXoa = sp.DaXoa;
                check.TenSP = sp.TenSP;
                check.MoTa = sp.MoTa;
                check.NgayCapNhat = sp.NgayCapNhat;
                check.DonGia = sp.DonGia;
                check.SoLuongTon = sp.SoLuongTon;
                check.LuotXem = sp.LuotXem;
                check.MaNSX = sp.MaNSX;
                check.loaiSanPham = sp.loaiSanPham;
                check.MaNCC = sp.MaNCC;
                db.SaveChanges();

                return RedirectToAction("XemSanPham", "Admin");

            }
            return View(sp);

        }

        public ActionResult XoaSanPham(int MaSP)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp != null)
            {
                sp.DaXoa = true;
                db.SaveChanges();
            }
            return RedirectToAction("XemSanPham", "Admin");
        }

        public ActionResult XemLoaiSanPham()
        {
            //if (Session["Quyen"].ToString() != "Admin")
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            List<loaiSanPham> listLSP = db.loaiSanPhams.Where(n=>n.DaXoa == false).ToList();
            return View(listLSP);
        }


        [HttpGet]
        public ActionResult SuaLoaiSanPham(int? MaLoaiSP)
        {
            if(MaLoaiSP == null)
            {
                Response.StatusCode = 404;
            }
            else
            {

                loaiSanPham lsp = db.loaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == MaLoaiSP);
                if (lsp == null)
                {
                    Response.StatusCode = 404;
                }
                
                return View(lsp);
            }


            return View();
        }

        [HttpPost]
        public ActionResult SuaLoaiSanPham(loaiSanPham lsp)
        {
        loaiSanPham check = db.loaiSanPhams.SingleOrDefault(n=>n.MaLoaiSP == lsp.MaLoaiSP);
            if(check == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                check.TenLoai = lsp.TenLoai;
                db.SaveChanges();
                return RedirectToAction("XemLoaisanPham", "Admin");
            }


            return View(lsp);
        }

        [HttpGet]
        public ActionResult ThemLoaiSanPham()
        {


            return View();
        }

        [HttpPost]
        public ActionResult ThemLoaiSanPham(loaiSanPham lsp)
        {

            db.loaiSanPhams.Add(lsp);
            db.SaveChanges();
            return RedirectToAction("XemLoaiSanPham", "Admin");
        }

        public ActionResult XoaloaiSanPham(int? MaLoaiSP)
        {
            
                loaiSanPham lsp = db.loaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == MaLoaiSP);
                if (lsp == null)
                {
                    Response.StatusCode = 404;
                }
                else
                {
                    lsp.DaXoa = true;
                    db.SaveChanges();
                    
            }
            return RedirectToAction("XemLoaiSanPham", "Admin");
        }

        public ActionResult XemNguoiDung()
        {
            List< ThanhVien> tv = db.ThanhViens.ToList();

            return View(tv);
        }


        [HttpGet]
        public ActionResult ThemNguoiDung()
        {
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens, "MaLoaiTV", "TenLoai");

            return View();
        }

        [HttpPost]
        public ActionResult ThemNguoiDung(ThanhVien tv)
        {

            if (tv == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                db.ThanhViens.Add(tv);
                db.SaveChanges();
                return RedirectToAction("XemNguoiDung", "Admin");
            }
            return View();
        }


        
        public ActionResult KhoaNguoiDung(int? MaThanhVien)
        {

            
                ThanhVien check = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == MaThanhVien);
                if(check == null)
                {
                    Response.StatusCode = 404;
                }
                else
                {
                    check.DaKhoa = true;
                    db.SaveChanges();
                    
                }
            
            return RedirectToAction("XemNguoiDung", "Admin");
        }
        public ActionResult MoKhoaNguoiDung(int? MaThanhVien)
        {


            ThanhVien check = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == MaThanhVien);
            if (check == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                check.DaKhoa = false;
                db.SaveChanges();

            }

            return RedirectToAction("XemNguoiDung", "Admin");
        }

        public ActionResult DonHangChoXacNhan()
        {

            List<DonDatHang> listddh = db.DonDatHangs.Where(n=>n.MaTinhTrangGiaoHang == 5).ToList();
            
            

            

            return View(listddh);
        }

        public ActionResult XemChiTietDonHang(string MaDDH)
        {

            if(MaDDH == null)
            {
                Response.StatusCode = 404;

            }
            else
            {
                List< ChiTietDonDatHang > ct = db.ChiTietDonDatHangs.Where(n=>n.MaDDH == MaDDH).ToList();
                if(ct == null)
                {
                    Response.StatusCode = 404;
                }
                else
                {
                   
                    return View(ct);
                }
            }
            return View();
        }

        public ActionResult HuyDonHang(string MaDDH)
        {
            
            if( MaDDH == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                List<ChiTietDonDatHang> listCT = db.ChiTietDonDatHangs.Where(n => n.MaDDH == MaDDH).ToList();
                for(var i=0; i< listCT.Count(); i++)
                {
                    db.ChiTietDonDatHangs.Remove(listCT[i]);
                }
                DonDatHang ddh = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == MaDDH);
                db.DonDatHangs.Remove(ddh);
                
                
            }
            db.SaveChanges();
            return RedirectToAction("DonHangChoXacNhan");
        }

        public ActionResult XacNhanDonHang(string MaDDH)
        {
            if (MaDDH == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                DonDatHang ddh = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == MaDDH);
                if(ddh == null)
                {
                    Response.StatusCode = 404;

                }
                else
                {
                    ddh.MaTinhTrangGiaoHang = 6;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("DonHangChoXacNhan");

        }

        public ActionResult DonHangDangGiao()
        {

            List<DonDatHang> listddh = db.DonDatHangs.Where(n => n.MaTinhTrangGiaoHang == 6).ToList();


            return View(listddh);
        }

        public ActionResult XemBinhLuan()
        {
            List<BinhLuan> listBL = db.BinhLuans.ToList();

            return View(listBL);
        }

        [HttpGet]
        public ActionResult TraLoiBinhLuan(int? MaBL)
        {
            if (MaBL == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                BinhLuan checkBL = db.BinhLuans.SingleOrDefault(n => n.MaBL == MaBL);
                if (checkBL == null)
                {
                    Response.StatusCode = 404;
                }
                else
                {
                    List<TraLoiBinhLuan> list = db.TraLoiBinhLuans.ToList();
                    ViewBag.list = list;
                    return View(checkBL);
                }
            }
            return View();

        }

        [HttpPost]
        public ActionResult TraLoiBinhLuan(TraLoiBinhLuan tlbl)
        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            TraLoiBinhLuan traloi = new TraLoiBinhLuan();
            traloi.MaBL = tlbl.MaBL;
            traloi.NoiDungTraLoi = tlbl.NoiDungTraLoi;
            traloi.MaThanhVien = 1002;
            db.TraLoiBinhLuans.Add(traloi);
            db.SaveChanges();
            

            return RedirectToAction("TraLoiBinhLuan","Admin", new { MaBL = tlbl.MaBL });
        }

    }
}
