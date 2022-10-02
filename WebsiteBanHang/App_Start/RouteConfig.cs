using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebsiteBanHang
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "DangKy",
              url: "dang-ky",
              defaults: new { controller = "Home", action = "DangKy" }
          );

            routes.MapRoute(
               name: "ChiTietSanPham",
               url: "san-pham/{seo-keyword}-{id}",
               defaults: new { controller = "SanPham", action = "XemChitietSP", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "TrangChu",
                url: "trang-chu",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
