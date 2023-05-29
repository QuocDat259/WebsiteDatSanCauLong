using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatSan.Models;
using System.Net;
using System.Data.Entity.Core;

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
                var sans = db.San.Where(p => p.IdUser == id /*&& p.status == true*/).ToList();
                ViewBag.userId = id;
                return View(sans);
            }
        }


        public ActionResult ShowGioDats(int id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var gioDats = db.GioDat.Where(g => g.idsan == id).ToList();
                    if (gioDats.Count == 0)
                    {
                        string message = "Không có khung giờ nào được tìm thấy.";
                        return Content(message);
                    }

                    return View(gioDats);
                }
            }
            catch (EntityCommandExecutionException ex)
            {
                // Log or handle the exception as needed
                var innerException = ex.InnerException;
                throw;
            }
        }
        //public ActionResult Details(int id)
        //{
        //    using (var db = new ApplicationDbContext())
        //    {
        //        var hoaDon = db.San.Find(id);
        //        if (hoaDon == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(hoaDon);
        //    }
        //}

    }
}