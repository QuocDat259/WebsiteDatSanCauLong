using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatSan.Models;

namespace WebsiteDatSan.Areas.Admin.Controllers
{
    public class QLTatCaHoaDonController : Controller
    {
        private WebCauLongss db = new WebCauLongss();

        // GET: Admin/QLTatCaHoaDon/HoaDon
        public ActionResult HoaDon()
        {
            IEnumerable<HoaDon> hoaDonList = db.HoaDon.ToList();
            ViewBag.UserName = User.Identity.GetUserName();
            return View(hoaDonList);
        }
    }
}
