using eTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using eTrade.ViewModel;

namespace eTrade.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("Admin")]
    public class MusterilerController : Controller
    {
        // GET: Musteriler
        [Route("Musteri/Listesi")]
        public ActionResult List()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                ViewBag.ToplamTutar = db.Satislars.Sum(x => x.ToplamTutar);
                return View(db.Musteris.Include(x => x.Satislars).Include(x => x.MusteriAdreslers).Include(x => x.Resimler).OrderByDescending(x => x.Satislars.Sum(y => y.ToplamTutar)).ToList());
            }

        }
        [Route("Musteri/{id}")]
        public ActionResult OrderDetailByUsername(Guid id)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var s = db.Satislars.Include(x => x.SatisDetays).Include(x => x.SatisDetays.Select(z => z.Urunler)).Where(x => x.MusteriID == id).ToList();
                return View(s);
            }
        }
    }
}