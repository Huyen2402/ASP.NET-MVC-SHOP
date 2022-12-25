using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteBanHang.Models
{
    public class SessionDiaChi
    {
        public int MaTinh { get; set; }
        public int MaHuyen { get; set; }
        public int MaXa { get; set; }
        public string DiaChi { get; set; }

        public SessionDiaChi() { }
        public SessionDiaChi(int MaTinh, int MaHuyen, int MaXa, string DiaChi) {
            this.MaTinh = MaTinh;
            this.MaHuyen = MaHuyen;
            this.MaXa = MaXa;
            this.DiaChi = DiaChi;
        }
    }
}