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
    
    public partial class FlashSale
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FlashSale()
        {
            this.ChiTietFlashSale = new HashSet<ChiTietFlashSale>();
        }
    
        public int MaSale { get; set; }
        public Nullable<System.DateTime> NgaySale { get; set; }
        public string ThoiGianSale { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietFlashSale> ChiTietFlashSale { get; set; }
    }
}
