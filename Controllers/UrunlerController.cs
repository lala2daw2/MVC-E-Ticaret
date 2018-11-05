using eTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using eTrade.ViewModel;
using System.Web.Helpers;
using System.IO;

namespace eTrade.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("Admin")]
    public class UrunlerController : Controller
    {
        // GET: Urunler
        [Route("Urun/Listesi")]
        public ActionResult List()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.Urunlers.Include(x => x.Markalar).Include(x => x.Kategoriler).ToList();
                return View(model);
            }
        }
        [Route("Urun/YeniUrun")]
        public ActionResult Create()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                UrunVM urun = new UrunVM()
                {
                    Kategori = db.Kategorilers.ToList(),
                    Marka = db.Markalars.ToList()
                };
                return View(urun);
            }
        }
        [HttpPost]
        [Route("Urun/YeniUrun")]
        public ActionResult Create(UrunVM model)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                db.Urunlers.Add(model.Urunler);
                model.Urunler.EklemeTarihi = System.DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }
        [Route("UrunDetay/{name}/{ID}")]
        public ActionResult Detail(int ID,string name)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                UrunVM urun = new UrunVM()
                {
                    Urunler = db.Urunlers.Include(x => x.Markalar).Include(x => x.Kategoriler).Include(x=>x.UrunOzelliks).Where(x => x.ID == ID).SingleOrDefault(),
                    Resim = db.Resimlers.Where(x => x.UrunID == ID).ToList(),
                    Ozellik=db.UrunOzelliks.Include(x=>x.UrunOzellikTip).Include(x=>x.OzellikDeger).Where(x=>x.UrunID==ID).ToList()
                };
                if (urun.Urunler == null)
                    return HttpNotFound();
                return View(urun);
            }
        }
        [Route("Urun/ResimEkle/{name}/{ID}")]
        public ActionResult ResimEkle(int ID,string name)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.Urunlers.Where(x => x.ID == ID).SingleOrDefault();
                if (model == null)
                    return HttpNotFound();
                return View(model);
            }
        }
        [HttpPost]
        [Route("Urun/ResimEkle/{name}/{ID}")]
        public ActionResult ResimEkle(int ID, HttpPostedFileBase txtFoto,string name)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.Urunlers.Where(x => x.ID == ID).SingleOrDefault();
                if (model == null)
                    return HttpNotFound();
                if (txtFoto != null)
                {
                    InfoVM bilgi = new InfoVM();
                    WebImage img = new WebImage(txtFoto.InputStream);
                    FileInfo info = new FileInfo(txtFoto.FileName);
                    string newFoto = Guid.NewGuid().ToString() + info.Extension;
                    img.Resize(600, 300);
                    img.Save("~/Upload/UrunFoto/" + newFoto);
                    Resimler r = new Resimler()
                    {
                        OrtaYol = "/Upload/UrunFoto/" + newFoto,
                        Varsayilan = true,
                        UrunID = model.ID
                    };
                    //bilgi.Message = "Fotoğraf ekleme başarılı";
                    db.Resimlers.Add(r);
                    db.SaveChanges();
                    //bilgi.Statu = true;
                    //bilgi.Url = "/Urunler/List";
                    //bilgi.LinkText = "Geri dönmek için tıklayınız.";
                   
                    return View(model);
                }
                else
                {
                    ViewBag.Hata = "Resim ekleme başarısız";
                    return View();
                }
            }
        }
        [Route("Urun/Edit/{name}/{ID}")]
        public ActionResult Update(int ID)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                UrunVM urun = new UrunVM()
                {
                    Kategori = db.Kategorilers.ToList(),
                    Marka = db.Markalars.ToList(),
                    Urunler = db.Urunlers.Where(x => x.ID == ID).SingleOrDefault()
                };
                if (urun.Urunler == null)
                    return HttpNotFound();
                return View(urun);
            }
        }
        [HttpPost]
        [Route("Edit/{name}/{ID}")]
        public ActionResult Update(int ID, UrunVM model)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var urun = db.Urunlers.Where(x => x.ID == ID).SingleOrDefault();
                if (urun == null)
                    return HttpNotFound();
                urun.Adi = model.Urunler.Adi;
                urun.Aciklama = model.Urunler.Aciklama;
                urun.KategoriID = model.Urunler.KategoriID;
                urun.MarkaID = model.Urunler.MarkaID;
                urun.AlisFiyat = model.Urunler.AlisFiyat;
                urun.SatisFiyat = model.Urunler.SatisFiyat;
                db.SaveChanges();
                return RedirectToAction("List");

            }
        }
        public ActionResult Delete(int ID)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.Urunlers.Where(x => x.ID == ID).SingleOrDefault();
                var img = db.Resimlers.Where(x => x.UrunID == ID).ToList().SingleOrDefault();
                if (model == null)
                    return HttpNotFound();
                if(img!=null)
                {
                    if (System.IO.File.Exists(Server.MapPath(img.OrtaYol)))
                        System.IO.File.Delete(Server.MapPath(img.OrtaYol));
                    for (int i = 0; i > img.OrtaYol.Count(); i++)
                    {
                        db.Resimlers.Remove(img);
                    }
                }
                db.Urunlers.Remove(model);
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }
    }
}