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
    
    public partial class DonDatHangs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonDatHangs()
        {
            this.ChiTietDonDatHangs = new HashSet<ChiTietDonDatHangs>();
        }
    
        public int MaDDH { get; set; }
        public Nullable<System.DateTime> NgayDat { get; set; }
        public Nullable<bool> TinhTrangGiaoHang { get; set; }
        public Nullable<System.DateTime> NgayGiao { get; set; }
        public string DaThanhToan { get; set; }
        public Nullable<int> MaKH { get; set; }
        public Nullable<int> UuDai { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonDatHangs> ChiTietDonDatHangs { get; set; }
        public virtual KhachHangs KhachHangs { get; set; }
    }
}
