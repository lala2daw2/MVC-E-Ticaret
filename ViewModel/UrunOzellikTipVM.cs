using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eTrade.Models;

namespace eTrade.ViewModel
{
    public class UrunOzellikTipVM
    {
        public IEnumerable<Kategoriler> Kategoriler { get; set; }
        public UrunOzellikTip Tip { get; set; }

    }
}