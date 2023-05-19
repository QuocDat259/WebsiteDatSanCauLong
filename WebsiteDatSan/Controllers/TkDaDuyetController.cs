using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatSan.Models;
using System.Net;

namespace WebsiteDatSan.Controllers
{
    public class TkDaDuyetController : Controller
    {
        // GET: TkDaDuyet
        public ActionResult Index()
        {
            var userManager = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new ApplicationDbContext()));
            var userList = userManager.Users.Where(u => (bool)u.IsApproved).ToList();
            return View(userList);
        }
        public ActionResult ShowSanChuSan(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new ApplicationDbContext())
            {
                var sans = db.Sans.Where(p => p.IdUser == id /*&& p.status == true*/).ToList();
                ViewBag.userId = id;
                return View(sans);
            }
        }
    }
}