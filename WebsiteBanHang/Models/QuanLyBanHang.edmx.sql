
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/28/2022 15:50:37
-- Generated from EDMX file: D:\Projects\ASP.NET-MVC-SHOP\WebsiteBanHang\Models\QuanLyBanHang.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [QuanLyBanHang];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_BinhLuan_SanPham]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BinhLuan] DROP CONSTRAINT [FK_BinhLuan_SanPham];
GO
IF OBJECT_ID(N'[dbo].[FK_BinhLuan_TThanhVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BinhLuan] DROP CONSTRAINT [FK_BinhLuan_TThanhVien];
GO
IF OBJECT_ID(N'[dbo].[FK_ChiTietDonDatHang_DonDatHang]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietDonDatHang] DROP CONSTRAINT [FK_ChiTietDonDatHang_DonDatHang];
GO
IF OBJECT_ID(N'[dbo].[FK_ChiTietDonDatHang_SanPham]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietDonDatHang] DROP CONSTRAINT [FK_ChiTietDonDatHang_SanPham];
GO
IF OBJECT_ID(N'[dbo].[FK_ChiTietPhieuNhap_PhieuNhap]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietPhieuNhap] DROP CONSTRAINT [FK_ChiTietPhieuNhap_PhieuNhap];
GO
IF OBJECT_ID(N'[dbo].[FK_ChiTietPhieuNhap_SanPham]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietPhieuNhap] DROP CONSTRAINT [FK_ChiTietPhieuNhap_SanPham];
GO
IF OBJECT_ID(N'[dbo].[FK_DonDatHang_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DonDatHang] DROP CONSTRAINT [FK_DonDatHang_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_KhachHang_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KhachHang] DROP CONSTRAINT [FK_KhachHang_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_PhieuNhap_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PhieuNhap] DROP CONSTRAINT [FK_PhieuNhap_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_SanPham_LoaiSanPham]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SanPham] DROP CONSTRAINT [FK_SanPham_LoaiSanPham];
GO
IF OBJECT_ID(N'[dbo].[FK_SanPham_NhaCungCap]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SanPham] DROP CONSTRAINT [FK_SanPham_NhaCungCap];
GO
IF OBJECT_ID(N'[dbo].[FK_SanPham_NhaSanXuat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SanPham] DROP CONSTRAINT [FK_SanPham_NhaSanXuat];
GO
IF OBJECT_ID(N'[dbo].[FK_ThanhVien_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThanhVien] DROP CONSTRAINT [FK_ThanhVien_ToTable];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[BinhLuan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BinhLuan];
GO
IF OBJECT_ID(N'[dbo].[ChiTietDonDatHang]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChiTietDonDatHang];
GO
IF OBJECT_ID(N'[dbo].[ChiTietPhieuNhap]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChiTietPhieuNhap];
GO
IF OBJECT_ID(N'[dbo].[DonDatHang]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DonDatHang];
GO
IF OBJECT_ID(N'[dbo].[KhachHang]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KhachHang];
GO
IF OBJECT_ID(N'[dbo].[loaiSanPham]', 'U') IS NOT NULL
    DROP TABLE [dbo].[loaiSanPham];
GO
IF OBJECT_ID(N'[dbo].[LoaiThanhVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LoaiThanhVien];
GO
IF OBJECT_ID(N'[dbo].[NhaCungCap]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NhaCungCap];
GO
IF OBJECT_ID(N'[dbo].[NhaSanXuat]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NhaSanXuat];
GO
IF OBJECT_ID(N'[dbo].[PhieuNhap]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PhieuNhap];
GO
IF OBJECT_ID(N'[dbo].[SanPham]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SanPham];
GO
IF OBJECT_ID(N'[dbo].[ThanhVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ThanhVien];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'BinhLuans'
CREATE TABLE [dbo].[BinhLuans] (
    [MaBL] int IDENTITY(1,1) NOT NULL,
    [NoiDungBL] nvarchar(max)  NULL,
    [MaThanhVien] int  NULL,
    [MaSP] int  NULL
);
GO

-- Creating table 'ChiTietDonDatHangs'
CREATE TABLE [dbo].[ChiTietDonDatHangs] (
    [MaChiTietDDH] int  NOT NULL,
    [MaDDH] int  NULL,
    [MaSP] int  NULL,
    [TenSP] nvarchar(255)  NULL,
    [SoLuong] int  NULL,
    [DonGia] decimal(18,0)  NULL
);
GO

-- Creating table 'ChiTietPhieuNhaps'
CREATE TABLE [dbo].[ChiTietPhieuNhaps] (
    [MaChiTietPN] int IDENTITY(1,1) NOT NULL,
    [MaPN] int  NULL,
    [MaSP] int  NULL,
    [DonGiaNhap] decimal(18,0)  NULL,
    [SoLuongNhap] int  NULL
);
GO

-- Creating table 'DonDatHangs'
CREATE TABLE [dbo].[DonDatHangs] (
    [MaDDH] int IDENTITY(1,1) NOT NULL,
    [NgayDat] datetime  NULL,
    [TinhTrangGiaoHang] bit  NULL,
    [NgayGiao] datetime  NULL,
    [DaThanhToan] nchar(10)  NULL,
    [MaKH] int  NULL,
    [UuDai] int  NULL
);
GO

-- Creating table 'KhachHangs'
CREATE TABLE [dbo].[KhachHangs] (
    [MaKH] int IDENTITY(1,1) NOT NULL,
    [TenKH] nvarchar(100)  NULL,
    [DiaChi] nvarchar(255)  NULL,
    [Email] nvarchar(255)  NULL,
    [SDT] varchar(12)  NULL,
    [MaThanhVien] int  NULL
);
GO

-- Creating table 'loaiSanPhams'
CREATE TABLE [dbo].[loaiSanPhams] (
    [MaLoaiSP] int IDENTITY(1,1) NOT NULL,
    [TenLoai] nvarchar(255)  NULL,
    [Icon] nvarchar(max)  NULL,
    [BiDanh] nvarchar(max)  NULL
);
GO

-- Creating table 'LoaiThanhViens'
CREATE TABLE [dbo].[LoaiThanhViens] (
    [MaLoaiTV] int IDENTITY(1,1) NOT NULL,
    [TenLoai] nvarchar(50)  NULL,
    [UuDai] int  NULL
);
GO

-- Creating table 'NhaCungCaps'
CREATE TABLE [dbo].[NhaCungCaps] (
    [MaNCC] int IDENTITY(1,1) NOT NULL,
    [TenNCC] nvarchar(100)  NULL,
    [DiaChi] nvarchar(255)  NULL,
    [Email] nvarchar(255)  NULL,
    [SDT] varchar(12)  NULL,
    [Fax] nvarchar(50)  NULL
);
GO

-- Creating table 'NhaSanXuats'
CREATE TABLE [dbo].[NhaSanXuats] (
    [MaNSX] int IDENTITY(1,1) NOT NULL,
    [TenNSX] nvarchar(100)  NULL,
    [ThongTin] nvarchar(255)  NULL,
    [Logo] nvarchar(max)  NULL
);
GO

-- Creating table 'PhieuNhaps'
CREATE TABLE [dbo].[PhieuNhaps] (
    [MaPN] int IDENTITY(1,1) NOT NULL,
    [MaNCC] int  NULL,
    [NgayNhap] datetime  NULL,
    [DaXoa] bit  NULL
);
GO

-- Creating table 'SanPhams'
CREATE TABLE [dbo].[SanPhams] (
    [MaSP] int IDENTITY(1,1) NOT NULL,
    [TenSP] nvarchar(255)  NULL,
    [DonGia] decimal(18,0)  NULL,
    [NgayCapNhat] datetime  NULL,
    [CauHinh] nvarchar(max)  NULL,
    [MoTa] nvarchar(max)  NULL,
    [HinhAnh] nvarchar(max)  NULL,
    [SoLuongTon] int  NULL,
    [LuotXem] int  NULL,
    [LuotBinhChon] int  NULL,
    [LuotBinhLuan] int  NULL,
    [SoLanMua] int  NULL,
    [Moi] int  NULL,
    [MaNCC] int  NULL,
    [MaNSX] int  NULL,
    [MaLoaiSP] int  NULL,
    [DaXoa] bit  NULL,
    [HinhAnh1] nvarchar(max)  NULL,
    [HinhAnh2] nvarchar(max)  NULL,
    [HinhAnh3] nvarchar(max)  NULL,
    [HinhAnh4] nvarchar(max)  NULL
);
GO

-- Creating table 'ThanhViens'
CREATE TABLE [dbo].[ThanhViens] (
    [MaThanhVien] int IDENTITY(1,1) NOT NULL,
    [TaiKhoan] nvarchar(100)  NULL,
    [MatKhau] nvarchar(100)  NULL,
    [HoTen] nvarchar(100)  NULL,
    [DiaChi] nvarchar(255)  NULL,
    [Email] nvarchar(255)  NULL,
    [SDT] varchar(12)  NULL,
    [CauHoi] nvarchar(max)  NULL,
    [CauTraLoi] nvarchar(max)  NULL,
    [MaLoaiTV] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [MaBL] in table 'BinhLuans'
ALTER TABLE [dbo].[BinhLuans]
ADD CONSTRAINT [PK_BinhLuans]
    PRIMARY KEY CLUSTERED ([MaBL] ASC);
GO

-- Creating primary key on [MaChiTietDDH] in table 'ChiTietDonDatHangs'
ALTER TABLE [dbo].[ChiTietDonDatHangs]
ADD CONSTRAINT [PK_ChiTietDonDatHangs]
    PRIMARY KEY CLUSTERED ([MaChiTietDDH] ASC);
GO

-- Creating primary key on [MaChiTietPN] in table 'ChiTietPhieuNhaps'
ALTER TABLE [dbo].[ChiTietPhieuNhaps]
ADD CONSTRAINT [PK_ChiTietPhieuNhaps]
    PRIMARY KEY CLUSTERED ([MaChiTietPN] ASC);
GO

-- Creating primary key on [MaDDH] in table 'DonDatHangs'
ALTER TABLE [dbo].[DonDatHangs]
ADD CONSTRAINT [PK_DonDatHangs]
    PRIMARY KEY CLUSTERED ([MaDDH] ASC);
GO

-- Creating primary key on [MaKH] in table 'KhachHangs'
ALTER TABLE [dbo].[KhachHangs]
ADD CONSTRAINT [PK_KhachHangs]
    PRIMARY KEY CLUSTERED ([MaKH] ASC);
GO

-- Creating primary key on [MaLoaiSP] in table 'loaiSanPhams'
ALTER TABLE [dbo].[loaiSanPhams]
ADD CONSTRAINT [PK_loaiSanPhams]
    PRIMARY KEY CLUSTERED ([MaLoaiSP] ASC);
GO

-- Creating primary key on [MaLoaiTV] in table 'LoaiThanhViens'
ALTER TABLE [dbo].[LoaiThanhViens]
ADD CONSTRAINT [PK_LoaiThanhViens]
    PRIMARY KEY CLUSTERED ([MaLoaiTV] ASC);
GO

-- Creating primary key on [MaNCC] in table 'NhaCungCaps'
ALTER TABLE [dbo].[NhaCungCaps]
ADD CONSTRAINT [PK_NhaCungCaps]
    PRIMARY KEY CLUSTERED ([MaNCC] ASC);
GO

-- Creating primary key on [MaNSX] in table 'NhaSanXuats'
ALTER TABLE [dbo].[NhaSanXuats]
ADD CONSTRAINT [PK_NhaSanXuats]
    PRIMARY KEY CLUSTERED ([MaNSX] ASC);
GO

-- Creating primary key on [MaPN] in table 'PhieuNhaps'
ALTER TABLE [dbo].[PhieuNhaps]
ADD CONSTRAINT [PK_PhieuNhaps]
    PRIMARY KEY CLUSTERED ([MaPN] ASC);
GO

-- Creating primary key on [MaSP] in table 'SanPhams'
ALTER TABLE [dbo].[SanPhams]
ADD CONSTRAINT [PK_SanPhams]
    PRIMARY KEY CLUSTERED ([MaSP] ASC);
GO

-- Creating primary key on [MaThanhVien] in table 'ThanhViens'
ALTER TABLE [dbo].[ThanhViens]
ADD CONSTRAINT [PK_ThanhViens]
    PRIMARY KEY CLUSTERED ([MaThanhVien] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MaSP] in table 'BinhLuans'
ALTER TABLE [dbo].[BinhLuans]
ADD CONSTRAINT [FK_BinhLuan_SanPham]
    FOREIGN KEY ([MaSP])
    REFERENCES [dbo].[SanPhams]
        ([MaSP])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BinhLuan_SanPham'
CREATE INDEX [IX_FK_BinhLuan_SanPham]
ON [dbo].[BinhLuans]
    ([MaSP]);
GO

-- Creating foreign key on [MaThanhVien] in table 'BinhLuans'
ALTER TABLE [dbo].[BinhLuans]
ADD CONSTRAINT [FK_BinhLuan_TThanhVien]
    FOREIGN KEY ([MaThanhVien])
    REFERENCES [dbo].[ThanhViens]
        ([MaThanhVien])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BinhLuan_TThanhVien'
CREATE INDEX [IX_FK_BinhLuan_TThanhVien]
ON [dbo].[BinhLuans]
    ([MaThanhVien]);
GO

-- Creating foreign key on [MaDDH] in table 'ChiTietDonDatHangs'
ALTER TABLE [dbo].[ChiTietDonDatHangs]
ADD CONSTRAINT [FK_ChiTietDonDatHang_DonDatHang]
    FOREIGN KEY ([MaDDH])
    REFERENCES [dbo].[DonDatHangs]
        ([MaDDH])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChiTietDonDatHang_DonDatHang'
CREATE INDEX [IX_FK_ChiTietDonDatHang_DonDatHang]
ON [dbo].[ChiTietDonDatHangs]
    ([MaDDH]);
GO

-- Creating foreign key on [MaSP] in table 'ChiTietDonDatHangs'
ALTER TABLE [dbo].[ChiTietDonDatHangs]
ADD CONSTRAINT [FK_ChiTietDonDatHang_SanPham]
    FOREIGN KEY ([MaSP])
    REFERENCES [dbo].[SanPhams]
        ([MaSP])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChiTietDonDatHang_SanPham'
CREATE INDEX [IX_FK_ChiTietDonDatHang_SanPham]
ON [dbo].[ChiTietDonDatHangs]
    ([MaSP]);
GO

-- Creating foreign key on [MaPN] in table 'ChiTietPhieuNhaps'
ALTER TABLE [dbo].[ChiTietPhieuNhaps]
ADD CONSTRAINT [FK_ChiTietPhieuNhap_PhieuNhap]
    FOREIGN KEY ([MaPN])
    REFERENCES [dbo].[PhieuNhaps]
        ([MaPN])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChiTietPhieuNhap_PhieuNhap'
CREATE INDEX [IX_FK_ChiTietPhieuNhap_PhieuNhap]
ON [dbo].[ChiTietPhieuNhaps]
    ([MaPN]);
GO

-- Creating foreign key on [MaSP] in table 'ChiTietPhieuNhaps'
ALTER TABLE [dbo].[ChiTietPhieuNhaps]
ADD CONSTRAINT [FK_ChiTietPhieuNhap_SanPham]
    FOREIGN KEY ([MaSP])
    REFERENCES [dbo].[SanPhams]
        ([MaSP])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChiTietPhieuNhap_SanPham'
CREATE INDEX [IX_FK_ChiTietPhieuNhap_SanPham]
ON [dbo].[ChiTietPhieuNhaps]
    ([MaSP]);
GO

-- Creating foreign key on [MaKH] in table 'DonDatHangs'
ALTER TABLE [dbo].[DonDatHangs]
ADD CONSTRAINT [FK_DonDatHang_ToTable]
    FOREIGN KEY ([MaKH])
    REFERENCES [dbo].[KhachHangs]
        ([MaKH])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DonDatHang_ToTable'
CREATE INDEX [IX_FK_DonDatHang_ToTable]
ON [dbo].[DonDatHangs]
    ([MaKH]);
GO

-- Creating foreign key on [MaThanhVien] in table 'KhachHangs'
ALTER TABLE [dbo].[KhachHangs]
ADD CONSTRAINT [FK_KhachHang_ToTable]
    FOREIGN KEY ([MaThanhVien])
    REFERENCES [dbo].[ThanhViens]
        ([MaThanhVien])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_KhachHang_ToTable'
CREATE INDEX [IX_FK_KhachHang_ToTable]
ON [dbo].[KhachHangs]
    ([MaThanhVien]);
GO

-- Creating foreign key on [MaLoaiSP] in table 'SanPhams'
ALTER TABLE [dbo].[SanPhams]
ADD CONSTRAINT [FK_SanPham_LoaiSanPham]
    FOREIGN KEY ([MaLoaiSP])
    REFERENCES [dbo].[loaiSanPhams]
        ([MaLoaiSP])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SanPham_LoaiSanPham'
CREATE INDEX [IX_FK_SanPham_LoaiSanPham]
ON [dbo].[SanPhams]
    ([MaLoaiSP]);
GO

-- Creating foreign key on [MaLoaiTV] in table 'ThanhViens'
ALTER TABLE [dbo].[ThanhViens]
ADD CONSTRAINT [FK_ThanhVien_ToTable]
    FOREIGN KEY ([MaLoaiTV])
    REFERENCES [dbo].[LoaiThanhViens]
        ([MaLoaiTV])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ThanhVien_ToTable'
CREATE INDEX [IX_FK_ThanhVien_ToTable]
ON [dbo].[ThanhViens]
    ([MaLoaiTV]);
GO

-- Creating foreign key on [MaNCC] in table 'PhieuNhaps'
ALTER TABLE [dbo].[PhieuNhaps]
ADD CONSTRAINT [FK_PhieuNhap_ToTable]
    FOREIGN KEY ([MaNCC])
    REFERENCES [dbo].[NhaCungCaps]
        ([MaNCC])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PhieuNhap_ToTable'
CREATE INDEX [IX_FK_PhieuNhap_ToTable]
ON [dbo].[PhieuNhaps]
    ([MaNCC]);
GO

-- Creating foreign key on [MaNCC] in table 'SanPhams'
ALTER TABLE [dbo].[SanPhams]
ADD CONSTRAINT [FK_SanPham_NhaCungCap]
    FOREIGN KEY ([MaNCC])
    REFERENCES [dbo].[NhaCungCaps]
        ([MaNCC])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SanPham_NhaCungCap'
CREATE INDEX [IX_FK_SanPham_NhaCungCap]
ON [dbo].[SanPhams]
    ([MaNCC]);
GO

-- Creating foreign key on [MaNSX] in table 'SanPhams'
ALTER TABLE [dbo].[SanPhams]
ADD CONSTRAINT [FK_SanPham_NhaSanXuat]
    FOREIGN KEY ([MaNSX])
    REFERENCES [dbo].[NhaSanXuats]
        ([MaNSX])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SanPham_NhaSanXuat'
CREATE INDEX [IX_FK_SanPham_NhaSanXuat]
ON [dbo].[SanPhams]
    ([MaNSX]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------