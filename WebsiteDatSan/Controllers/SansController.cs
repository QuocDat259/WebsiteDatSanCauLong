using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteDatSan.Models;

namespace WebsiteDatSan.Controllers
{
    public class SansController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sans
        public ActionResult Index()
        {
            var san = db.San.Include(s => s.LoaiSan);
            return View(san.ToList());
        }

        // GET: Sans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            San san = db.San.Find(id);
            if (san == null)
            {
                return HttpNotFound();
            }
            return View(san);
        }

        // GET: Sans/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiSan = new SelectList(db.LoaiSan, "MaLoaiSan", "TenLoaiSan");
            return View();
        }

        // POST: Sans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSan,MaLoaiSan,TenSan,DiaChi,GiaTien,TrangThai,IdUser")] San san)
        {
            if (ModelState.IsValid)
            {
                db.San.Add(san);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiSan = new SelectList(db.LoaiSan, "MaLoaiSan", "TenLoaiSan", san.MaLoaiSan);
            return View(san);
        }

        // GET: Sans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            San san = db.San.Find(id);
            if (san == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSan = new SelectList(db.LoaiSan, "MaLoaiSan", "TenLoaiSan", san.MaLoaiSan);
            return View(san);
        }

        // POST: Sans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSan,MaLoaiSan,TenSan,DiaChi,GiaTien,TrangThai,IdUser")] San san)
        {
            if (ModelState.IsValid)
            {
                db.Entry(san).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiSan = new SelectList(db.LoaiSan, "MaLoaiSan", "TenLoaiSan", san.MaLoaiSan);
            return View(san);
        }

        // GET: Sans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            San san = db.San.Find(id);
            if (san == null)
            {
                return HttpNotFound();
            }
            return View(san);
        }

        // POST: Sans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            San san = db.San.Find(id);
            db.San.Remove(san);
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
