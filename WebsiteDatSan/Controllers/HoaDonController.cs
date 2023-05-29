using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebsiteDatSan.Models;

namespace WebsiteDatSan.Controllers
{
    public class HoaDonController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: HoaDon
        public ActionResult HoaDon()
        {
            // Lấy thông tin người đăng nhập hiện tại
            var userId = User.Identity.GetUserId();
            // Lấy UserName của người đăng nhập
            var userManager = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new ApplicationDbContext()));
            var user = userManager.FindById(userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            // Lấy danh sách hóa đơn cho người dùng hiện tại
            IEnumerable<HoaDon> hoaDonList = db.HoaDon.Where(h => h.id == userId).ToList();

            //List< HoaDon> hoaDons = db.HoaDon.Where(h => h.id == userId).ToList();
            //if (hoaDons == null)
            //{
            //    // Xử lý trường hợp không tìm thấy hóa đơn
            //    return RedirectToAction("NotFound", "Error"); // Chuyển hướng đến trang thông báo lỗi
            //}
            //foreach (var hd in hoaDons)
            //{

            //}
            // Lấy ngày đặt hiện tại
            /*DateTime ngayDat = DateTime.Now;
            // Lấy thông tin từ bảng San
            San san = db.San.FirstOrDefault(s => s.MaSan == hoaDon.MaSan);

            // Lấy thông tin từ bảng GioDat
            GioDat gioDat = db.GioDat.FirstOrDefault(g => g.MaGioDat == hoaDon.MaGioDat);

            // Lấy thông tin từ bảng HinhThucThanhToan
            HinhThucThanhToan hinhThucThanhToan = db.HinhThucThanhToan.FirstOrDefault(h => h.MaHinhTHuc == hoaDon.MahinhThuc);

            decimal Tien = (decimal)hoaDon.TongTien;
            string[] part = Tien.ToString().Split(',');
            string tien = part[0]; // "N0" để định dạng thành số nguyên không có phần thập phân và dấu phân cách

            // Gán thông tin vào ViewBag
            ViewBag.TenSan = san?.TenSan;
            ViewBag.DiaChi = san?.DiaChi;
            ViewBag.TongTien = ti;
            ViewBag.GioBatDau = gioDat?.GioBatDau;
            ViewBag.GioKetThuc = gioDat?.GioKetThuc;
            ViewBag.TenHinhThuc = hinhThucThanhToan?.TenHinhThuc;
            hoaDon.NgayDat = ngayDat;*/

            return View("HoaDon", hoaDonList);
        }
    }
}
