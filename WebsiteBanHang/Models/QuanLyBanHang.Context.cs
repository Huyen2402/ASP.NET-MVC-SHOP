﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebsiteBanHang.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BinhLuan> BinhLuans { get; set; }
        public virtual DbSet<ChatwithShop> ChatwithShops { get; set; }
        public virtual DbSet<ChiTietDonDatHang> ChiTietDonDatHangs { get; set; }
        public virtual DbSet<ChiTietFlashSale> ChiTietFlashSales { get; set; }
        public virtual DbSet<ChiTietGiamGia> ChiTietGiamGias { get; set; }
        public virtual DbSet<ChiTietMatHangKinhDoanh> ChiTietMatHangKinhDoanhs { get; set; }
        public virtual DbSet<DiaChi> DiaChis { get; set; }
        public virtual DbSet<DonDatHang> DonDatHangs { get; set; }
        public virtual DbSet<FlashSale> FlashSales { get; set; }
        public virtual DbSet<GiamGia> GiamGias { get; set; }
        public virtual DbSet<HangThanhVien> HangThanhViens { get; set; }
        public virtual DbSet<Huyen> Huyens { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<KichCo> KichCos { get; set; }
        public virtual DbSet<loaiSanPham> loaiSanPhams { get; set; }
        public virtual DbSet<LoaiThanhVien> LoaiThanhViens { get; set; }
        public virtual DbSet<MatHangKinhDoanh> MatHangKinhDoanhs { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public virtual DbSet<NhaSanXuat> NhaSanXuats { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<ThanhVien> ThanhViens { get; set; }
        public virtual DbSet<Tinh> Tinhs { get; set; }
        public virtual DbSet<TinhTrangGiaoHang> TinhTrangGiaoHangs { get; set; }
        public virtual DbSet<TraLoiBinhLuan> TraLoiBinhLuans { get; set; }
        public virtual DbSet<VideoQC> VideoQCs { get; set; }
        public virtual DbSet<Xa> Xas { get; set; }
    }
}
