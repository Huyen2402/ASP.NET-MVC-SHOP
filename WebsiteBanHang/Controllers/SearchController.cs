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
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Search
        [HttpPost]
        public ActionResult SearchSP(string txbSearch)
        {
            List<SanPham> listSearch = db.SanPhams.Where(s => s.TenSP.Contains(txbSearch)).ToList();
            return View(listSearch);
        }

        public ActionResult SearchSPPartial()
        {
            return PartialView();
        }
    }
}