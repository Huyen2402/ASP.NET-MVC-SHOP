//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class TraLoiBinhLuan
    {
        public int MaTraLoiBL { get; set; }
        public string NoiDungTraLoi { get; set; }
        public Nullable<int> MaBL { get; set; }
        public Nullable<int> MaThanhVien { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string MaSP { get; set; }
    
        public virtual BinhLuan BinhLuan { get; set; }
    }
}
