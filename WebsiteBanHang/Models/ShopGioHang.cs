using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteBanHang.Models
{
    public class ShopGioHang
    {
       
        public int MaShop { get; set; }
       
        public List<GioHang> ListGioHang { get; set; }

        public ShopGioHang()
        {

        }
        public ShopGioHang(int MaShop)
        {
            this.MaShop = MaShop;
            this.ListGioHang = new List<GioHang>();
        }
    }
}