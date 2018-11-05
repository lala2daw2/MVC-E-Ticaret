using eTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Helpers;
using System.IO;

namespace eTrade.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("Admin")]
    public class MarkaController : Controller
    {
        // GET: Marka
        [Route("Marka/Listesi")]
        public ActionResult List()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                return View(db.Markalars.Include(x => x.Resimler).Include(x=>x.Urunlers).ToList());
            }
        }
        [Route("Marka/YeniMarka")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("Marka/YeniMarka")]
        public ActionResult Create(Markalar model, HttpPostedFileBase txtFoto)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                if (txtFoto != null)
                {
                    WebImage img = new WebImage(txtFoto.InputStream);
                    FileInfo info = new FileInfo(txtFoto.FileName);
                    string newFoto = Guid.NewGuid().ToString() + info.Extension;
                    img.Resize(150, 300);
                    img.Save("~/Upload/MarkaFoto/" + newFoto);
                    Resimler r = new Resimler()
                    {
                        KucukYol = "/Upload/MarkaFoto/" + newFoto,
                    };
                    db.Resimlers.Add(r);
                    model.ResimID = r.ID;
                }
                db.Markalars.Add(model);
                db.SaveChanges();
                return RedirectToAction("List");

            }
        }
        [Route("Marka/Edit/{name}/{ID}")]
        public ActionResult Update(int ID,string name)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.Markalars.Include(x => x.Resimler).Where(x => x.ID == ID).SingleOrDefault();
                if (model == null)
                    return HttpNotFound();
                return View(model);
            }
        }
        [HttpPost]
        [Route("Marka/Edit/{name}/{ID}")]
        public ActionResult Update(int ID, Markalar model, HttpPostedFileBase txtFoto,string name)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var result = db.Markalars.Include(x => x.Resimler).Where(x => x.ID == ID).SingleOrDefault();
                var resim = db.Resimlers.Where(x => x.ID == result.ResimID).SingleOrDefault();
                if (result == null)
                    return HttpNotFound();
                if (txtFoto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(resim.KucukYol)))
                        System.IO.File.Delete(Server.MapPath(resim.KucukYol));
                    WebImage img = new WebImage(txtFoto.InputStream);
                    FileInfo info = new FileInfo(txtFoto.FileName);
                    string newFoto = Guid.NewGuid().ToString() + info.Extension;
                    img.Resize(150, 300);
                    img.Save("~/Upload/MarkaFoto/" + newFoto);
                    resim.KucukYol = "/Upload/MarkaFoto/" + newFoto;
                }
                result.MarkaAd = model.MarkaAd;
                result.Aciklama = model.Aciklama;
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }
        public ActionResult Delete(int ID)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.Markalars.Where(x => x.ID == ID).SingleOrDefault();
                var resim = db.Resimlers.Where(x => x.ID == model.ResimID).SingleOrDefault();
                if (model == null)
                    return HttpNotFound();
                else
                {
                    if (System.IO.File.Exists(Server.MapPath(resim.KucukYol)))
                        System.IO.File.Delete(Server.MapPath(resim.KucukYol));
                    db.Resimlers.Remove(resim);
                    db.Markalars.Remove(model);
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
            }
        }
    }
}