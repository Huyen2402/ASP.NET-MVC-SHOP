using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using System.Web.Hosting;
using System.Xml.Linq;

namespace WebsiteBanHang.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        Entities db= new Entities();
        public ActionResult XemShop(int? MaShop)
        {
            if(MaShop == null)
            {
                Response.StatusCode = 404;
            }
           Shop shop = db.Shops.SingleOrDefault(n=>n.MaShop == MaShop);
            ViewBag.list = db.SanPhams.Where(n => n.MaShop == MaShop && n.DaXoa == false).ToList();

            return View(shop);
        }
        [HttpGet]
       public ActionResult CheckAvtShop(int? MaShop)
        {
            Shop s = db.Shops.SingleOrDefault(n => n.MaShop == MaShop);
            if(s != null)
            {
                Session["MaShop"] = s.MaShop;
                ViewBag.MaShop = MaShop;
                if(s.HinhAnh != null)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return View();
                }
            }
           
            return RedirectToAction("DangNhapCuaHang","Home");
        }

        public JsonResult UpAvt( string url)
        {
            Nullable<int> MaShop = Session["MaShop"] as Nullable<int>;
            Shop shop = db.Shops.SingleOrDefault(n => n.MaShop == MaShop);
            if(shop != null){
                shop.HinhAnh = url;
                db.SaveChanges();
                return Json(new { mess = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { mess = "fail" }, JsonRequestBehavior.AllowGet);
            }
           
        }

        //[HttpPost]
        //public JsonResult CheckAvtShop()
        //{
        //    Nullable< int> MaShop = Session["MaShop"] as  Nullable<int>;
        //    Shop check = db.Shops.SingleOrDefault(n=>n.MaShop== MaShop);
        //    if(check != null)
        //    {
        //        // Checking no of files injected in Request object  
        //        if (Request.Files.Count > 0)
        //        {
        //            try
        //            {
        //                //  Get all files from Request object  
        //                HttpFileCollectionBase files = Request.Files;
        //                for (int i = 0; i < files.Count; i++)
        //                {
        //                    //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
        //                    //string filename = Path.GetFileName(Request.Files[i].FileName);  

        //                    HttpPostedFileBase file = files[i];
        //                    string fname;

        //                    // Checking for Internet Explorer  
        //                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
        //                    {
        //                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
        //                        fname = testfiles[testfiles.Length - 1];
        //                    }
        //                    else
        //                    {
        //                        fname = file.FileName;
        //                        check.HinhAnh = fname;
        //                        db.SaveChanges();
        //                    }

        //                    // Get the complete folder path and store the file inside it.  
        //                    fname = Path.Combine(Server.MapPath("~/Content/images/"), fname);
        //                    file.SaveAs(fname);
                            
        //                }
        //                try
        //                {
        //                    if (ModelState.IsValid)
        //                    {
        //                        var senderEmail = new MailAddress("huyenb1910384@student.ctu.edu.vn", "Huyen");
        //                        var receiverEmail = new MailAddress(check.TaiKhoan, "Receiver");
        //                        var password = "yyxrbzsfbkrftlny";
        //                        string subject = "Xác nhận đăng ký cửa hàng - Sàn thương mại điện tử Ori Cute";
        //                        string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Views/EmailTemplates/Customer.cshtml"));
        //                        body = body.Replace(@"######", @"Chào bạn, đây là Email xác nhận bạn đã đăng ký cửa hàng tại sàn thương mại Ori Cute. Sau 5-7 ngày sau khi quản trị viên xét duyệt chúng tôi sẽ có thông báo cho bạn. Đây là tin nhắn tự động bạn không cần phải trả lời. Có rất nhiều lựa chọn nhưng bạn vẫn chọn sàn thương mại của mình, Ori cảm ơn bạn rất nhiều!! ");
        //                        var smtp = new SmtpClient
        //                        {
        //                            Host = "smtp.gmail.com",
        //                            Port = 587,
        //                            EnableSsl = true,
        //                            DeliveryMethod = SmtpDeliveryMethod.Network,
        //                            UseDefaultCredentials = false,
        //                            Credentials = new NetworkCredential(senderEmail.Address, password)
        //                        };
        //                        using (var mess = new MailMessage(senderEmail, receiverEmail)
        //                        {
        //                            IsBodyHtml = true,
        //                            Subject = subject,
        //                            Body = body
        //                        })
        //                        {
        //                            smtp.Send(mess);
        //                        }

        //                    }
        //                }
        //                catch (Exception)
        //                {
        //                    ViewBag.Error = "Some Error";
        //                }

        //                // Returns message that successfully uploaded  
        //                return Json("File Uploaded Successfully!");
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json("Error occurred. Error details: " + ex.Message);
        //            }
        //        }
        //        else
        //        {
        //            return Json("No files selected.");
        //        }
        //    }


        //    //return Json(new { status = "fail" }, JsonRequestBehavior.AllowGet);
        //    return Json("Add avt success!.");
        //    //return RedirectToAction("DangKyCuaHang", "Home");
        //}

        public JsonResult NewProduct(int? MaShop)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<SanPham> listNew = db.SanPhams.Where(x=>x.MaShop == MaShop && x.DaXoa == false).OrderByDescending(n => n.NgayCapNhat).ToList();
            
            //return Json( new { status = true,  } , JsonRequestBehavior.AllowGet);
            return Json(listNew, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BestSell(int? MaShop)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<SanPham> bestSell = db.SanPhams.Where(x => x.MaShop == MaShop && x.DaXoa == false).OrderByDescending(n => n.SoLanMua).ToList();

            //return Json( new { status = true,  } , JsonRequestBehavior.AllowGet);
            return Json(bestSell, JsonRequestBehavior.AllowGet);
        }

        public JsonResult filterProduct(int MaShop, int val)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if(val == 1)
            {
                List<SanPham> listFilter = db.SanPhams.Where(x => x.DaXoa == false && x.MaShop == MaShop).OrderBy(n => n.DonGia).ToList();
                return Json(listFilter, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<SanPham> listFilter = db.SanPhams.Where(x => x.DaXoa == false && x.MaShop == MaShop).OrderByDescending(n => n.DonGia).ToList();
                return Json(listFilter, JsonRequestBehavior.AllowGet);
            }
        }
    }
}