using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class SearchController : Controller
    {
        Entities db = new Entities();
        // GET: Search
        [HttpPost]
        public ActionResult SearchSP(string txbSearch)
        {
            List<SanPham> listSearch = db.SanPhams.Where(s => s.TenSP.Contains(txbSearch) && s.DaXoa == false).ToList();
            ViewBag.listshop = db.Shops.Where(n=>n.TenShop.Contains(txbSearch)).ToList();

            return View(listSearch);
        }

        public ActionResult SearchSPPartial()
        {
            return PartialView();
        }
    }
}