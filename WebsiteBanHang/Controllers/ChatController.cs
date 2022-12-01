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
    public class ChatController : Controller
    {
       public Entities db = new Entities();
       
        public ActionResult Index()
        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            IEnumerable<ThanhVien> listTV = db.ThanhViens.ToList();
            List<Chat2> messages = new List<Chat2>();
            foreach (ThanhVien item in listTV)
            {
                Chat2 chat = db.Chat2.Where(x => x.FromUserId == item.MaThanhVien && x.FromUserId != tv.MaThanhVien).ToList().LastOrDefault();
                if (chat != null)
                {
                    messages.Add(chat);
                }
            }
            return View(messages);
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetAllMessageChating(int UserID) //2
        {
            try
            {
                IEnumerable<Chat2> listMessage1 = db.Chat2.Where(x => x.FromUserId == UserID || x.ToUserId == UserID).OrderBy(x => x.CreatedDate).ToList();
                var listMessage = listMessage1.Select(x =>
                new
                {
                    Id = x.Id,
                    FromUserID = x.FromUserId,
                    Text = x.Text,
                    CreatedDate = x.CreatedDate.Value,
                    FromUserName = x.ThanhVien.HoTen
                });
                return Json(listMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Send(int FromUserId, int ToUserId, string Text, string SIde)
        {
            if (SIde == "Client")
            {
                Chat2 chat = db.Chat2.ToList().LastOrDefault();
                if (chat != null && !chat.Sent.Value)
                {
                    chat.Sent = true;
                    db.SaveChanges();
                   
                }
                Chat2 Chat = new Chat2();
                Chat.Sent = false;
                Chat.FromUserId = FromUserId;
                Chat.ToUserId = ToUserId;
                Chat.Text = Text;
                Chat.CreatedDate = DateTime.Now;

                db.Chat2.Add(Chat);
                db.SaveChanges();
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Chat2 Chat = new Chat2();

                Chat.FromUserId = FromUserId;
                Chat.ToUserId = ToUserId;
                Chat.Text = Text;
                Chat.CreatedDate = DateTime.Now;
                Chat.Sent = true;

                db.Chat2.Add(Chat);
                db.SaveChanges();
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Chating(int WithUserId, int ChatId = 0)
        {
            IEnumerable<Chat2> listChat;
            if (ChatId != 0)
            {
                //Update Sent
                Chat2 chat = db.Chat2.Find(ChatId);
                if (!chat.Sent.Value)
                {
                    chat.Sent = true;
                    db.SaveChanges();
                }
              
                listChat = db.Chat2.Where(x => x.FromUserId == WithUserId || x.ToUserId == WithUserId).OrderBy(x => x.CreatedDate).ToList();

                ViewBag.HoTen = db.ThanhViens.Find(WithUserId).HoTen;
                return View(listChat);

            }
            ViewBag.HoTen = db.ThanhViens.Find(WithUserId).HoTen;
            return View();
        }

        [AllowAnonymous]
        public JsonResult GetNotiMessage()
        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            try
            {
                var listMessage = db.Chat2.Where(x => x.Sent == false && x.FromUserId != tv.MaThanhVien).ToList().Select(x => new { ID = x.Id, FromUserID = x.FromUserId, FromUserAvatar = "user.png", FromUserName = x.ThanhVien.HoTen, CreatedDate = (DateTime.Now - x.CreatedDate.Value).Minutes }); ;
                return Json(listMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
