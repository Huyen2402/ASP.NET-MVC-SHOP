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
using System.Net.Mail;
using System.Net;
using Microsoft.Ajax.Utilities;

namespace WebsiteBanHang.Controllers
{
    public class DatHangController : Controller
    {
        Entities db = new Entities();
        // GET: DatHang
        [HttpPost]
        public ActionResult DatHang(SessionDiaChi sessionDiaChi)
        {
            DatHang dh = Session["DatHang"] as DatHang;
            Session["DiaChi"] = sessionDiaChi;
            SessionDiaChi ss = Session["DiaChi"] as SessionDiaChi;
            List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];

          
            
            if (Session["idKH"] != null)
            {
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
                    ddh.MaTinhTrangGiaoHang = 1;
                    ChiTietGiamGia ctgg = db.ChiTietGiamGias.Single(n => n.MaCTGiamGia == dh.MaCTGiamGia);
                    ctgg.DaSuDung = true;
                    if (dh.id == 1)
                    {
                        ddh.MaDDH = DateTime.Now.Ticks.ToString();
                        ddh.HinhThucThanhToan = "COD";
                        ddh.DaThanhToan = false;
                       ddh.MaShop = dh.MaShop;
                        ddh.UuDai = 0;
                        ddh.MaKH = iduser;
                        ddh.MaTinh = ss.MaTinh;
                        ddh.MaHuyen = ss.MaHuyen;
                        ddh.MaXa = ss.MaXa;
                        ddh.DiaChi= ss.DiaChi;
                        db.DonDatHangs.Add(ddh);



                        for (var i = 0; i < listGioHang.Count(); i++)
                        {
                            if (listGioHang[i].MaShop == dh.MaShop)
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
                            Session["DatHang"] = null;
                            return RedirectToAction("XemGioHang", "GioHang");
                        }
                           
                    }
                    if (dh.id == 2)
                    {

                        if (listGioHang != null)
                        {
                            decimal total = 0;
                            for (int i = 0; i < listGioHang.Count(); i++)
                            {
                                decimal price = listGioHang[i].ThanhTien;
                                total = total + price;
                            }
                            string tongiten = dh.ThanhTien.ToString();


                            //ddh.HinhThucThanhToan = "MoMo";
                            //ddh.DaThanhToan = true;

                            //request params need to request to MoMo system
                            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                            string partnerCode = "MOMONJLR20220909";
                            string accessKey = "hWtILE8L8yb1vzVz";
                            string serectkey = "ktQfGrAtjnGWlAUo6Ea2SP7fVhBbzrhK";
                            string orderInfo = "Huyền Cosmetic";
                            string returnUrl = "http://localhost:62979/DatHang/ReturnUrl?MaShop="+dh.MaShop + "&MaCTGiamGia=" + dh.MaCTGiamGia;
                            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

                            string amount = dh.ThanhTien.ToString();
                            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
                            string requestId = DateTime.Now.Ticks.ToString();
                            string extraData = "";
                            Session["orderid"] = orderid;

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
                            //return RedirectToAction("XemGioHang", "GioHang");

                            return Redirect(jmessage.GetValue("payUrl").ToString());

                        }
                    }


                    if (dh.id == 3)
                    {

                        if (listGioHang != null)
                        {
                            decimal total = 0;
                            for (int i = 0; i < listGioHang.Count(); i++)
                            {
                                decimal price = listGioHang[i].ThanhTien;
                                total = (dh.ThanhTien) * 100;
                            }
                            string tongiten = total.ToString();
                            //ddh.HinhThucThanhToan = "VNPay";
                            //ddh.DaThanhToan = true;

                            string url = ConfigurationManager.AppSettings["Url"];
                            //string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
                            string returnUrl = "http://localhost:62979/DatHang/PaymentConfirm?MaShop="+ dh.MaShop+ "&MaCTGiamGia=" + dh.MaCTGiamGia;
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

                         

                            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

                            return Redirect(paymentUrl);
                        }
                    }





                }
            }
            else
            {

                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            
            
            return RedirectToAction("XemGioHang","GioHang");
            
        }

        public ActionResult ReturnUrl(int MaShop, int MaCTGiamGia)
        {
            SessionDiaChi ss = Session["DiaChi"] as SessionDiaChi;
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = Server.UrlDecode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string serectkey = "ktQfGrAtjnGWlAUo6Ea2SP7fVhBbzrhK";
            //string serectKey = ConfigurationManager.AppSettings["serectKey"].ToString();
            string signature = crypto.signSHA256(param, serectkey);
            if (signature != Request["signature"].ToString())
            {
                ViewBag.message = "Thông tin request không hợp lệ";
            }
            else if (!Request.QueryString["errorCode"].Equals("0"))
            {
                ViewBag.message = "Thanh toán thất bại";
            }
            else
            {
                List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];
               
                int iduser = (int)Session["idKH"];
                if (listGioHang == null)
                {
                    return null;
                }
                else
                {
                    ChiTietGiamGia ctgg = db.ChiTietGiamGias.Single(n=>n.MaCTGiamGia == MaCTGiamGia);
                    ctgg.DaSuDung = true;
                    DonDatHang ddh = new DonDatHang();
                    string ngay = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    ddh.NgayDat = DateTime.Parse(ngay);
                    ddh.MaShop = MaShop;
                    ddh.MaTinhTrangGiaoHang = 1;
                    
                    if (listGioHang != null)
                    {
                        decimal total = 0;
                        for (int i = 0; i < listGioHang.Count(); i++)
                        {
                            decimal price = listGioHang[i].ThanhTien;
                            total = total + price;
                        }
                        string tongiten = total.ToString();


                        ddh.HinhThucThanhToan = "MoMo";
                        ddh.DaThanhToan = true;
                        ddh.MaShop = MaShop;
                        ddh.MaDDH = Session["orderid"].ToString();
                        ddh.UuDai = 0;
                        ddh.MaKH = iduser;
                        ddh.MaTinh = ss.MaTinh;
                        ddh.MaHuyen = ss.MaHuyen;
                        ddh.MaXa = ss.MaXa;
                        ddh.DiaChi= ss.DiaChi;
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
                        Session["orderid"] = null;
                        Session["DatHang"] = null;
                        ViewBag.message = "Đặt hàng và thanh toán thành công";

                        return RedirectToAction("XemGioHang", "GioHang");
                    }

                }   
                
            }
            return View();
        }
        public ActionResult PaymentConfirm(int MaShop, int MaCTGiamGia)
        {
            SessionDiaChi ss = Session["DiaChi"] as SessionDiaChi;
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        List<GioHang> listGioHang = (List<GioHang>)Session["GioHang"];
                        
                        int iduser = (int)Session["idKH"];
                        if (listGioHang == null)
                        {
                            return null;
                        }
                        else
                        {
                            ChiTietGiamGia ctgg = db.ChiTietGiamGias.Single(n => n.MaCTGiamGia == MaCTGiamGia);
                            ctgg.DaSuDung = true;
                            DonDatHang ddh = new DonDatHang();
                            string ngay = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                            ddh.NgayDat = DateTime.Parse(ngay);
                            ddh.MaTinhTrangGiaoHang = 1;
                            if (listGioHang != null)
                            {
                                decimal total = 0;
                                for (int i = 0; i < listGioHang.Count(); i++)
                                {
                                    decimal price = listGioHang[i].ThanhTien;
                                    total = total + price;
                                }
                                string tongiten = total.ToString();


                                ddh.HinhThucThanhToan = "VNPay";
                                ddh.DaThanhToan = true;
                                ddh.MaShop = MaShop;

                                ddh.MaDDH = DateTime.Now.Ticks.ToString();
                                ddh.UuDai = 0;
                                ddh.MaKH = iduser;
                                ddh.MaTinh = ss.MaTinh;
                                ddh.MaHuyen = ss.MaHuyen;
                                ddh.MaXa = ss.MaXa;
                                ddh.DiaChi= ss.DiaChi;
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
                                Session["DatHang"] = null;
                                ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                                return RedirectToAction("XemGioHang", "GioHang");
                            }
                        }

                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }

        public ActionResult MuaHang(int id, decimal ThanhTien, int MaShop, int MaCTGiamGia)
        {
            Session["DatHang"] = new DatHang(id, ThanhTien, MaShop, MaCTGiamGia);
            Shop shop = db.Shops.SingleOrDefault(n => n.MaShop == MaShop);
            List<GioHang> listSPGioHang = Session["GioHang"] as List<GioHang>;
            List<GioHang> listSP = new List<GioHang>();
            for (int i=0; i <= listSPGioHang.Count - 1; i++)
            {
                if(listSPGioHang[i].MaShop == MaShop)
                {
                    listSP.Add(listSPGioHang[i]);
                }
            }
            ViewBag.listSP = listSP;
            return View(shop);
        }

        public ActionResult DiaChiPartial()
        {
            ViewBag.MaTinh = new SelectList(db.Tinhs, "MaTinh", "TenTinh");
            int iduser = (int)Session["idKH"];
             ViewBag.lastAddress = db.DiaChis.OrderByDescending(n=>n.ID).First();
            List<DiaChi> listDC = db.DiaChis.Where(n => n.MaThanhVien == iduser).ToList();
            return PartialView(listDC);
        }

        public JsonResult CustomAddress(int ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            //DiaChi dc = db.DiaChis.SingleOrDefault(n => n.ID == ID);
            //var x = dc.Select(n => new
            //{
            //    tenTinh = n.Tinh.TenTinh,
            //    tenHuyen = n.Huyen.TenHuyen,
            //    tenXa = n.Xa.TenXa,
            //    tenThanhVien = n.ThanhVien.HoTen,
            //    diaChi = n.DiaChi1

            //});
            var list = db.DiaChis.Where(n => n.ID == ID).Select(n => new
            {
                ID = n.ID,
                tenTinh = n.Tinh.TenTinh,
                idTinh = n.Tinh.MaTinh,
                tenHuyen = n.Huyen.TenHuyen,
                idHuyen = n.Huyen.MaHuyen,
                tenXa = n.Xa.TenXa,
                idXa = n.Xa.MaXa,
                tenThanhVien = n.ThanhVien.HoTen,
                sdt = n.Phone,
                diaChi = n.Address

            }).Take(1).ToList();

            if (list != null)
            {
                return Json( new { status = true, data = list },  JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null);
            }

        }

        [HttpPost]
        public JsonResult AddAddress(DiaChi dc)
        {

            ThanhVien u = Session["TaiKhoan"] as ThanhVien;
            ThanhVien tv = db.ThanhViens.Single(n => n.MaThanhVien == u.MaThanhVien);
            if (tv != null)
                {
                //    DiaChi newdc = new DiaChi();
                //    newdc.MaThanhVien = u.MaThanhVien;
                //    newdc.SDT = dc.SDT;

                //    newdc.Address = dc.Address;
                //    newdc.MaHuyen = dc.MaHuyen;
                //    newdc.MaTinh = dc.MaTinh;
                //    newdc.MaXa = dc.MaXa;
                //    db.DiaChis.Add(newdc);
                //    db.SaveChanges();
       
                dc.MaThanhVien = u.MaThanhVien;
                db.DiaChis.Add(dc);
                db.SaveChanges();
                    
                return Json(new {data = dc, status = true}, JsonRequestBehavior.AllowGet);
            }
            return Json(null);

        }



    }
}