using eTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTrade.ViewModel
{
    public class UrunVM
    {
        public IEnumerable<Resimler> Resim { get; set; }
        public Urunler Urunler { get; set; }
        public IEnumerable<Markalar> Marka { get; set; }
        public IEnumerable<Kategoriler> Kategori { get; set; }
        public List<UrunOzellik> Ozellik { get; set; }

    }
}