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
    
    public partial class GiamGia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GiamGia()
        {
            this.ChiTietGiamGia = new HashSet<ChiTietGiamGia>();
        }
    
        public int MaGiamGia { get; set; }
        public string TenGiamGia { get; set; }
        public Nullable<decimal> SoTien { get; set; }
        public Nullable<decimal> ToiThieu { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietGiamGia> ChiTietGiamGia { get; set; }
    }
}
