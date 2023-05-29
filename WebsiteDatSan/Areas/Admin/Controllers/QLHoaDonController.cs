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

        // GET: HoaDon
        public ActionResult HoaDon()
        {
            // Lấy danh sách tất cả hóa đơn
            IEnumerable<HoaDon> hoaDonList = db.HoaDon.ToList();
            ViewBag.UserName = User.Identity.GetUserName();
            return View("HoaDon", hoaDonList);
        }
    }
}