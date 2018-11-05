using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eTrade.Models;

namespace eTrade.ViewModel
{
    public class UrunListVM
    {
        public IEnumerable<Urunler> Urunler { get; set; }
        public IEnumerable<Resimler> Resim { get; set; }

    }
}