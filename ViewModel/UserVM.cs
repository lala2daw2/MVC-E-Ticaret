using eTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace eTrade.ViewModel
{
    public class UserVM
    {
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string EMail { get; set; }

        public MembershipUser Kullanicilar { get; set; }
        public Musteri Musteriler { get; set; }
        public MusteriAdresler Adres { get; set; }
        public Resimler Resim { get; set; }


    }
}