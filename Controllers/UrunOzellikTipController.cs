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
    public class UrunOzellikTipController : Controller
    {
        // GET: UrunOzellik
        [Route("OzellikTip/Listesi")]
        public ActionResult List()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                return View(db.UrunOzellikTips.Include(x => x.Kategoriler).ToList());
            }
        }
        [Route("OzellikDeger/Listesi")]
        public ActionResult DegerListele()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                return View(db.OzellikDegers.Include(x => x.UrunOzellikTip.Kategoriler).Include(x => x.UrunOzellikTip).OrderByDescending(x => x.UrunOzellikTip.Kategoriler.KategoriAd).ToList());
            }
        }
        [Route("OzellikTip/YeniTip")]
        public ActionResult Create()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                UrunOzellikTipVM tip = new UrunOzellikTipVM()
                {
                    Kategoriler = db.Kategorilers.ToList()
                };
                return View(tip);
            }

        }
        [Route("OzellikTip/YeniTip")]
        [HttpPost]
        public ActionResult Create(UrunOzellikTipVM model)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                InfoVM info = new InfoVM();
                db.UrunOzellikTips.Add(model.Tip);
                info.Message = model.Tip.Adi + " başarıyla eklendi.";
                db.SaveChanges();
                info.Statu = true;
                info.LinkText = "Geri dön";
                info.Url = "/UrunOzellikTip/List";
                return View("_info", info);
            }
        }

        [Route("OzellikTip/Edit/{name}/{ID}")]
        public ActionResult Update(int ID,string name)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                UrunOzellikTipVM tipVM = new UrunOzellikTipVM()
                {
                    Kategoriler = db.Kategorilers.ToList(),
                    Tip = db.UrunOzellikTips.Where(x => x.ID == ID).SingleOrDefault()
                };
                if (tipVM.Tip == null)
                    return HttpNotFound();
                return View(tipVM);
            }
        }
        [HttpPost]
        [Route("OzellikTip/Edit/{name}/{ID}")]
        public ActionResult Update(int ID, UrunOzellikTipVM model)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var tip = db.UrunOzellikTips.Where(x => x.ID == ID).SingleOrDefault();
                if (tip == null)
                    return HttpNotFound();
                tip.Adi = model.Tip.Adi;
                tip.Aciklama = model.Tip.Aciklama;
                tip.KategoriID = model.Tip.KategoriID;
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }
        public ActionResult Delete(int ID)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.UrunOzellikTips.Where(x => x.ID == ID).SingleOrDefault();
                if (model == null)
                    return HttpNotFound();
                db.UrunOzellikTips.Remove(model);
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }
        [Route("DegerEkle/{name}/{ID}")]
        public ActionResult DegerEkle(int ID,string name)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                return View();
            }
        }
        [HttpPost]
        [Route("DegerEkle/{name}/{ID}")]
        public ActionResult DegerEkle(int ID, OzellikDeger model,string name)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                OzellikDeger deger = new OzellikDeger()
                {
                    Aciklama = model.Aciklama,
                    Adi = model.Adi,
                    OzellikTipID = ID
                };
                db.OzellikDegers.Add(deger);
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }
        [Route("OzellikTip/UrunListesi")]
        public ActionResult ÜrünList()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                return View(db.Urunlers.Include(x => x.Kategoriler).Include(x => x.Markalar).ToList());

            }
        }
        [Route("~/Urun/OzellikVer/{name}/{ID}")]
        public ActionResult OzellikVer(int ID,string name)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.Urunlers.Include(x => x.Kategoriler).Where(x => x.ID == ID).SingleOrDefault();
                var tip = db.UrunOzellikTips.Where(x => x.KategoriID == model.KategoriID).ToList();
                ViewBag.Tip = new SelectList(tip, "ID", "Adi");
                if (model == null)
                    return HttpNotFound();
                return View(model);
            }
        }
        [HttpPost]
        [Route("~/Urun/OzellikVer/{name}/{ID}")]
        public ActionResult OzellikVer(int ID, UrunOzellik oz)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {

                var result = db.UrunOzelliks.Where(x => x.OzellikDegerID == oz.OzellikDegerID && x.OzellikTipID == oz.OzellikTipID && x.UrunID == ID).FirstOrDefault();
                if (result == null)
                    ViewBag.Hata = "Daha önceden bu ürüne bu özellik yüklenmiş tekrar yüklenemez.";
                var urun = db.Urunlers.Where(x => x.ID == ID).SingleOrDefault();
                if (urun == null)
                    return HttpNotFound();
                else
                {
                    InfoVM info = new InfoVM();
                    db.UrunOzelliks.Add(oz);
                    oz.UrunID = ID;
                    info.Message ="Özellik başarıyla eklendi";
                    db.SaveChanges();
                    info.Statu = true;
                    info.LinkText = "Geri dönmek için tıklayınız";
                    info.Url = "/Admin/OzellikTip/UrunListesi";
                    return View("_info",info);
                }

            }
        }

        public PartialViewResult OzellikDegerWidget(int? ID)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.OzellikDegers.Where(x => x.OzellikTipID == ID).ToList();
                return PartialView(model);
            }
        }
    }
}