using eTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace eTrade.App_Start
{
    public class Sepet
    {
        public static Sepet AktifSepet
        {
            get
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx.Session["cart"] == null)
                    ctx.Session["cart"] = new Sepet();
                return (Sepet)ctx.Session["cart"];
            }
        }

        private List<SepetItem> urunler = new List<SepetItem>();

        public List<SepetItem> Urunler
        {
            get
            {
                return urunler;
            }
            set
            {
                urunler = value;
            }
        }
        
        

        public void Add(SepetItem si)
        {
            if (HttpContext.Current.Session["cart"] != null)
            {
                Sepet s = (Sepet)HttpContext.Current.Session["cart"];
                if (s.Urunler.Any(x => x.Urun.ID == si.Urun.ID))
                {
                    s.Urunler.FirstOrDefault(x => x.Urun.ID == si.Urun.ID).Adet++;
                }
                else
                    s.Urunler.Add(si);
            }
            else
            {
                Sepet s = new Sepet();
                s.Urunler.Add(si);
                HttpContext.Current.Session["cart"] = s;
            }
        }
        public decimal ToplamTutar
        {
            get
            {
                return Urunler.Sum(x => x.Tutar);
            }
        }


        public class SepetItem
        {
            public Urunler Urun { get; set; }
            public int Adet { get; set; }
            public double Indirim { get; set; }
            public decimal Tutar
            {
                get
                {
                    return (decimal)Urun.SatisFiyat * Adet ;
                }
                set { Tutar = Tutar; }
                
            }
            public override string ToString()
            {
                return Tutar.ToString();
            }


        }
    }
}