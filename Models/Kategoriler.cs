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
    
    public partial class Kategoriler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kategoriler()
        {
            this.Urunlers = new HashSet<Urunler>();
            this.UrunOzellikTips = new HashSet<UrunOzellikTip>();
        }
    
        public int ID { get; set; }
        public string KategoriAd { get; set; }
        public string Aciklama { get; set; }
        public Nullable<int> ResimID { get; set; }
    
        public virtual Resimler Resimler { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urunler> Urunlers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UrunOzellikTip> UrunOzellikTips { get; set; }
    }
}