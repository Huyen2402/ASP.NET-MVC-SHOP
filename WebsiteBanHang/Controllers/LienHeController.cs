using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class LienHeController : Controller
    {
        Entities db = new Entities();
        // GET: LienHe
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SendMailAuto()
        {
            List<ThanhVien> listTV = db.ThanhViens.ToList();
            for(int i=0; i < listTV.Count; i++)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var senderEmail = new MailAddress("huyenb1910384@student.ctu.edu.vn", "Huyen");
                        var receiverEmail = new MailAddress(listTV[i].Email, "Receiver");
                        var password = "yyxrbzsfbkrftlny";
                        string subject = "Xác nhận tài khoản người dùng - Sàn thương mại điện tử Ori Cute";
                        //string body = "Chào bạn, đây là mã xác nhận tài khoản của bạn: ";
                        string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Views/EmailTemplates/QuangCao.cshtml"));
                        body = body.Replace("###Name", listTV[i].HoTen);

                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail.Address, password)
                        };
                        using (var mess = new MailMessage(senderEmail, receiverEmail)
                        {
                            IsBodyHtml = true,
                            Subject = subject,
                            Body = body
                        })
                        {
                            smtp.Send(mess);
                        }

                    }
                }
                catch (Exception)
                {
                    ViewBag.Error = "Some Error";
                }
            }
           
            return Json(new {status = "success"}, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RestartPrice()
        {
            DateTime CurrentDay = DateTime.Now;

            

            List<FlashSale> listFlash = db.FlashSales.ToList();
            foreach (FlashSale flashSale in listFlash)
            {
               
                
                    ViewBag.Day = flashSale;

                    List<ChiTietFlashSale> list = db.ChiTietFlashSales.Where(n => n.MaSale == flashSale.MaSale).ToList();
                    ViewBag.listSale = list;
                    for (var i = 0; i < list.Count(); i++)
                    {
                        int? MaSP = list[i].MaSP;
                        SanPham sp = db.SanPhams.Single(n => n.MaSP == MaSP);
                        sp.KhuyenMai = null;
                        db.SaveChanges();
                       

                    }
                
            }
            return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
        }
    }
}