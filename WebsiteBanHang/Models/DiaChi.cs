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
    
    public partial class DiaChi
    {
        public int ID { get; set; }
        public Nullable<int> MaTinh { get; set; }
        public Nullable<int> MaHuyen { get; set; }
        public Nullable<int> MaXa { get; set; }
        public Nullable<int> MaThanhVien { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    
        public virtual Huyen Huyen { get; set; }
        public virtual ThanhVien ThanhVien { get; set; }
        public virtual Tinh Tinh { get; set; }
        public virtual Xa Xa { get; set; }
    }
}