//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eTrade.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Satislar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Satislar()
        {
            this.SatisDetays = new HashSet<SatisDetay>();
        }
    
        public int ID { get; set; }
        public Nullable<System.Guid> MusteriID { get; set; }
        public Nullable<System.DateTime> SatisTarihi { get; set; }
        public Nullable<decimal> ToplamTutar { get; set; }
        public Nullable<bool> Sepettemi { get; set; }
        public Nullable<int> KargoID { get; set; }
        public Nullable<int> SiparisDurumID { get; set; }
    
        public virtual Kargolar Kargolar { get; set; }
        public virtual Musteri Musteri { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SatisDetay> SatisDetays { get; set; }
        public virtual SiparisDurum SiparisDurum { get; set; }
    }
}
