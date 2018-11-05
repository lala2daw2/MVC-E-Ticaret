using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTrade.Models;
using System.Data.Entity;

namespace eTrade.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("Admin")]
    public class SatislarController : Controller
    {
        // GET: Satislar
        [Route("Satis/BekleyenSatislar")]
        public ActionResult BekleyenSiparisler()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {

                return View(db.Satislars.Include(x=>x.SiparisDurum).Include(x=>x.Kargolar).Include(x=>x.Musteri).Where(x=>x.SiparisDurumID==1).ToList());

            }
        }
        [Route("Satis/TamamlananSatislar")]
        public ActionResult GecmisSiparisler()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                return View(db.Satislars.Include(x=>x.Musteri).Include(x=>x.SiparisDurum).Include(x=>x.Kargolar).Include(x=>x.SiparisDurum).Where(x => x.SiparisDurumID == 3).ToList());
            }
        }
        [Route("Satis/KargodakiSatislar")]
        public ActionResult KargodaOlanSiparisler()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                return View(db.Satislars.Include(x => x.Musteri).Include(x => x.SiparisDurum).Include(x => x.Kargolar).Include(x => x.SiparisDurum).Where(x => x.SiparisDurumID == 2).ToList());
            }
        }
        [Route("~/SatisGüncelle/{name}/{id}")]
        public ActionResult DurumGuncelle(int id,string name)
        {
            using (eTicaretEntities db= new eTicaretEntities())
            {
                var SiparisDurums = db.SiparisDurums.ToList();
                ViewBag.Durum = new SelectList(SiparisDurums, "ID", "Ad");
                var model = db.Satislars.Include(x=>x.Musteri).Where(x => x.ID == id).SingleOrDefault();
                if (model == null)
                    return HttpNotFound();
                return View(model);
            }
        }
        [Route("~/SatisGüncelle/{name}/{id}")]
        [HttpPost]
        public ActionResult DurumGuncelle (int id,int SiparisDurumID)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.Satislars.Where(x => x.ID == id).SingleOrDefault();
                if (model == null)
                    return HttpNotFound();
                model.SiparisDurumID = SiparisDurumID;
                db.SaveChanges();
                return RedirectToAction("BekleyenSiparisler");
            }
        }
    }
}