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
    public class SanssController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ChuSan/Sanss
        public ActionResult Index()
        {
            var sans = db.Sans.Include(s => s.LoaiSan);
            return View(sans.ToList());
        }

        // GET: ChuSan/Sanss/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            San san = db.Sans.Find(id);
            if (san == null)
            {
                return HttpNotFound();
            }
            return View(san);
        }

        // GET: ChuSan/Sanss/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiSan = new SelectList(db.LoaiSans, "MaLoaiSan", "TenLoaiSan");
            return View();
        }

        // POST: ChuSan/Sanss/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSan,MaLoaiSan,TenSan,DIaChi,GIaTien,TrangThai,HinhAnh")] San san)
        {
            if (ModelState.IsValid)
            {
                db.Sans.Add(san);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiSan = new SelectList(db.LoaiSans, "MaLoaiSan", "TenLoaiSan", san.MaLoaiSan);
            return View(san);
        }

        // GET: ChuSan/Sanss/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            San san = db.Sans.Find(id);
            if (san == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSan = new SelectList(db.LoaiSans, "MaLoaiSan", "TenLoaiSan", san.MaLoaiSan);
            return View(san);
        }

        // POST: ChuSan/Sanss/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSan,MaLoaiSan,TenSan,DIaChi,GIaTien,TrangThai,HinhAnh")] San san)
        {
            if (ModelState.IsValid)
            {
                db.Entry(san).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiSan = new SelectList(db.LoaiSans, "MaLoaiSan", "TenLoaiSan", san.MaLoaiSan);
            return View(san);
        }

        // GET: ChuSan/Sanss/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            San san = db.Sans.Find(id);
            if (san == null)
            {
                return HttpNotFound();
            }
            return View(san);
        }

        // POST: ChuSan/Sanss/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            San san = db.Sans.Find(id);
            db.Sans.Remove(san);
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
