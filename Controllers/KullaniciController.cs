using eTrade.Models;
using eTrade.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;
using System.Web.Helpers;
using System.IO;

namespace eTrade.Controllers
{
    [RoutePrefix("Kullanici")]
    public class KullaniciController : Controller
    {
        // GET: Kullanici
        [Route("~/Kayıtol")]
        public ActionResult RegisterOrLogin()
        {
            return View();
        }
        [HttpPost]
        [Route("~/Kayıtol")]
        public ActionResult RegisterOrLogin(UserVM model)
        {
            var loginedUser = Membership.GetUser(model.KullaniciAdi);
            if (loginedUser != null)
            {
                bool isTrue = Membership.ValidateUser(model.KullaniciAdi, model.Sifre);
                if (isTrue)
                {
                    FormsAuthentication.RedirectFromLoginPage(model.KullaniciAdi, false);
                    return RedirectToAction("Index", "Home");
                }
                return View(model);
            }
            else
            {
                Membership.CreateUser(model.KullaniciAdi, model.Sifre, model.EMail);
                return RedirectToAction("RegisterOrLogin");
            }
        }
        [Route("~/Profil/{id}")]
        public ActionResult Profil(string id)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var k = Membership.GetUser(id);
                //UserVM user = new UserVM()
                //{
                //    Kullanicilar = Membership.GetUser(id),
                //    Musteriler = db.Musteris.Where(x => x.ID == (Guid)k.ProviderUserKey).SingleOrDefault()
                //};
                var resim = db.Musteris.Include(x => x.Resimler).Where(x => x.ID == (Guid)k.ProviderUserKey).SingleOrDefault();
                if (resim != null)
                {
                    string fotoYol = resim.Resimler.KucukYol;
                    ViewBag.Foto = fotoYol;
                }
                if (User.Identity.Name == k.UserName)
                    return View(k);
                return HttpNotFound();
            }
        }
        public ActionResult SignOut()
        {
            if (HttpContext.Session["cart"] != null)
                HttpContext.Session.Remove("cart");
            FormsAuthentication.SignOut();
            return RedirectToAction("RegisterOrLogin");
        }


        [Route("Edit/{id}")]
        public ActionResult Edit(string id)
        {

            using (eTicaretEntities db = new eTicaretEntities())
            {
                var k = Membership.GetUser(id);
                UserVM user = new UserVM()
                {
                    Kullanicilar = Membership.GetUser(id),
                    Musteriler = db.Musteris.Include(x => x.Resimler).SingleOrDefault(x => x.KullaniciAdi == id),
                };
                if (User.Identity.Name == k.UserName)
                {
                    var resim = db.Musteris.Include(x => x.Resimler).Where(x => x.ID == (Guid)k.ProviderUserKey).SingleOrDefault();
                    if (resim != null)
                    {
                        string fotoYol = resim.Resimler.KucukYol;
                        ViewBag.Foto = fotoYol;
                    }
                    return View(user);
                }
                if (user.Musteriler == null)
                    return View();
                return HttpNotFound();
            }
        }
        [HttpPost]
        [Route("Edit/{id}")]
        public ActionResult Edit(string id, UserVM model, string txtAdres, HttpPostedFileBase txtFoto)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                Resimler r = new Resimler();
                string newFoto = "";
                var loginedUser = Membership.GetUser(id);
                var musteris = db.Musteris.Where(x => x.ID == (Guid)loginedUser.ProviderUserKey).SingleOrDefault();
                if (musteris == null)
                {

                    if (txtFoto != null)
                    {
                        WebImage img = new WebImage(txtFoto.InputStream);
                        FileInfo info = new FileInfo(txtFoto.FileName);
                        newFoto = Guid.NewGuid().ToString() + info.Extension;
                        img.Resize(100, 150);
                        img.Save("~/Upload/KullaniciPP/" + newFoto);
                        r.KucukYol = "/Upload/KullaniciPP/" + newFoto;
                        db.Resimlers.Add(r);
                    }
                    var resim = db.Resimlers.Where(x => x.KucukYol == "/Upload/KullaniciPP/" + newFoto).SingleOrDefault();
                    model.Musteriler.KullaniciAdi = id;
                    model.Musteriler.EMail = loginedUser.Email;
                    model.Musteriler.ID = (Guid)loginedUser.ProviderUserKey;
                    model.Musteriler.ResimID = r.ID;
                    db.Musteris.Add(model.Musteriler);
                    MusteriAdresler adres = new MusteriAdresler()
                    {
                        Ad = model.Musteriler.Ad,
                        Adres = txtAdres,
                        MusteriID = model.Musteriler.ID
                    };
                    db.MusteriAdreslers.Add(adres);
                }
                else
                {
                    var musteri = db.Musteris.Where(x => x.ID == (Guid)loginedUser.ProviderUserKey).SingleOrDefault();
                    var rsm = db.Resimlers.Where(x => x.ID == musteri.ResimID).SingleOrDefault();

                    if (txtFoto != null)
                    {
                        if (rsm != null)
                        {
                            if (System.IO.File.Exists(Server.MapPath(rsm.KucukYol)))
                                System.IO.File.Delete(Server.MapPath(rsm.KucukYol));
                        }
                        WebImage img = new WebImage(txtFoto.InputStream);
                        FileInfo info = new FileInfo(txtFoto.FileName);
                        newFoto = Guid.NewGuid().ToString() + info.Extension;
                        img.Resize(100, 150);
                        img.Save("~/Upload/KullaniciPP/" + newFoto);
                        r.KucukYol = "/Upload/KullaniciPP/" + newFoto;
                        db.Resimlers.Add(r);
                        musteri.ResimID = r.ID;
                    }
                    musteri.Ad = model.Musteriler.Ad;
                    musteri.Soyad = model.Musteriler.Soyad;
                    musteri.Telefon = model.Musteriler.Telefon;
                    MusteriAdresler adres = new MusteriAdresler();
                    adres.Adres = txtAdres;
                }

                db.SaveChanges();
                return RedirectToAction("Profil", new { id = id });
            }
        }
        [Route("~/Siparislerim/{id}")]
        public ActionResult Siparislerim(string id)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var loginedUser = Membership.GetUser(id);
                if (loginedUser.UserName == User.Identity.Name)
                {
                    var siparis = db.Satislars.Include(x => x.Musteri).Include(x => x.SiparisDurum).Include(x => x.Kargolar).Where(x => x.MusteriID == (Guid)loginedUser.ProviderUserKey && x.SiparisDurumID == 1).ToList();
                    if (siparis == null)
                        return View();
                    var resim = db.Musteris.Include(x => x.Resimler).Where(x => x.ID == (Guid)loginedUser.ProviderUserKey).SingleOrDefault();
                    if (resim != null)
                    {
                        string fotoYol = resim.Resimler.KucukYol;
                        ViewBag.Foto = fotoYol;
                    }
                    return View(siparis);
                }
                return HttpNotFound();
            }
        }

        public ActionResult SiparisDetays(int ID)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.SatisDetays.Include(x => x.Urunler).Where(x => x.SatisID == ID).ToList();
                if (model == null)
                    return HttpNotFound();
                return View(model);
            }
        }

        [Route("~/GecmisSiparis/{id}")]
        public ActionResult GecmisSiparis(string id)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var loginedUser = Membership.GetUser(id);
                if (loginedUser.UserName == User.Identity.Name)
                {
                    var siparis = db.Satislars.Include(x => x.Musteri).Include(x => x.SiparisDurum).Include(x => x.Kargolar).Where(x => x.MusteriID == (Guid)loginedUser.ProviderUserKey && x.SiparisDurumID == 3).ToList();
                    if (siparis == null)
                        return View();
                    var resim = db.Musteris.Include(x => x.Resimler).Where(x => x.ID == (Guid)loginedUser.ProviderUserKey).SingleOrDefault();
                    if (resim != null)
                    {
                        string fotoYol = resim.Resimler.KucukYol;
                        ViewBag.Foto = fotoYol;
                    }
                    return View(siparis);
                }
                return HttpNotFound();
            }
        }
        public ActionResult GecmisSiparisDetay(int ID)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.SatisDetays.Include(x => x.Satislar).Include(x => x.Urunler).Where(x => x.Satislar.SiparisDurumID == 3).ToList();
                if (model == null)
                    return HttpNotFound();
                return View(model);
            }
        }
    }
}