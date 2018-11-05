using eTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using eTrade.ViewModel;
using eTrade.App_Start;
using PagedList;
using PagedList.Mvc;

namespace eTrade.Controllers
{
    [RoutePrefix("Anasayfa")]
    public class HomeController : Controller
    {
        // GET: Home
        [Route()]
        public ActionResult Index()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                return View(db.Urunlers.Include(x => x.Resimlers).OrderByDescending(x=>x.ID).ToList());
            }
        }
        public PartialViewResult KategoriPartial()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.Kategorilers.Include(x=>x.Urunlers).ToList();
                return PartialView(model);
            }
        }
        [Route("~/Kategoris/{catName}/{ID}")]
        public ActionResult KategoriUrunList(int ID,string catName)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.Urunlers.Include(x=>x.Resimlers).Include(x=>x.Kategoriler).Where(x => x.KategoriID == ID).OrderByDescending(x=>x.ID).ToList();
             
                return View(model);

            }
        }
        public PartialViewResult MarkaWidget()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                var model = db.Markalars.Include(x => x.Resimler).ToList();
                return PartialView(model);
            }
        }
        [Route("~/Urun/TumUrunler")]
        public ActionResult AllProducts(int? sayfaNo)
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                int _sayfaNo = sayfaNo??1;
                return View(db.Urunlers.Include(x => x.Resimlers).OrderByDescending(x=>x.ID).ToPagedList<Urunler>(_sayfaNo, 8));
            }
        }
        [Route("~/Hakkimizda")]
        public ActionResult About()
        {
            return View();
        }
        [Route("~/İletisim")]
        public ActionResult Contact()
        {
            return View();
        }
        [Route("~/UrunDetay/{name}/{ID}")]
        public ActionResult ProductDetail(int ID,string name)
        {
            using (eTicaretEntities db= new eTicaretEntities())
            {
                UrunVM urun = new UrunVM()
                {
                    Urunler = db.Urunlers.Include(x=>x.Resimlers).SingleOrDefault(x => x.ID == ID),
                    Ozellik = db.UrunOzelliks.Include(x=>x.OzellikDeger).Include(x=>x.UrunOzellikTip).Where(x => x.UrunID == ID).ToList(),
                    Resim=db.Resimlers.Where(x=>x.UrunID==ID).ToList()
                };
                return View(urun);
            }
        }
        public PartialViewResult MiniSepet()
        {
            using (eTicaretEntities db = new eTicaretEntities())
            {
                if (HttpContext.Session["cart"] != null)
                {
                    return PartialView(HttpContext.Session["cart"]);

                }
                else
                    return PartialView();
            }
        }
    }
}