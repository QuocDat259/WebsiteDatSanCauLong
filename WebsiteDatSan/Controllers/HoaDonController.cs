using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteDatSan.Models;

namespace WebsiteDatSan.Controllers
{
    public class HoaDonController : Controller
    {
        WebCauLongss db = new WebCauLongss();
        // GET: HoaDon
        public ActionResult HoaDon(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HoaDon hoaDon = db.HoaDon.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            // Lấy thông tin người đăng nhập hiện tại
            var userId = User.Identity.GetUserId();
            // Lấy UserName của người đăng nhập
            var userManager = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new ApplicationDbContext()));
            var user = userManager.FindById(userId);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string userName = user.UserName;

            // Lấy ngày đặt hiện tại
            DateTime ngayDat = DateTime.Now;
            // Lấy thông tin từ bảng San
            San san = db.San.FirstOrDefault(s => s.MaSan == hoaDon.MaSan);

            // Lấy thông tin từ bảng GioDat
            GioDat gioDat = db.GioDat.FirstOrDefault(g => g.MaGioDat == hoaDon.MaGioDat);

            // Lấy thông tin từ bảng HinhThucThanhToan
            HinhThucThanhToan hinhThucThanhToan = db.HinhThucThanhToan.FirstOrDefault(h => h.MaHinhTHuc == hoaDon.MahinhThuc);

            // Gán thông tin vào ViewBag
            ViewBag.TenSan = san?.TenSan;
            ViewBag.DiaChi = san?.DiaChi;
            ViewBag.TongTien = new SelectList(db.San, "MaSan", "GiaTien",hoaDon.TongTien);
            ViewBag.GioBatDau = gioDat?.GioBatDau;
            ViewBag.GioKetThuc = gioDat?.GioKetThuc;
            ViewBag.TenHinhThuc = hinhThucThanhToan?.TenHinhThuc;
            hoaDon.NgayDat = ngayDat;

            return View("HoaDon", hoaDon);
        }

    }
}