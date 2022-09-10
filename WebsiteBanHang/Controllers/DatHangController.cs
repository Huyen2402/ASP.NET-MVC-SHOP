using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using MoMo;
using DemoVNPay.Others;
using System.Configuration;


namespace WebsiteBanHang.Controllers
{
    public class DatHangController : Controller
    {
        // GET: DatHang
        [HttpGet]
        public ActionResult DatHang(int id)
        {
            QuanLyBanHangEntities db = new QuanLyBanHangEntities();
            List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];
           // decimal total = 0;
           //for(int i = 0; i < listGioHang.Count(); i++)
           // {
           //     decimal price = listGioHang[i].ThanhTien ;
           //     total += price;
           // }

            int iduser = (int)Session["idKH"];
            if (listGioHang == null)
            {
                return null;
            }
            else
            {
                DonDatHang ddh = new DonDatHang();
                string ngay = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                ddh.NgayDat = DateTime.Parse(ngay);
                ddh.MaTinhTrangGiaoHang = 5;
                if(id == 1)
                {
                    ddh.MaDDH = DateTime.Now.Ticks.ToString();
                    ddh.HinhThucThanhToan = "COD";
                    ddh.DaThanhToan = false;

                    ddh.UuDai = 0;
                    ddh.MaKH = iduser;
                    ddh.MaTinh = (int)Session["MaTinh"];
                    ddh.MaHuyen = (int)Session["MaHuyen"];
                    ddh.MaXa = (int)Session["MaXa"];
                    db.DonDatHangs.Add(ddh);



                    for (var i = 0; i < listGioHang.Count(); i++)
                    {
                        ChiTietDonDatHang ct = new ChiTietDonDatHang();

                        ct.MaDDH = ddh.MaDDH;

                        ct.MaSP = listGioHang[i].MaSP;
                        ct.TenSP = listGioHang[i].TenSP;
                        ct.SoLuong = listGioHang[i].SoLuong;
                        ct.DonGia = listGioHang[i].Dongia;
                        db.ChiTietDonDatHangs.Add(ct);

                        if (ct != null)
                        {
                            SanPham spsl = db.SanPhams.SingleOrDefault(n => n.MaSP == ct.MaSP);
                            spsl.SoLuongTon--;
                            spsl.SoLanMua++;
                        }



                    }

                    db.SaveChanges();
                    Session["GioHang"] = null;
                }
                if(id == 2)
                {

                    if(listGioHang != null)
                    {
                        decimal total=0;
                        for(int i = 0; i < listGioHang.Count(); i++)
                        {
                            decimal price = listGioHang[i].ThanhTien;
                            total = total+ price;
                        }
                        string tongiten = total.ToString();
                       
                    
                    ddh.HinhThucThanhToan = "MoMo";
                    ddh.DaThanhToan = true;

                    //request params need to request to MoMo system
                    string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                    string partnerCode = "MOMONJLR20220909";
                    string accessKey = "hWtILE8L8yb1vzVz";
                    string serectkey = "ktQfGrAtjnGWlAUo6Ea2SP7fVhBbzrhK";
                    string orderInfo = "Huyền Cosmetic";
                    string returnUrl = "http://localhost:62979/GioHang/XemGioHang";
                    string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

                    string amount = tongiten;
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
                        ddh.MaDDH = orderid;
                        ddh.UuDai = 0;
                        ddh.MaKH = iduser;
                        ddh.MaTinh = (int)Session["MaTinh"];
                        ddh.MaHuyen = (int)Session["MaHuyen"];
                        ddh.MaXa = (int)Session["MaXa"];
                        db.DonDatHangs.Add(ddh);
                        for (var i = 0; i < listGioHang.Count(); i++)
                        {
                            ChiTietDonDatHang ct = new ChiTietDonDatHang();

                            ct.MaDDH = ddh.MaDDH;

                            ct.MaSP = listGioHang[i].MaSP;
                            ct.TenSP = listGioHang[i].TenSP;
                            ct.SoLuong = listGioHang[i].SoLuong;
                            ct.DonGia = listGioHang[i].Dongia;
                            db.ChiTietDonDatHangs.Add(ct);

                            if (ct != null)
                            {
                                SanPham spsl = db.SanPhams.SingleOrDefault(n => n.MaSP == ct.MaSP);
                                spsl.SoLuongTon--;
                                spsl.SoLanMua++;
                            }



                        }
                        db.SaveChanges();
                        Session["GioHang"] = null;

                        string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

                    JObject jmessage = JObject.Parse(responseFromMomo);
                    //return RedirectToAction("XemGioHang", "GioHang");

                    return Redirect(jmessage.GetValue("payUrl").ToString());
                    }
                }


                if(id == 3)
                {

                    if (listGioHang != null)
                    {
                        decimal total = 0;
                        for (int i = 0; i < listGioHang.Count(); i++)
                        {
                            decimal price = listGioHang[i].ThanhTien;
                            total = (total + price)*100;
                        }
                        string tongiten = total.ToString();
                        ddh.HinhThucThanhToan = "VNPay";
                        ddh.DaThanhToan = true;

                        string url = ConfigurationManager.AppSettings["Url"];
                        string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
                        string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
                        string hashSecret = ConfigurationManager.AppSettings["HashSecret"];



                        PayLib pay = new PayLib();

                        pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
                        pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
                        pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
                        pay.AddRequestData("vnp_Amount", tongiten); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
                        pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
                        pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
                        pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
                        pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
                        pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
                        pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
                        pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
                        pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
                        pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

                        ddh.MaDDH = DateTime.Now.Ticks.ToString();
                        ddh.UuDai = 0;
                        ddh.MaKH = iduser;
                        ddh.MaTinh = (int)Session["MaTinh"];
                        ddh.MaHuyen = (int)Session["MaHuyen"];
                        ddh.MaXa = (int)Session["MaXa"];
                        db.DonDatHangs.Add(ddh);
                        for (var i = 0; i < listGioHang.Count(); i++)
                        {
                            ChiTietDonDatHang ct = new ChiTietDonDatHang();

                            ct.MaDDH = ddh.MaDDH;

                            ct.MaSP = listGioHang[i].MaSP;
                            ct.TenSP = listGioHang[i].TenSP;
                            ct.SoLuong = listGioHang[i].SoLuong;
                            ct.DonGia = listGioHang[i].Dongia;
                            db.ChiTietDonDatHangs.Add(ct);

                            if (ct != null)
                            {
                                SanPham spsl = db.SanPhams.SingleOrDefault(n => n.MaSP == ct.MaSP);
                                spsl.SoLuongTon--;
                                spsl.SoLanMua++;
                            }



                        }

                        db.SaveChanges();
                        Session["GioHang"] = null;

                        string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

                        return Redirect(paymentUrl);
                    }
                }
                




            }
            
            return RedirectToAction("XemGioHang","GioHang");
        }
    }
}