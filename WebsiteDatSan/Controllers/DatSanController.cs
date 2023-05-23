using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteDatSan.Models;

namespace WebsiteDatSan.Controllers
{
    public class DatSanController : Controller
    {
        WebCauLongss db = new WebCauLongss();

        // GET: DatSan
        public ActionResult DatSan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GioDat gioDat = db.GioDat.Find(id);
            if (gioDat == null)
            {
                return HttpNotFound();
            }

            // Lấy thông tin người đăng nhập hiện tại
            var userId = User.Identity.GetUserId();

            // Lấy thông tin từ bảng San
            var san = db.San.FirstOrDefault(s => s.MaSan == gioDat.San.MaSan);

            // Truyền thông tin người đăng nhập, GioDat và San cho view
            ViewBag.UserId = userId;
            ViewBag.GioDat = gioDat;
            ViewBag.San = san;

            return View(gioDat);
        }
    }
}
