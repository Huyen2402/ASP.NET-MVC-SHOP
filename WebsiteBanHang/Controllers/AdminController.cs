using Antlr.Runtime;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Schema;
using WebsiteBanHang.Extensions;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class AdminController : Controller
    {
        Entities db = new Entities();

        // GET: Admin
        public ActionResult Index()
        {
            Shop s = Session["CuaHang"] as Shop;

            List<ThanhVien> listTV = db.ThanhViens.Where(n => n.MaLoaiTV == 1).ToList();
            ViewBag.TongTV = listTV.Count();
            List<BinhLuan> listBl = db.BinhLuans.Where(n => n.SanPham.MaShop == s.MaShop).ToList();
            List<DonDatHang> listDDH = db.DonDatHangs.Where(x => x.MaShop == s.MaShop).ToList();
            ViewBag.TongBL = listBl.Count() > 0 ? listBl.Count : 0;
            ViewBag.TongDDH = listDDH.Count > 0 ? listDDH.Count : 0;
            ViewBag.TongTien = listDDH.Count > 0 ? TongTien(s.MaShop) : 0;

            //ViewBag.TongTien = TongTien(s.MaShop);
            int year = int.Parse(DateTime.Now.Year.ToString());
            // Data chart
            List<DataChart> data = db.DonDatHangs.Where(x => x.NgayDat.Value.Year == year)
                .GroupBy(x => x.NgayDat.Value.Month)
                .Select(x => new DataChart()
                {
                    Month = (x.FirstOrDefault().NgayDat.Value.Month),
                    Total = x.ToList().Sum(y => y.ChiTietDonDatHang.Sum(b => b.SoLuong * b.DonGia)).Value
                }).ToList();

            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach (var item in data)
            {
                dataPoints.Add(new DataPoint("Tháng " + item.Month.ToString(), double.Parse(item.Total.ToString())));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }

        public decimal? TongTien(int? MaShop)
        {

            decimal? tongtien = db.ChiTietDonDatHangs.Where(n => n.DonDatHang.MaShop == MaShop).Sum(n => n.DonGia * n.SoLuong).Value;

            return tongtien;

        }



        public ActionResult ThongKeBieuDo(int? time)
        {

            return View();
        }
        public ActionResult XemSanPham()
        {
            Shop s = Session["CuaHang"] as Shop;
            int? ma = s.MaMatHang;
            List<ChiTietMatHangKinhDoanh> list = db.ChiTietMatHangKinhDoanhs.Where(n => n.MaMatHang == ma).ToList();
            ViewBag.listCT = db.SanPhams.Where(n => n.DaXoa == false && n.MaShop == s.MaShop).ToList();
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


        public ActionResult ThemSanPham1(FormCollection f)
        {
            string TenSP = f["TenSP"];
            return View(f);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ThemSanPhama(SanPham sp)
        {
            Shop s = Session["CuaHang"] as Shop;
            // list nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.loaiSanPhams, "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX");



            //// upload file hình ảnh
            //for (int i = 0; i < HinhAnh.Length; i++)
            //{
            //    //Check content image
            //    if (HinhAnh[i] != null && HinhAnh[i].ContentLength > 0)
            //    {
            //        //Get file name
            //        var fileName = Path.GetFileName(HinhAnh[i].FileName);
            //        //Get path
            //        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
            //        HinhAnh[i].SaveAs(path);
            //        //Check exitst
            //        //if (!System.IO.File.Exists(path))
            //        //{
            //        //    //Add image into folder
            //       // HinhAnh[i].SaveAs(path);
            //        //   



            //        //}


            //    }
            //    else
            //    {
            //        return Json(new { mess = "success" }, JsonRequestBehavior.AllowGet);
            //    }

            //}

            //sp.HinhAnh = HinhAnh[0].FileName;
            //sp.HinhAnh1 = HinhAnh[1].FileName;
            //sp.HinhAnh2 = HinhAnh[2].FileName;
            //sp.HinhAnh3 = HinhAnh[3].FileName;
            sp.NgayCapNhat = DateTime.Now;
            sp.LuotBinhLuan = 0;
            sp.SoLanMua = 0;
            sp.DaXoa = false;
            sp.Moi = 1;

            sp.MaShop = s.MaShop;
            sp.SEOKeyword = StringHelper.UrlFriendly(sp.TenSP);
            db.SanPhams.Add(sp);

            db.SaveChanges();
            return Json(new { mess = "success", MaSP = sp.MaSP }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("ContThemSP","Admin", new {@MaSP = sp.MaSP });



        }
        public JsonResult SaveAsImg(int MaSP, string[] url)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);

            if (sp == null)
            {
                return Json(new { mess = "fial" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                if (url.Length > 0)
                {
                    sp.HinhAnh = url[0];
                    sp.HinhAnh1 = url[1];
                    sp.HinhAnh2 = url[2];
                    sp.HinhAnh3 = url[3];
                    db.SaveChanges();
                }


                return Json(new { mess = "success" }, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult ContThemSP(int MaSP)
        {
            ViewBag.MaSP = MaSP;


            return View();
        }
        public JsonResult TotalQuantity(int MaSP)
        {
            int? TongSL = db.KichCos.Where(n => n.MaSP == MaSP).Sum(x => x.SL);
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            sp.SoLuongTon = TongSL;
            db.SaveChanges();
            return Json(new { mess = "success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ContThemSPJS(string Ten, int SL, int MaSP)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp != null)
            {
                KichCo c = new KichCo();
                c.MaSP = MaSP;
                c.Ten = Ten;
                c.SL = SL;
                db.KichCos.Add(c);
                db.SaveChanges();
                return Json(new { mess = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { mess = "fail" }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public ActionResult SuaSanPham(int? MaSP)
        {
            Shop s = Session["CuaHang"] as Shop;

            if (MaSP == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                // tìm sản phẩm co trong csdl không ?
                SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP && n.MaShop == s.MaShop);
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
                    ViewBag.KC = db.KichCos.Where(n => n.MaSP == sp.MaSP).ToList();
                    return View(sp);
                }

            }
            return View();

        }

        public JsonResult GetAllImg(int MaSP)
        {
            SanPham s = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            string[] url = { s.HinhAnh, s.HinhAnh1, s.HinhAnh2, s.HinhAnh3 };
            return Json(url, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditSaveAsImg(int MaSP, string[] url, int[] a)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);

            if (sp == null)
            {
                return Json(new { mess = "fial" }, JsonRequestBehavior.AllowGet);

            }
            else
              if (url.Length > 1)
            {
                for (int i = 0; i < url.Length; i++)
                {
                    if (a[0] != 0)
                    {
                        sp.HinhAnh = url[i];
                        i++;
                    }
                    if (a[1] != 0)
                    {
                        sp.HinhAnh1 = url[i];
                            i++;
                    }
                    if (a[2] != 0)
                    {
                        sp.HinhAnh2 = url[i];
                        i++;
                    }
                    if (a[3] != 0)
                    {
                        sp.HinhAnh3 = url[i];
                        i++;
                    }
                }



            }
           if(url.Length == 1)
            {
                
                    if (a[0] != 0)
                    {
                        sp.HinhAnh = url[0];
                      
                    }
                    if (a[1] != 0)
                    {
                        sp.HinhAnh1 = url[0];
                        
                    }
                    if (a[2] != 0)
                    {
                        sp.HinhAnh2 = url[0];
                       
                    }
                    if (a[3] != 0)
                    {
                        sp.HinhAnh3 = url[0];
                       
                    }
               
            }

            db.SaveChanges();
            return Json(new { mess = "success" }, JsonRequestBehavior.AllowGet);
        }
    

        

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSanPham1(SanPham sp, HttpPostedFileBase[] HinhAnh, KichCo[]kc)
        {
            Shop s = Session["CuaHang"] as Shop;
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.loaiSanPhams, "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX");

            SanPham check = db.SanPhams.SingleOrDefault(n => n.MaSP == sp.MaSP && n.MaShop == s.MaShop);
            if (check != null)
            {
                //for (int i = 0; i < HinhAnh.Length; i++)
                //{
                //    //Check content image
                //    if (HinhAnh[i] != null && HinhAnh[i].ContentLength > 0)
                //    {
                //        //Get file name
                //        var fileName = Path.GetFileName(HinhAnh[i].FileName);
                //        //Get path
                //        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                //        //Check exitst
                //        if (!System.IO.File.Exists(path))
                //        {
                //            //Add image into folder
                //            HinhAnh[i].SaveAs(path);
                //        }
                //    }
                //}

                //if (HinhAnh[0] != null)
                //{
                //    System.IO.File.Delete(Server.MapPath("~/Content/images/" + check.HinhAnh));
                //    check.HinhAnh = HinhAnh[0].FileName;
                //}
                //if (HinhAnh[1] != null)
                //{
                //    System.IO.File.Delete(Server.MapPath("~/Content/images/" + check.HinhAnh1));
                //    check.HinhAnh1 = HinhAnh[1].FileName;
                //}
                //if (HinhAnh[2] != null)
                //{
                //    System.IO.File.Delete(Server.MapPath("~/Content/images/" + check.HinhAnh2));
                //    check.HinhAnh2 = HinhAnh[2].FileName;
                //}
                //if (HinhAnh[3] != null)
                //{
                //    System.IO.File.Delete(Server.MapPath("~/Content/images/" + check.HinhAnh3));
                //    check.HinhAnh3 = HinhAnh[3].FileName;
                //}


                check.LuotBinhLuan = sp.LuotBinhLuan;
                check.DaXoa = sp.DaXoa;
                check.TenSP = sp.TenSP;
                check.MoTa = sp.MoTa;
                check.NgayCapNhat = DateTime.Now;
                check.DonGia = sp.DonGia;
                check.SoLuongTon = sp.SoLuongTon;
                check.MoTaNgan = sp.MoTaNgan;
                check.MaNSX = sp.MaNSX;
                check.loaiSanPham = sp.loaiSanPham;
                check.MaNCC = sp.MaNCC;
                db.SaveChanges();

                return Json(new { mess = "success", MaSP = sp.MaSP }, JsonRequestBehavior.AllowGet);

            }
            return View(sp);

        }

        public ActionResult XoaSanPham(int MaSP)
        {
            Shop s = Session["CuaHang"] as Shop;
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP && n.MaShop == s.MaShop);
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
       
            List<loaiSanPham> listLSP = db.loaiSanPhams.Where(n => n.DaXoa == false ).ToList();
            return View(listLSP);
        }


        [HttpGet]
        public ActionResult SuaLoaiSanPham(int? MaLoaiSP)
        {
            if (MaLoaiSP == null)
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
            loaiSanPham check = db.loaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == lsp.MaLoaiSP);
            if (check == null)
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

        //[HttpPost]
        //public ActionResult ThemLoaiSanPham(loaiSanPham lsp)
        //{

        //    db.loaiSanPhams.Add(lsp);
        //    db.SaveChanges();
        //    return RedirectToAction("XemLoaiSanPham", "Admin");
        //}

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
            List<ThanhVien> tv = db.ThanhViens.ToList();

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
            if (check == null)
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
            Shop s = Session["CuaHang"] as Shop;
            List<DonDatHang> listddh = db.DonDatHangs.Where(n => n.MaTinhTrangGiaoHang == 1 && n.MaShop == s.MaShop).OrderByDescending(n=>n.NgayDat).ToList();





            return View(listddh);
        }

        public ActionResult XemChiTietDonHang(string MaDDH)
        {
            Shop s = Session["CuaHang"] as Shop;
            if (MaDDH == null)
            {
                Response.StatusCode = 404;

            }
            else
            {
                List<ChiTietDonDatHang> ct = db.ChiTietDonDatHangs.Where(n => n.MaDDH == MaDDH && n.DonDatHang.MaShop == s.MaShop).ToList();
                if (ct == null)
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
                if (ddh == null)
                {
                    Response.StatusCode = 404;

                }
                else
                {
                    ddh.MaTinhTrangGiaoHang = 2;
                    ThongBaoDH newTB = new ThongBaoDH();
                    newTB.MaTV = ddh.MaKH;
                    newTB.MaDH = MaDDH;
                    newTB.DaXem = false;
                    newTB.ThoiGian = DateTime.Now;
                    db.ThongBaoDHs.Add(newTB);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("DonHangChoXacNhan");

        }

        public ActionResult DonHangDangGiao()
        {
            Shop s = Session["CuaHang"] as Shop;
            List<DonDatHang> listddh = db.DonDatHangs.Where(n => n.MaTinhTrangGiaoHang == 2 && n.MaShop == s.MaShop).ToList();


            return View(listddh);
        }
        public ActionResult DonHangThanhCong()
        {
            Shop s = Session["CuaHang"] as Shop;
            List<DonDatHang> listThanhCong = db.DonDatHangs.Where(n => n.MaTinhTrangGiaoHang == 3 && n.MaShop == s.MaShop).ToList();
            return View(listThanhCong);
        }

        public ActionResult XemBinhLuan()
        {
            Shop sh = Session["CuaHang"] as Shop;
            DateTime dateStart = DateTime.Now.AddDays(-3);
            DateTime dateEnd = DateTime.Now;
            List<BinhLuan> listBL = db.BinhLuans.Where(s =>  s.SanPham.MaShop == sh.MaShop).OrderByDescending(b => b.NgayTao).ToList();
            //List<BinhLuan> listBL = db.BinhLuans.ToList();

            return View(listBL);
        }

        [HttpGet]
        public ActionResult TraLoiBinhLuan(int? MaBL)
        {
            Shop s = Session["CuaHang"] as Shop;
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
                    ViewBag.tensop = s.TenShop;
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
            traloi.NgayTao = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            traloi.MaThanhVien = 1002;
            db.TraLoiBinhLuans.Add(traloi);
            db.SaveChanges();


            return RedirectToAction("TraLoiBinhLuan", "Admin", new { MaBL = tlbl.MaBL });
        }
        public ActionResult TraLoiBinhLuan2(FormCollection f)
        {
            Shop s = Session["CuaHang"] as Shop;
            TraLoiBinhLuan traloi = new TraLoiBinhLuan();
            traloi.MaBL = Int32.Parse(f["MaBL"]) ;
            traloi.NoiDungTraLoi = f["NoiDungTraLoi"] ;
            traloi.MaSP = Int32.Parse(f["MaSP"]);
            traloi.NgayTao = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            traloi.MaThanhVien = Int32.Parse(f["MaThanhVien"]);
            db.TraLoiBinhLuans.Add(traloi);
            db.SaveChanges();
            return Json(new { mess = "success"}, JsonRequestBehavior.AllowGet);
        }
        public void InFileExcel()
        {

            string date = DateTime.Now.ToString();
            List<ChiTietDonDatHang> listexcel = db.ChiTietDonDatHangs.ToList();

            ExcelPackage ex = new ExcelPackage();
            ExcelWorksheet ws = ex.Workbook.Worksheets.Add("Report");
            ws.Cells["A1"].Value = "Tên cửa hàng";
            ws.Cells["B1"].Value = "Huyền Cosmetic";
            ws.Cells["A2"].Value = "Ngày tháng";
            ws.Cells["B2"].Value = date;

            ws.Cells["A3"].Value = "Phone";
           
            
            ws.Cells["B3"].Value = "0356492230";

            ws.Cells["A6"].Value = "MaDDH";
            ws.Cells["B6"].Value = "MaSP";
            ws.Cells["C6"].Value = "TenSP";
            ws.Cells["D6"].Value = "SL";
            ws.Cells["E6"].Value = "DonGia";
            ws.Cells["F6"].Value = "MaChiTietDDH";

            int rowSart = 7;
            foreach (var item in listexcel)
            {
                ws.Cells[string.Format("A{0}", rowSart)].Value = item.MaDDH;
                ws.Cells[string.Format("B{0}", rowSart)].Value = item.MaSP;
                ws.Cells[string.Format("C{0}", rowSart)].Value = item.TenSP;
                ws.Cells[string.Format("D{0}", rowSart)].Value = item.SoLuong;
                ws.Cells[string.Format("E{0}", rowSart)].Value = item.DonGia;
                ws.Cells[string.Format("F{0}", rowSart)].Value = item.MaChiTietDDH1;
                rowSart++;
            }
            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment: filename="+ "InFileExcel.xlsx");
            Response.BinaryWrite(ex.GetAsByteArray());
            Response.End();


        }

        public JsonResult UpdateStatusComment(int? MaBL)
        {
            Shop s = Session["CuaHang"] as Shop;
            if (MaBL == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                BinhLuan bl = db.BinhLuans.SingleOrDefault(n => n.MaBL == MaBL && n.SanPham.MaShop == s.MaShop);
                bl.DaXem = true;
                db.SaveChanges();
              
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
           
        }
        public ActionResult DangXuat()
        {
            Session["CuaHang"] = null;
            return RedirectToAction("DangNhapCuaHang", "Home");
        }

        public JsonResult BoQuaBL(int? MaBL)
        {
            if(MaBL == null)
            {
                Response.StatusCode = 404;
            }
            BinhLuan bl = db.BinhLuans.SingleOrDefault(n => n.MaBL == MaBL);
            bl.DaXem = true;
            db.SaveChanges();
            return Json(new {status = true}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddFlashSale()
        {
            ViewBag.CurrentDay = DateTime.Now;
            List<FlashSale> listsale = db.FlashSales.ToList();
          

            return View(listsale);
        }

        
        public ActionResult AddFlashSale(int MaSP, int MaSale)
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddProductFlashSale(int MaSale)
        {
            Shop shop = Session["CuaHang"] as Shop;
            ViewBag.MaSale = MaSale;
            List<SanPham> listSp = db.SanPhams.Where(n => n.MaShop == shop.MaShop && n.DaXoa == false).ToList();
            return View(listSp);
        }


        [HttpPost]
        public JsonResult AddChiTietFlashSale(int[] arr, int MaSale)
        {
            Shop shop = Session["CuaHang"] as Shop;
            for(int i =0; i <= arr.Length-1; i++)
            {
                int MaSP = arr[i];
                SanPham sp = db.SanPhams.Single(n => n.MaSP == MaSP);
                if (sp != null)
                {
                    ChiTietFlashSale ct = new ChiTietFlashSale();
                    ct.MaSP = arr[i];
                    ct.MaSale = MaSale;
                    ct.MaShop = shop.MaShop;
                    ct.NgungSale = false;
                    db.ChiTietFlashSales.Add(ct);
                    db.SaveChanges();

                    Session["arrMaSP"] = arr;

                }
              
            }
           
           
            return Json(new {status = true}, JsonRequestBehavior.AllowGet);

        }
      
        [HttpGet]
        public ActionResult AddDiscount()
        {
            return View();
        }
     
        public JsonResult AddDiscount123(int sale)
        {
            Shop shop = Session["CuaHang"] as Shop;
            int[] arr= Session["arrMaSP"] as int[];


           
            for(int i =0; i <= arr.Length-1; i++)
            {
                foreach (ChiTietFlashSale ct in db.ChiTietFlashSales)
                {
                    if (ct.MaShop == shop.MaShop)
                    {
                        int MaSP = arr[i];
                        SanPham sp = db.SanPhams.Single(n => n.MaSP == MaSP);
                        sp.KhuyenMai = sale;
                    }
                }
            }

            db.SaveChanges();
            Session["arrMaSP"] = null;
            return Json(new {status =true}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult XemVoucher()
        {
            Shop shop = Session["CuaHang"] as Shop;
            List<GiamGia> gg = db.GiamGias.Where(n=>n.MaShop == shop.MaShop).ToList();
            return View(gg);
        }
        public JsonResult GetVoucher(int MaGiamGia)
        {
            db.Configuration.ProxyCreationEnabled = false;
            GiamGia gg = db.GiamGias.SingleOrDefault(n=>n.MaGiamGia == MaGiamGia);
            if(gg == null)
            {
                return Json(null);
            }
            else
            {
                return Json(new {data = gg }, JsonRequestBehavior.AllowGet);
            }
           
        }

        public JsonResult EditVoucher(GiamGia gg)
        {
            db.Configuration.ProxyCreationEnabled = false;
            GiamGia g = db.GiamGias.SingleOrDefault(n => n.MaGiamGia == gg.MaGiamGia);
            if (g == null)
            {
                return Json(null);
            }
            else
            {
                g.TenGiamGia = gg.TenGiamGia;
                g.SoTien = gg.SoTien;
                g.SL = gg.SL;
                db.SaveChanges();
                return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ThemVoucher()
        {
            return View();
        }

        public JsonResult ThemVoucherJson(GiamGia gg)
        {
            Shop shop = Session["CuaHang"] as Shop;
            GiamGia g = new GiamGia();
            g.TenGiamGia = gg.TenGiamGia;
            g.SoTien = gg.SoTien;
            g.SL = gg.SL;
            g.MaShop = shop.MaShop;
            db.GiamGias.Add(g);
            db.SaveChanges();

            return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult XoaVoucher(int MaGiamGia)
        {
            db.Configuration.ProxyCreationEnabled = false;
            GiamGia g = db.GiamGias.SingleOrDefault(n => n.MaGiamGia == MaGiamGia);
            if (g == null)
            {
                return Json(new { data = "failed" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                db.GiamGias.Remove(g);
                db.SaveChanges();
               
                return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);
            }

          

        }

        public ActionResult XemMatHang()
        {
            Shop shop = Session["CuaHang"] as Shop;
            int? ma = shop.MaMatHang;
            List<ChiTietMatHangKinhDoanh> list = db.ChiTietMatHangKinhDoanhs.Where(n=>n.MaMatHang == ma ).ToList();
            List<loaiSanPham> LSP = db.loaiSanPhams.Where(n => n.DaXoa == false).ToList();
            ViewBag.LSP = LSP;
            return View(list);
        }
        public JsonResult ThemMatHang(int MaLSP)
        {
           
            Shop shop = Session["CuaHang"] as Shop;
            int? ma = shop.MaMatHang;
            List<ChiTietMatHangKinhDoanh> hi = db.ChiTietMatHangKinhDoanhs.Where(n => n.MaMatHang == shop.MaMatHang).ToList();
            ChiTietMatHangKinhDoanh[] a = hi.ToArray();
            for(int i=0; i< a.Length; i++)
            {
                if (hi[i].MaLSP == MaLSP)
                {
                    return Json(new { data = "duplicate" }, JsonRequestBehavior.AllowGet);
                }
                
            }
            ChiTietMatHangKinhDoanh c = new ChiTietMatHangKinhDoanh();
                c.MaMatHang = shop.MaMatHang;
                c.MaLSP = MaLSP;
                c.NgungKD = false;
                db.ChiTietMatHangKinhDoanhs.Add(c);
                db.SaveChanges();
                List<ChiTietMatHangKinhDoanh> list = db.ChiTietMatHangKinhDoanhs.Where(n => n.MaMatHang == ma).ToList();

                return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);
            
            return Json(null, JsonRequestBehavior.AllowGet);

        }
        public JsonResult NgungKD(int MaCT)
        {
            db.Configuration.ProxyCreationEnabled = false;
            ChiTietMatHangKinhDoanh ct = db.ChiTietMatHangKinhDoanhs.SingleOrDefault(n => n.MaCTMatHang == MaCT);
            ct.NgungKD = true;
            db.SaveChanges();
            return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult XemSPFlashSale()
        {
            DateTime t = DateTime.Now;
            Shop shop = Session["CuaHang"] as Shop;
            List<ChiTietFlashSale> ct = db.ChiTietFlashSales.Where(n => n.MaShop == shop.MaShop && n.NgungSale == false).ToList();
            List<ChiTietFlashSale> send = new List<ChiTietFlashSale>();
            //FlashSale f = db.FlashSales.SingleOrDefault(n => n.MaSale == ct[0].MaSale);
            foreach(var item in ct)
            {
                if (item.FlashSale.NgaySale.Value.Date == t.Date && item.FlashSale.NgaySale.Value.Month == t.Month && item.FlashSale.NgaySale.Value.Year == t.Year)
                {
                    send.Add(item);
                }
            }
           
            return View(send);
        }
        public JsonResult NgungSale(int MaCTSale)
        {
            ChiTietFlashSale ct = db.ChiTietFlashSales.SingleOrDefault(n => n.MaChiTietSale == MaCTSale);
            if(ct != null)
            {
                ct.NgungSale = true;
                db.SaveChanges();
                return Json(new { mess = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { mess = "faild" }, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult SaleFuture()
        {
            DateTime t = DateTime.Now;
            Shop shop = Session["CuaHang"] as Shop;
            List<ChiTietFlashSale> ct = db.ChiTietFlashSales.Where(n => n.MaShop == shop.MaShop && n.NgungSale == false).ToList();
            List<ChiTietFlashSale> send = new List<ChiTietFlashSale>();
            //FlashSale f = db.FlashSales.SingleOrDefault(n => n.MaSale == ct[0].MaSale);
            foreach (var item in ct)
            {
                if (item.FlashSale.NgaySale.Value.Date > t.Date  && item.FlashSale.NgaySale.Value.Month >= t.Month && item.FlashSale.NgaySale.Value.Year >= t.Year)
                {
                    send.Add(item);
                }
            }

            return View(send);
        }
        public JsonResult XoaSale(int MaCTSale)
        {
            ChiTietFlashSale ct = db.ChiTietFlashSales.SingleOrDefault(n => n.MaChiTietSale == MaCTSale);
            if (ct != null)
            {
                db.ChiTietFlashSales.Remove(ct);
                db.SaveChanges();
                return Json(new { mess = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { mess = "faild" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
