using eTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace eTrade.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminHomeController : Controller
    {
        // GET: AdminHome
        [Route("Home")]
        public ActionResult Index()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                ViewBag.UrunSayisi = db.Urunlers.Count();
                ViewBag.SiparisSayisi = db.Satislars.Where(x => x.SiparisDurumID == 1).Count();
                ViewBag.MusteriSayisi = db.Musteris.Count();
                int count = Membership.GetAllUsers().Count;
                ViewBag.KullaniSayisi = count;
                ViewBag.KargoSiparis = db.Satislars.Where(x => x.SiparisDurumID == 2).Count();
                return View();

            }
        }
    }
}