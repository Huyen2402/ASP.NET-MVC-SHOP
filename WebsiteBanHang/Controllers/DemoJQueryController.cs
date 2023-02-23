using Microsoft.ML;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace WebsiteBanHang.Controllers
{
    
    public class DemoJQueryController : Controller
    {
       HttpClientHandler _client = new HttpClientHandler();
        KetQua kq = new KetQua();
        public DemoJQueryController()
        {
            _client.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        }
        // GET: DemoJQuery
        public ActionResult JQuery()
        {
            return View();
        }


  
        public async Task<string> Test(string path)
        {
            string apires = "";
             kq = new KetQua();
            using(var cl = new HttpClient(_client))
            {
                using(var responde = await cl.GetAsync("https://localhost:7193/WeatherForecast/Test?data="+path)) 
                {
                     apires = await responde.Content.ReadAsStringAsync();
                   
                
                   
                }
            }
            return apires ;
        }
        public ActionResult CallOTPView()
        {
            return View();
        }

        public async Task<string> CallOTP()
        {
            string apires = "";
            kq = new KetQua();
           
            using (var cl = new HttpClient(_client))
            {
                using (var responde = await cl.GetAsync("http://localhost:3000"))
                {
                    apires = await responde.Content.ReadAsStringAsync();



                }
            }
            return apires;
        }

        public async Task<string> checkVerify(string MaXacNhan, string Token)
        {
            string apires = "";
            kq = new KetQua();

            using (var cl = new HttpClient(_client))
            {
                using (var responde = await cl.GetAsync("http://localhost:3000/verify?MaXacNhan="+ MaXacNhan + "&Token="+Token))
                {
                    apires = await responde.Content.ReadAsStringAsync();



                }
            }
            return apires;
        }

    }
}