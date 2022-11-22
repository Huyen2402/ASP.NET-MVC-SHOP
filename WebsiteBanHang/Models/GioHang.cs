﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebsiteBanHang.Models
{
    public class GioHang
    {

        public string TenSP { get; set; } 
        public int MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal Dongia { get; set; }
        public string HinhAnh { get; set; }
        public decimal ThanhTien { get; set; }
        public int MaShop { get; set; }
        public string TenShop { get; set; }
        public GioHang (int MaSP, int sl)
        {
            using (Entities db = new Entities()) 
            {
                
                this.MaSP = MaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == MaSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.Dongia = (decimal)sp.DonGia.Value;
                this.SoLuong = sl;
                this.MaShop = (int)sp.MaShop;
                this.ThanhTien = Dongia * SoLuong;
            }

            



        }
        public GioHang(int MaSP, int? MaShop)
        {
            using (Entities db = new Entities())
            {
                this.MaSP = MaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == MaSP);
               
                this.MaShop = (int)MaShop;
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.Dongia = (decimal)sp.DonGia.Value;
                this.TenShop = sp.Shop.TenShop;
                this.SoLuong = 1;
                this.ThanhTien = Dongia * SoLuong;
            } 
        }
        
        public GioHang()
        {

        }

        public GioHang(int MaSP)
        {
            using (Entities db = new Entities())
            {

                this.MaSP = MaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == MaSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.Dongia = (decimal)sp.DonGia.Value;
                this.SoLuong = 1;
                this.MaShop = (int)sp.MaShop;
                this.TenShop = sp.Shop.TenShop;
                this.ThanhTien = Dongia * SoLuong;
            }





        }
    }
}