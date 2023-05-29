using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatSan.Models;

namespace WebsiteDatSan.Areas.Admin.Controllers
{
    public class QLHoaDonController : Controller
    {
        // GET: Admin/QLHoaDon
        private WebCauLongss db = new WebCauLongss();
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: HoaDon
        public ActionResult HoaDon()
        {
            // Lấy danh sách tất cả hóa đơn
            List<HoaDon> hoaDonList = db.HoaDon.OrderBy(u => u.id).ToList();
            List<AspNetUsers> aspNetUsers = new List<AspNetUsers>();
            string userId = "";
            AdminVM adminVM = new AdminVM();
            foreach (HoaDon hoaDon in hoaDonList)
            {
                if (hoaDon.id != userId)
                {
                    AspNetUsers user = new AspNetUsers();
                    user = _db.Users.FirstOrDefault(u => u.Id == hoaDon.id);
                    aspNetUsers.Add(user);
                    userId = hoaDon.id;
                }
            }
            adminVM.AspNetUsers = aspNetUsers;
            adminVM.HoaDons = hoaDonList;
            ViewBag.UserName = User.Identity.GetUserName();
            return View("HoaDon", adminVM);
        }
    }
}