using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteDatSan.Models;

namespace WebsiteDatSan.Areas.ChuSan.Controllers
{
    public class QLGioDatsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ChuSan/QLGioDats
        public ActionResult Index()
        {
            var gioDat = db.GioDat.Include(g => g.San);
            return View(gioDat.ToList());
        }

        // GET: ChuSan/QLGioDats/Details/5
        public ActionResult Details(int? id)
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
            return View(gioDat);
        }

        // GET: ChuSan/QLGioDats/Create
        public ActionResult Create()
        {
            ViewBag.idsan = new SelectList(db.San, "MaSan", "TenSan");
            return View();
        }

        // POST: ChuSan/QLGioDats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaGioDat,GioBatDau,GioKetThuc,idsan,TrangThai")] GioDat gioDat)
        {
            if (ModelState.IsValid)
            {
                db.GioDat.Add(gioDat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idsan = new SelectList(db.San, "MaSan", "TenSan", gioDat.idsan);
            return View(gioDat);
        }

        // GET: ChuSan/QLGioDats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy ID của người dùng đang đăng nhập
            string userId = User.Identity.GetUserId();

            // Kiểm tra xem người dùng có quyền là "ChuSan" hay không
            bool isChuSan = User.IsInRole("ChuSan");

            if (!isChuSan)
            {
                // Người dùng không có quyền "ChuSan", chuyển hướng đến trang không được phép
                return RedirectToAction("Unauthorized", "Error");
            }

            GioDat gioDat = db.GioDat.Find(id);
            if (gioDat == null)
            {
                return HttpNotFound();
            }
            ViewBag.idsan = new SelectList(db.San, "MaSan", "TenSan", gioDat.idsan);
            return View(gioDat);
        }

        // POST: ChuSan/QLGioDats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaGioDat,GioBatDau,GioKetThuc,idsan,TrangThai")] GioDat gioDat)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem người dùng có quyền là "ChuSan" hay không
                bool isChuSan = User.IsInRole("ChuSan");

                if (!isChuSan)
                {
                    // Người dùng không có quyền "ChuSan", chuyển hướng đến trang không được phép
                    return RedirectToAction("Unauthorized", "Error");
                }

                db.Entry(gioDat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idsan = new SelectList(db.San, "MaSan", "TenSan", gioDat.idsan);
            return View(gioDat);
        }

        // GET: ChuSan/QLGioDats/Delete/5
        public ActionResult Delete(int? id)
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
            return View(gioDat);
        }

        // POST: ChuSan/QLGioDats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GioDat gioDat = db.GioDat.Find(id);
            db.GioDat.Remove(gioDat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
