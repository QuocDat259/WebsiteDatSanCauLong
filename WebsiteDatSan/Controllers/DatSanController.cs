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

            // Truyền thông tin người đăng nhập, GioDat, San và UserName cho view
            ViewBag.UserId = userId;
            ViewBag.GioDat = gioDat;
            ViewBag.San = san.TenSan;
            ViewBag.UserName = userName;
            ViewBag.NgayDat = ngayDat;
            ViewBag.GiaTien = giaTien.HasValue ? giaTien.Value : 0m;
            ViewBag.id = id;
            return View(hoaDon);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DatSan([Bind(Include = "MaHoaDon,MaSan,MaGioDat,MahinhThuc,TongTien,NgayDat")] HoaDon hoadon, int id, string mahinhthuc)
        {
            if (ModelState.IsValid)
            {
                // Lấy ID của người dùng hiện tại
                string currentUserId = User.Identity.GetUserId();
                DateTime ngayDat = DateTime.Now;

                // Gán ID và các thông tin khác vào đối tượng HoaDon
                hoadon.id = currentUserId;
                hoadon.NgayDat = ngayDat;

                // Lấy thông tin GioDat từ cơ sở dữ liệu
                GioDat gioDat = db.GioDat.Find(id);
                if (gioDat != null)
                {
                    hoadon.MaSan = gioDat.idsan;
                    hoadon.MaGioDat = gioDat.MaGioDat;
                }

                // Chuyển đổi kiểu dữ liệu của MahinhThuc từ string sang int?
                int? mahinhthucValue = null;
                if (!string.IsNullOrEmpty(mahinhthuc))
                {
                    int parsedMahinhthuc;
                    if (int.TryParse(mahinhthuc, out parsedMahinhthuc))
                    {
                        mahinhthucValue = parsedMahinhthuc;
                    }
                }

                // Gán MahinhThuc vào HoaDon
                hoadon.MahinhThuc = mahinhthucValue;

                // Lưu HoaDon vào cơ sở dữ liệu
                db.HoaDon.Add(hoadon);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            // Nếu dữ liệu không hợp lệ, truyền các thông tin cần thiết cho view
            ViewBag.HinhThuc = new SelectList(db.HinhThucThanhToan, "MaHinhThuc", "TenHinhThuc", hoadon.HinhThucThanhToan);

            return View(hoadon);
        }



        public ActionResult Payment()
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "test";
            string returnUrl = "https://localhost:44334/DatSan/ConfirmPaymentClient";
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = "1000";
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
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

        public ActionResult ConfirmPaymentClient()
        {
            return View();
        }
    }
}
