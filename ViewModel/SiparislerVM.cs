using eTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTrade.ViewModel
{
    public class SiparislerVM
    {
        public Satislar Satis { get; set; }
        public IEnumerable<SatisDetay> SDetay { get; set; }
    }
}