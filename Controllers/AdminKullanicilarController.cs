using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace eTrade.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("Admin")]
    public class AdminKullanicilarController : Controller
    {
        // GET: AdminKullanicilar
        [Route("Kullanici/Listesi")]
        public ActionResult List()
        {
            MembershipUserCollection users= Membership.GetAllUsers();
            return View(users);
        }
        [Route("Kullanici/Yetkilendir/{id}")]
        public ActionResult YetkiVer(string id)
        {
            ViewBag.Roles = Roles.GetAllRoles().ToList();
            return View(Membership.GetUser(id));
        }
        [Route("Kullanici/Yetkiver/{id}")]
        [HttpPost]
        public ActionResult YetkiVer(string id, string roleName)
        {
            Roles.AddUserToRole(id, roleName);
            return RedirectToAction("List");
        }
        public string RolGoster(string id)
        {
            List<string> roller = Roles.GetRolesForUser(id).ToList();
            string hata = "Bu kullanıcıya ait bir rol bulunmamaktadır.";
            if (roller.Count == 0)
                return hata;
            else
            {
                string roles = "";
                foreach (string item in roller)
                {
                    roles += item + "-";
                }
                roles = roles.Remove(roles.Length - 1, 1);
                return roles;
            }
        }
    }
}