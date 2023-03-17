using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class QuanTriVienController : Controller
    {
        Entities db = new Entities();
        // GET: QuanTriVien
        public ActionResult Index()
        {
            int? XetDuyet = db.Shops.Where(n=>n.XacNhan == false).ToList().Count;
            int? TongShop = db.Shops.Where(n=>n.XacNhan == true).ToList().Count;
            int? TongDM = db.loaiSanPhams.Where(n=>n.DaXoa == false).ToList().Count;
            int? TongNCC = db.NhaCungCaps.ToList().Count;
            int?  NSX = db.NhaSanXuats.ToList().Count();
            ViewBag.Xet = XetDuyet;
            ViewBag.TongShop = TongShop;
            ViewBag.TongDM = TongDM;
            ViewBag.TongNCC = TongNCC;
            ViewBag.TongNSX = NSX;
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ThanhVien tv)
        {
            ThanhVien check = db.ThanhViens.SingleOrDefault(n=>n.Email == tv.Email && n.MatKhau == tv.MatKhau && n.MaLoaiTV == 2) ;
            if(check == null)
            {

                ViewBag.Mess = "Sai tài khoản hoặc mật khẩu";
                return View();
            }
            ViewBag.Mess = "";
            return RedirectToAction("Index");
        }
        public ActionResult ChoXetDuyet()
        {
            List<Shop> listShop = db.Shops.Where(n => n.XacNhan == false).ToList();
            return View(listShop);
        }
        public ActionResult ThongTinShop(int MaShop)
        {
            Shop shop = db.Shops.SingleOrDefault(n => n.MaShop == MaShop);
            Nullable< int> MaMatHang = shop.MaMatHang;
            ViewBag.listCTKD = db.ChiTietMatHangKinhDoanhs.Where(n => n.MaMatHang == MaMatHang).ToList();


            return View(shop);
        }
        public ActionResult TaoFlashSale()
        {
            return View();
        }

      

        public JsonResult TaoFlashSaleAjax(int? value, DateTime? datee, DateTime? end)
        {
            int? val = value == null ? value = null : value;
            DateTime? date = datee == null ? datee = null : datee;
            if(date != null && end != null)
            {
                FlashSale newfl = new FlashSale();
                newfl.NgaySale = date;
                newfl.EndTime = end;
                db.FlashSales.Add(newfl);
                db.SaveChanges();
            }
            else
            {
                DateTime now = (DateTime)DateTime.Now;
                FlashSale newfl = new FlashSale();
                for (int i=0; i< val;i++)
                {
                   
                    newfl.NgaySale = now;
                    newfl.EndTime = now.AddDays(1);
                    db.FlashSales.Add(newfl);
                    db.SaveChanges();
                    now = now.AddDays(1);


                }
            }

            return Json("alo", JsonRequestBehavior.AllowGet);
        }

        public JsonResult XetDuyetShop(int MaShop)
        {
            Shop s = db.Shops.SingleOrDefault(n => n.MaShop == MaShop);
            if(s == null)
            {
                Response.StatusCode = 404;

            }
            else
            {
                s.XacNhan = true;
                return Json(new {mess = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { mess = "fail" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllShop()
        {
            List<Shop> listShop = db.Shops.Where(n=>n.XacNhan == true).ToList();

            return View(listShop);
        }

        public JsonResult GetInfoShop(int MaShop)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Shop s = db.Shops.SingleOrDefault(n => n.MaShop == MaShop);
          
            if (s == null)
            {
                return Json(null);
            }
            else
            {
                return Json(new { data = s }, JsonRequestBehavior.AllowGet);
            }
           
           
        }

        public JsonResult GetMatHang(int MaMatHang)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<ChiTietMatHangKinhDoanh> list = db.ChiTietMatHangKinhDoanhs.Where(n=>n.MaMatHang == MaMatHang).ToList();
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLSP(int MaLSP)
        {
            db.Configuration.ProxyCreationEnabled = false;
            loaiSanPham lsp = db.loaiSanPhams.SingleOrDefault(n=>n.MaLoaiSP== MaLSP);

            return Json(new { data = lsp }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult KhoaShop(int MaShop)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Shop s = db.Shops.SingleOrDefault(n=>n.MaShop == MaShop);
            if(s == null)
            {
                return Json(new { data = "failed" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                s.DaKhoa= true;
                db.SaveChanges();
                return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult XemLSP ()
        {
            List<loaiSanPham> listLSP = db.loaiSanPhams.Where(n => n.DaXoa == false).ToList();
            return View(listLSP);
        }
        public JsonResult GetInfoLSP(int MaLSP)
        {
            db.Configuration.ProxyCreationEnabled = false;
            loaiSanPham l = db.loaiSanPhams.SingleOrDefault(n=>n.MaLoaiSP == MaLSP);
            if(l == null)
            {
                return Json(null);
            }
            return Json(l, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditLSP(FormCollection f, HttpPostedFileBase HinhAnh)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            int MaLSP = Convert.ToInt32(f["MaLoaiSP"]);
            string TenLoai = f["TenLoai"];
           
            // Checking no of files injected in Request object 
            if (Request.Files.Count > 0)
            {
                try
                {
                    loaiSanPham check = db.loaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == MaLSP);
                    if (check != null)
                    {
                        HttpFileCollectionBase files = Request.Files;
                        for (int i = 0; i < files.Count; i++)
                        {
                            //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                            //string filename = Path.GetFileName(Request.Files[i].FileName);  

                            HttpPostedFileBase file = files[i];
                            string fname;

                            // Checking for Internet Explorer  
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {


                                fname = file.FileName;
                                if (check.HinhAnh != fname && fname != "")
                                {
                                    check.HinhAnh = fname;
                                    check.TenLoai = TenLoai;
                                   
                                    fname = Path.Combine(Server.MapPath("~/Content/images/"), fname);
                                    file.SaveAs(fname);
                                }
                                else
                                {
                                    check.TenLoai = TenLoai;
                                }



                                db.SaveChanges();
                            }


                        }
                        // Returns message that successfully uploaded  
                        return Json(new { mess = "success" }, JsonRequestBehavior.AllowGet);
                    }
                    //  Get all files from Request object  

                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json(new { mess = "hello" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { mess = "hello" }, JsonRequestBehavior.AllowGet);
        }

       
        public JsonResult ThemLoaiSanPham(FormCollection f)
        {
           
            string TenLoai = f["TenLoai"];
            string url = f["url"];  
            loaiSanPham lsp = new loaiSanPham();
            lsp.TenLoai = TenLoai;
            lsp.DaXoa = false;
            lsp.HinhAnh = url;
            db.loaiSanPhams.Add(lsp);
            db.SaveChanges();
            return Json(new { mess = "success" }, JsonRequestBehavior.AllowGet);
           

        }

        public JsonResult AddNSX(string TenNSX)
        {
            NhaSanXuat nsx = new NhaSanXuat();
            nsx.TenNSX = TenNSX;
            db.NhaSanXuats.Add(nsx);
            db.SaveChanges();
            return Json(new { mess = "success" }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult XemNSX()
        {
            List<NhaSanXuat> listNSX = db.NhaSanXuats.ToList();
            return View(listNSX);
        }
        public JsonResult GetInfoNSX(int MaNSX)
        {
            db.Configuration.ProxyCreationEnabled = false;
            NhaSanXuat check = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX == MaNSX);
            if(check== null)
            {
                return Json(new { mess = "fail" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = check, mess = "success" }, JsonRequestBehavior.AllowGet);
            }

           

        }
        public JsonResult EditNSX(NhaSanXuat nsx)
        {
            db.Configuration.ProxyCreationEnabled = false;
            NhaSanXuat check = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX == nsx.MaNSX);
            if (check == null)
            {
                return Json(new { mess = "fail" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                check.TenNSX = nsx.TenNSX;
                db.SaveChanges();
                return Json(new { mess = "success" }, JsonRequestBehavior.AllowGet);
            }



        }

    }
}