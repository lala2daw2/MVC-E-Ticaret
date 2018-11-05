using eTrade.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Data.Entity;
using eTrade.ViewModel;

namespace eTrade.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("Admin")]
    public class KategoriController : Controller
    {
        // GET: Kategori
        [Route("Kategori/Listesi")]
        public ActionResult List()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                return View(db.Kategorilers.Include(x => x.Resimler).Include(x => x.Urunlers).ToList());
            }
        }
        [HttpGet]
        [Route("Kategori/YeniKategori")]
        public ActionResult Create()
        {
            return View();
        }
        [Route("Kategori/YeniKategori")]
        public ActionResult Create(Kategoriler model, HttpPostedFileBase txtFoto)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                WebImage img = new WebImage(txtFoto.InputStream);
                FileInfo info = new FileInfo(txtFoto.FileName);
                string newFoto = Guid.NewGuid().ToString() + info.Extension;
                img.Resize(150, 300);
                img.Save("~/Upload/KategoriFoto/" + newFoto);
                Resimler r = new Resimler()
                {
                    KucukYol = "/Upload/KategoriFoto/" + newFoto
                };
                db.Resimlers.Add(r);
                db.Kategorilers.Add(model);
                model.ResimID = r.ID;
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }
        [HttpGet]
        [Route("Kategori/Edit/{name}/{ID}")]
        public ActionResult Update(int ID, int resimID, string name)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                KategoriResimVM model = new KategoriResimVM()
                {
                    Kategori = db.Kategorilers.Where(x => x.ID == ID).SingleOrDefault(),
                    Resim = db.Resimlers.Where(x => x.ID == resimID).SingleOrDefault()
                };
                if (model.Resim == null || model.Kategori == null)
                    return HttpNotFound();
                return View(model);
            }
        }
        [Route("Kategori/Edit/{name}/{ID}")]
        [HttpPost]
        public ActionResult Update(int ID, KategoriResimVM model, HttpPostedFileBase txtFoto, string name)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var kategori = db.Kategorilers.Where(x => x.ID == ID).SingleOrDefault();
                var resim = db.Resimlers.Where(x => x.ID == kategori.ResimID).SingleOrDefault();
                if (txtFoto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(resim.KucukYol)))
                        System.IO.File.Delete(Server.MapPath(resim.KucukYol));
                    WebImage img = new WebImage(txtFoto.InputStream);
                    FileInfo info = new FileInfo(txtFoto.FileName);
                    string newFoto = Guid.NewGuid().ToString() + info.Extension;
                    img.Resize(150, 300);
                    img.Save("~/Upload/KategoriFoto/" + newFoto);
                    resim.KucukYol = "/Upload/KategoriFoto/" + newFoto;
                }
                kategori.KategoriAd = model.Kategori.KategoriAd;
                kategori.Aciklama = model.Kategori.Aciklama;
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }
        [HttpPost]
        public ActionResult Delete(int ID)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.Kategorilers.Where(x => x.ID == ID).SingleOrDefault();
                //var resim = db.Resimlers.Where(x => x.ID == model.Resimler.ID).SingleOrDefault();
                if (model == null )
                    return HttpNotFound();
                else
                {
                    db.Kategorilers.Remove(model);
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
            }
        }
    }
}