using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteDatSan.Models;
using MoMo;
using System.Web.UI.WebControls;
using System.Web.Razor.Parser.SyntaxTree;
using System.Data.Entity.Migrations;

namespace WebsiteDatSan.Controllers
{
    public class DatSanController : Controller
    {
        WebCauLongss db = new WebCauLongss();

        // GET: DatSan
        public ActionResult DatSan(int? id)
        {
            HoaDon hoaDon = new HoaDon();
            var magiodat = db.GioDat.FirstOrDefault(c => c.idsan == id);
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

            // Lấy giá tiền từ bảng San
            decimal? giaTien = san.GiaTien;

            // Định dạng giá tiền thành chuỗi VND
            string formattedGiaTien = giaTien.HasValue ? giaTien.Value.ToString("N0") + " VND" : "0 VND";

            // Lấy danh sách hình thức thanh toán từ cơ sở dữ liệu
            var hinhThucList = db.HinhThucThanhToan.ToList();

            // Truyền thông tin người đăng nhập, GioDat, San và UserName cho view
            ViewBag.UserId = userId;
            ViewBag.GioDat = gioDat;
            ViewBag.San = san.TenSan;
            ViewBag.UserName = userName;
            ViewBag.NgayDat = ngayDat;
            ViewBag.GiaTien = giaTien.HasValue ? giaTien.Value : 0m;
            ViewBag.id = id;
            ViewBag.GioBatDau = gioDat.GioBatDau;
            ViewBag.GioKetThuc = gioDat.GioKetThuc;
            ViewBag.GiaTien = formattedGiaTien;
            ViewBag.HinhThuc = new SelectList(hinhThucList, "MaHinhThuc", "TenHinhThuc");
            return View(hoaDon);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DatSan([Bind(Include = "MaHoaDon,MaSan,MaGioDat,MahinhThuc,TongTien,NgayDat")] HoaDon hoadon, int id, int? mahinhthuc)
        {
            if (ModelState.IsValid)
            {
                // Lấy ID của người dùng hiện tại
                string currentUserId = User.Identity.GetUserId();
                DateTime ngayDat = DateTime.Now;
                hoadon.MaHoaDon = Guid.NewGuid().ToString();
                // Gán ID và các thông tin khác vào đối tượng HoaDon
                hoadon.id = currentUserId;
                hoadon.NgayDat = ngayDat;
                hoadon.MahinhThuc = mahinhthuc.Value;
                hoadon.TrangThai = false;
                // Lấy thông tin GioDat từ cơ sở dữ liệu
                GioDat gioDat = db.GioDat.Find(id);
                if (gioDat != null)
                {
                    hoadon.MaSan = gioDat.idsan;
                    hoadon.MaGioDat = gioDat.MaGioDat;

                }

                // Lấy thông tin từ bảng San
                var san = db.San.FirstOrDefault(s => s.MaSan == hoadon.MaSan);

                // Lấy giá tiền từ bảng San
                decimal? giaTien = san.GiaTien;

                // Gán giá tiền cho đối tượng HoaDon
                hoadon.TongTien = giaTien.HasValue ? giaTien.Value : 0m;

                // Định dạng giá tiền thành chuỗi VND
                string formattedGiaTien = giaTien.HasValue ? giaTien.Value.ToString("N0") + " VND" : "0 VND";

                // Lấy danh sách hình thức thanh toán
                var hinhThucList = db.HinhThucThanhToan.ToList();

                // Gán danh sách hình thức thanh toán vào ViewBag
                ViewBag.HinhThuc = new SelectList(hinhThucList, "MaHinhThuc", "TenHinhThuc", hoadon.MahinhThuc);

                // Lưu HoaDon vào cơ sở dữ liệu
                db.HoaDon.Add(hoadon);
                db.SaveChanges();
                if (mahinhthuc == 2)
                {
                    return RedirectToAction("Payment", "DatSan", new { order = hoadon.MaHoaDon });
                }
                return RedirectToAction("Index", "Home");
            }

            // Nếu dữ liệu không hợp lệ, truyền các thông tin cần thiết cho view
            ViewBag.HinhThuc = new SelectList(db.HinhThucThanhToan, "MaHinhThuc", "TenHinhThuc", hoadon.MahinhThuc);

            return View(hoadon);
        }


        public ActionResult Payment(string order)
        {
            //request params need to request to MoMo system
            //request params need to request to MoMo system
            var tienthanhtoan = db.HoaDon.FirstOrDefault(n => n.MaHoaDon == order);
            var userManager = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new ApplicationDbContext()));
            var user = userManager.FindById(tienthanhtoan.id);
            string userName = user.FullName;
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = userName + " thanh toán với số tiền " + String.Format("{0:0,0}", tienthanhtoan.TongTien.ToString()) + "VNĐ"; 
            string returnUrl = "https://localhost:44334/DatSan/ConfirmPaymentClient";
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test
            string amount = "80000";
            string orderid = order.ToString();
            string requestId = order.ToString();
            string extraData = "";
            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());

        }

        //public ActionResult ConfirmPaymentClient()
        //{
        //    return View();
        //}
        //public ActionResult Payment(string order)
        //{
        //    //request params need to request to MoMo system
        //    var tienthanhtoan = db.HoaDon.FirstOrDefault(n => n.id == order);
        //    var userManager = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new ApplicationDbContext()));
        //    var user = userManager.FindById(order);
        //    string userName = user.FullName;
        //    string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
        //    string partnerCode = "MOMOOJOI20210710";
        //    string accessKey = "iPXneGmrJH0G8FOP";
        //    string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
        //    string orderInfo = userName + " thanh toán với số tiền " + String.Format("{0:0,0}", tienthanhtoan.TongTien.ToString()) + "VNĐ"; ;
        //    string returnUrl = "https://localhost:44334/DatSan/ConfirmPaymentClient";
        //    string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test
        //    string amount = "1000";
        //    string orderid = order;
        //    string requestId = order;
        //    string extraData = "";

        //    //Before sign HMAC SHA256 signature
        //    string rawHash = "partnerCode=" +
        //        partnerCode + "&accessKey=" +
        //        accessKey + "&requestId=" +
        //        requestId + "&amount=" +
        //        amount + "&orderId=" +
        //        orderid + "&orderInfo=" +
        //        orderInfo + "&returnUrl=" +
        //        returnUrl + "&notifyUrl=" +
        //        notifyurl + "&extraData=" +
        //        extraData;

        //    MoMoSecurity crypto = new MoMoSecurity();
        //    //sign signature SHA256
        //    string signature = crypto.signSHA256(rawHash, serectkey);

        //    //build body json request
        //    JObject message = new JObject
        //    {
        //        { "partnerCode", partnerCode },
        //        { "accessKey", accessKey },
        //        { "requestId", requestId },
        //        { "amount", amount },
        //        { "orderId", orderid },
        //        { "orderInfo", orderInfo },
        //        { "returnUrl", returnUrl },
        //        { "notifyUrl", notifyurl },
        //        { "extraData", extraData },
        //        { "requestType", "captureMoMoWallet" },
        //        { "signature", signature }

        //    };

        //    string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

        //    JObject jmessage = JObject.Parse(responseFromMomo);

        //    return Redirect(jmessage.GetValue("payUrl").ToString());
        //}
        public ActionResult ConfirmPaymentClient(Result result)
        {
            //lấy kết quả Momo trả về và hiển thị thông báo cho người dùng (có thể lấy dữ liệu ở đây cập nhật xuống db)
            string rMessage = result.message;
            string rOrderId = result.orderId;
            string rErrorCode = result.errorCode; // = 0: thanh toán thành công
            if (rErrorCode == "0")
            {
                var hd = db.HoaDon.FirstOrDefault(u => u.MaHoaDon == rOrderId);
                hd.TrangThai = true;
                db.HoaDon.AddOrUpdate(hd);
                db.SaveChanges();
                ViewBag.message = "CẢM ƠN QUÝ KHÁCH ĐÃ TIN TƯỞNG VÀ ỦNG HỘ SHOP";
            }
            return View();
        }
    }
}
