﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class eTicaretEntities : DbContext
    {
        public eTicaretEntities()
            : base("name=eTicaretEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<aspnet_Roles> aspnet_Roles { get; set; }
        public virtual DbSet<aspnet_Users> aspnet_Users { get; set; }
        public virtual DbSet<Kargolar> Kargolars { get; set; }
        public virtual DbSet<Kategoriler> Kategorilers { get; set; }
        public virtual DbSet<Markalar> Markalars { get; set; }
        public virtual DbSet<Musteri> Musteris { get; set; }
        public virtual DbSet<MusteriAdresler> MusteriAdreslers { get; set; }
        public virtual DbSet<OzellikDeger> OzellikDegers { get; set; }
        public virtual DbSet<Resimler> Resimlers { get; set; }
        public virtual DbSet<SatisDetay> SatisDetays { get; set; }
        public virtual DbSet<Satislar> Satislars { get; set; }
        public virtual DbSet<SiparisDurum> SiparisDurums { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Urunler> Urunlers { get; set; }
        public virtual DbSet<UrunOzellik> UrunOzelliks { get; set; }
        public virtual DbSet<UrunOzellikTip> UrunOzellikTips { get; set; }
    }
}