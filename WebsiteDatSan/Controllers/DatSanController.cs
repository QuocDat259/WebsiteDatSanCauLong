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

            // Lấy UserName của người đăng nhập
            var userManager = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new ApplicationDbContext()));
            var user = userManager.FindById(userId);
            string userName = user.UserName;

            // Lấy ngày đặt hiện tại
            DateTime ngayDat = DateTime.Now;

            // Lấy giá tiền từ bảng San
            // Lấy giá tiền từ bảng San
            decimal? giaTien = san.GiaTien;

            // Truyền thông tin người đăng nhập, GioDat, San và UserName cho view
            ViewBag.UserId = userId;
            ViewBag.GioDat = gioDat;
            ViewBag.San = san;
            ViewBag.UserName = userName;
            ViewBag.NgayDat = ngayDat;
            ViewBag.GiaTien = giaTien.HasValue ? giaTien.Value : 0m;

            return View(gioDat);
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
