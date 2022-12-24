using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteBanHang.Models
{
    public class DatHang
    {
        public int id { get; set; }
        public decimal ThanhTien { get; set; }
        public int MaShop { get; set; }
        public int MaCTGiamGia { get; set; }

        public DatHang() {

        }
        public DatHang(int id, decimal ThanhTien, int MaShop, int MaCTGiamGia)
        {
            this.id = id;
            this.ThanhTien= ThanhTien;
            this.MaShop= MaShop;
            this.MaCTGiamGia= MaCTGiamGia;
        }
    }
}