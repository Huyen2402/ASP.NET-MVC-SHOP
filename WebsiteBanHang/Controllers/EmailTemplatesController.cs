using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class EmailTemplatesController : Controller
    {
        Entities db = new Entities();
        // GET: EmailTemplates
        public ActionResult Customer()
        {
            return View();
        }
        public ActionResult DonHang()
        {

            DatHang dh = Session["DatHang"] as DatHang;


            List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];
            List<GioHang> listRender = new List<GioHang>();
            for (int i = 0; i < listGioHang.Count; i++)
            {
                if (listGioHang[i].MaShop == dh.MaShop)
                {
                    GioHang ct = new GioHang(listGioHang[i].MaSP, listGioHang[i].SoLuong);
                    listRender.Add(ct);


                }
            }


            return View(listRender);
           
        }
        public ActionResult QuangCao()
        {
            return View();

        }

    }
}