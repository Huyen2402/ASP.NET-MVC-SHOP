using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteBanHang.Controllers
{
    public class EmailTemplatesController : Controller
    {
        // GET: EmailTemplates
        public ActionResult Customer()
        {
            return View();
        }
        public ActionResult DonHang()
        {
            return View();

        }
        public ActionResult QuangCao()
        {
            return View();

        }

    }
}