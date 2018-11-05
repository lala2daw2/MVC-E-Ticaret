using eTrade.App_Start;
using eTrade.Models;
using eTrade.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static eTrade.App_Start.Sepet;

namespace eTrade.Controllers
{
    public class SepetController : Controller
    {
        // GET: Sepet
        public void AddCart(int ID)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var loginedUser = Membership.GetUser(User.Identity.Name);
                if (loginedUser != null)
                {
                    SepetItem si = new SepetItem();
                    Sepet s = new Sepet();
                    var urun = db.Urunlers.FirstOrDefault(x => x.ID == ID);
                    si.Urun = urun;
                    si.Adet = 1;
                    si.Indirim = 0;
                    s.Add(si);
                }
            }

        }
        public void AdetDüsür(int ID)
        {
            if (HttpContext.Session["cart"] != null)
            {
                Sepet s = (Sepet)HttpContext.Session["cart"];
                if (s.Urunler.FirstOrDefault(x => x.Urun.ID == ID).Adet > 1)
                {
                    s.Urunler.FirstOrDefault(x => x.Urun.ID == ID).Adet--;
                }
                else
                {
                    SepetItem si = s.Urunler.FirstOrDefault(x => x.Urun.ID == ID);
                    s.Urunler.Remove(si);
                }
            }
        }
        [Route("Sepetim")]
        public PartialViewResult CartAllItems()
        {
            if (HttpContext.Session["cart"] != null)
                return PartialView(HttpContext.Session["cart"]);
            else
                return PartialView();
        }
        [Route("ÖdemeYap")]
        public ActionResult CompleteShop()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                if (HttpContext.Session["cart"] != null)
                {
                    List<SelectListItem> IcerikTurListe = (from k in db.Kargolars
                                                           select new SelectListItem
                                                           {
                                                               Text = k.SirketAdi,
                                                               Value = k.ID.ToString()
                                                           }).ToList();
                    ViewBag.Kargo = IcerikTurListe;
                    return View(HttpContext.Session["cart"]);
                }
                else
                {
                    return RedirectToAction("AllProducts", "Home");
                }
            }
        }
        [Route("ÖdemeYap")]
        [HttpPost]
        public ActionResult CompleteShop(int Kargo)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                if (HttpContext.Session["cart"] != null)
                {
                    Sepet s = (Sepet)HttpContext.Session["cart"];
                    var loginedUser = Membership.GetUser(User.Identity.Name);
                    SatisDetay detay = new SatisDetay();
                    Satislar satis = new Satislar()
                    {
                        KargoID = Kargo,
                        MusteriID = (Guid)loginedUser.ProviderUserKey,
                        SatisTarihi = System.DateTime.Now,
                        Sepettemi = true,
                        ToplamTutar = s.ToplamTutar,
                        SiparisDurumID = 1,
                    };
                    db.Satislars.Add(satis);
                    db.SaveChanges();
                    foreach (var item in s.Urunler)
                    {
                        SatisDetay dt = new SatisDetay()
                        {
                            Adet = item.Adet,
                            Fiyat = item.Tutar,
                            UrunID = item.Urun.ID,
                            Indirim = 1,
                            SatisID = satis.ID
                        };
                        List<SatisDetay> dety = new List<SatisDetay>();
                        dety.Add(dt);
                        db.SatisDetays.AddRange(dety);
                    }
                    db.SaveChanges();

                    HttpContext.Session.Remove("cart");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return HttpNotFound();
                }

            }
        }
    }
}