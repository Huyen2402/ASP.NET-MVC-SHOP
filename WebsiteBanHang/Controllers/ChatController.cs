﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class ChatController : Controller
    {
        Entities db = new Entities();

        public ActionResult Index()
        {
            Shop shop = Session["CuaHang"] as Shop;
            //ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            IEnumerable<ThanhVien> listTV = db.ThanhViens.ToList();
            List<Chat> messages = new List<Chat>();
            foreach (ThanhVien item in listTV)
            {
                Chat chat = db.Chats.Where(x => x.MaThanhVien == item.MaThanhVien && x.MaShop != shop.MaShop).ToList().LastOrDefault();
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
            Shop shop = Session["CuaHang"] as Shop;
            try
            {
                IEnumerable<Chat> listMessage1 = db.Chats.Where(x => x.MaThanhVien == UserID && x.MaShop == shop.MaShop).OrderBy(x => x.NgayTao).ToList();
                var listMessage = listMessage1.Select(x =>
                new
                {
                    Id = x.ID,
                    FromUserID = x.MaShop,
                    Text = x.Text,
                    CreatedDate = x.NgayTao.Value,
                    FromUserName = x.ThanhVien.HoTen,
                    Client = x.Client,
                    TenShop = shop.TenShop
                }) ;
                return Json(listMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllMessageChatingWithShop(int UserID) //2
        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            try
            {
                IEnumerable<Chat> listMessage1 = db.Chats.Where(x => x.MaShop == UserID && x.MaThanhVien == tv.MaThanhVien ).OrderBy(x => x.NgayTao).ToList();
                var listMessage = listMessage1.Select(x =>
                new
                {
                    Id = x.ID,
                    FromUserID = x.MaShop,
                    Text = x.Text,
                    CreatedDate = x.NgayTao.Value,
                    FromUserName = x.Shop.TenShop,
                    TenThanhVien = tv.HoTen,
                    Client = x.Client


                }) ;
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
            Shop shop = Session["CuaHang"] as Shop;
            if (SIde == "Client")
            {
                Chat chat = db.Chats.ToList().LastOrDefault();
                if (chat != null && !chat.DaXem.Value)
                {
                    chat.DaXem = true;
                    db.SaveChanges();
                }
                Chat Chat = new Chat();
                Chat.DaXem = false;
                Chat.MaThanhVien = FromUserId;
                Chat.MaShop = ToUserId;
                Chat.Text = Text;
                Chat.NgayTao = DateTime.Now;
                Chat.Client = true;
                db.Chats.Add(Chat);
                db.SaveChanges();
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Chat Chat = new Chat();

                Chat.MaShop = shop.MaShop;
                Chat.MaThanhVien = ToUserId;
                Chat.Text = Text;
                Chat.NgayTao = DateTime.Now;
                Chat.DaXem = true;
                Chat.Client = false;
                db.Chats.Add(Chat);
                db.SaveChanges();
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Chating(int WithUserId)
        {
            Shop shop = Session["CuaHang"] as Shop;
            IEnumerable<Chat> listChat;
           

                listChat = db.Chats.Where(x => x.MaThanhVien == WithUserId || x.MaShop == WithUserId).OrderBy(x => x.NgayTao).ToList();

                ViewBag.TenShop = shop.TenShop;
            ViewBag.MaShop = shop.MaShop;
                return View(listChat);

            

            
        }

        [AllowAnonymous]
        public JsonResult GetNotiMessage()
        {
            Shop shop = Session["CuaHang"] as Shop;
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            try
            {
                
                var listMessage = db.Chats.Where(x => x.DaXem == false && x.MaShop == shop.MaShop ).ToList().Select(x => new { ID = x.ID, FromUserID = x.MaThanhVien, FromUserAvatar = "user.png", FromUserName = x.ThanhVien.HoTen , CreatedDate = (DateTime.Now - x.NgayTao.Value).Minutes }); ;
                return Json(listMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        public JsonResult GetNotiComment()
        {
            Shop shop = Session["CuaHang"] as Shop;
            var listComment = db.BinhLuans.Where(n=>n.DaXem==false && n.SanPham.MaShop == shop.MaShop).ToList().Select(x=> new { ID = x.MaBL, NdBL = x.NoiDungBL, MaSP = x.MaSP ,UserId = x.MaCTDDH, NgayTao = (DateTime.Now - x.NgayTao.Value).Minutes});
            return Json(listComment, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ChatWithShop(int WithUserId)
        {
            IEnumerable<Chat> listChat;
          

                listChat = db.Chats.Where(x => x.MaShop == WithUserId || x.MaThanhVien == WithUserId).OrderBy(x => x.NgayTao).ToList();

                ViewBag.TenShop = db.Shops.Find(WithUserId).TenShop;
                return View(listChat);

           
        }

      
        public ActionResult ChatHistory()
        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            List<Chat> listChatShop = db.Chats.Where(n=>n.MaThanhVien == tv.MaThanhVien).ToList();  

            return View();
        }

        public ActionResult ChatInHeader()
        {
            ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
            List<int> termsList = new List<int>();
            List<Chat> newChat = new List<Chat>();
            List<Chat> listChatShop = db.Chats.Where(n => n.MaThanhVien == tv.MaThanhVien).ToList();
            
                foreach (var item in listChatShop)
                {
                int Mashop = 1;
                    termsList.Add(Mashop);
                }
                var numbers = new List<int>() { 10, 4, 8, 8 };
                var list1 = termsList.Distinct().ToList();
                for (int i = 0; i < list1.Count; i++)
                {
                    int MaShop = list1[i];
                Chat chat = db.Chats.Where(n => n.MaThanhVien == tv.MaThanhVien && n.MaShop == MaShop).OrderByDescending(n => n.NgayTao).First();
                    newChat.Add(chat);
                }
            
            


            return PartialView(newChat);

        }


    }
}
