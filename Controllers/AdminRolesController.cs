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
    public class AdminRolesController : Controller
    {
        // GET: AdminRoles
        [Route("Yetki/Listesi")]
        public ActionResult List()
        {
            List<string> roles = Roles.GetAllRoles().ToList();
            return View(roles);
        }
        [Route("Yetki/YeniYetki")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("Yetki/YeniYetki")]
        public ActionResult Create(string txtRole)
        {
            Roles.CreateRole(txtRole);
            return RedirectToAction("List");
        }
        public ActionResult Delete(string id)
        {
            Roles.DeleteRole(id);
            return RedirectToAction("List");
        }
    }
}