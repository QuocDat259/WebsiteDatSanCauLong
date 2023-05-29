using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatSan.Models;
using System.Net;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace WebsiteDatSan.Areas.Admin.Controllers
{
    public class QLChuSanController : Controller
    {
        private ApplicationDbContext _context;
        public QLChuSanController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Admin/QLChuSan
        public ActionResult Index()
        {
            List<AspNetUsers> ListUser = new List<AspNetUsers>();
            var role = _context.AspNetRoles.FirstOrDefault(u => u.Name == "ChuSan");
            foreach (var user in ListUser)
            {

                if (user.AspNetRoles.Where(e => e.Id == role.Id).FirstOrDefault() != null)
                {
                    ListUser.Add(user);
                }
            }

            // Return a fallback view or handle the case when "chủ sân" role is not found
            return View(ListUser);
        }

        public ActionResult Approve(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user =  _context.AspNetUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.IsApproved = true;
            _context.AspNetUsers.AddOrUpdate(user);

            // Lưu thay đổi vào cơ sở dữ liệu
             _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult message()
        {
            return View();
        }
    }
}