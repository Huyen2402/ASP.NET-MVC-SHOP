using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using MoMo;

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
                    ddh.HinhThucThanhToan = "COD";
                    ddh.DaThanhToan = false;
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
                    ddh.HinhThucThanhToan = "VNPay";
                    ddh.DaThanhToan = true;
                }
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

                   if(ct != null)
                    {
                        SanPham spsl = db.SanPhams.SingleOrDefault(n => n.MaSP == ct.MaSP);
                        spsl.SoLuongTon--;
                        spsl.SoLanMua++;
                    }



                }

             db.SaveChanges();




            }
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang","GioHang");
        }
    }
}