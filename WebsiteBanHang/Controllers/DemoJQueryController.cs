using Microsoft.ML;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WebsiteBanHang.Models;
using System.Net.Http.Headers;
using Microsoft.AspNet.SignalR.Hosting;

namespace WebsiteBanHang.Controllers
{
    // Create a Car class
    class DanhGia
    {
        public string cmt;  // Create a field
        public int MaCTDDH;
        public int start;

        // Create a class constructor for the Car class
        public DanhGia(string cmt, int MaCTDDH, int start)
        {
            this.start = start;
            this.cmt = cmt;
            this.MaCTDDH = MaCTDDH;
        }

        
    }

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


  
        public async Task<Object> Test(string path, int MaCTDDH, int start)
        {
            string apires = "";
            DanhGia dg = new DanhGia(path, MaCTDDH, start);
             kq = new KetQua();
            using(var cl = new HttpClient(_client))
            {
                using(var responde = await cl.GetAsync("https://localhost:7193/WeatherForecast/Test?data="+path)) 
                {
                     apires = await responde.Content.ReadAsStringAsync();

                   



                }
            }
            return Json(new { re = apires, danhgia = dg }, JsonRequestBehavior.AllowGet) ;
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
        public async Task<string> callAPIPushImgCloud(FormCollection f)
        {
            string apires = "";
            if (Request.Files.Count > 0)
            { 
                HttpPostedFileBase upload = Request.Files[0];
                HttpFileCollectionBase files = Request.Files;
                StringContent httpcontent = new StringContent(upload.FileName);
                //string path = JsonConvert.SerializeObject(httpcontent);
                //var content = new StringContent(path, Encoding.UTF8, "application/json");

                kq = new KetQua();

                using (var cl = new HttpClient(_client))
                {
                    using (var responde = await cl.PostAsync("http://localhost:3000/users/", httpcontent))
                    {
                        apires = await responde.Content.ReadAsStringAsync();



                    }
                }
            }
            else
            {
               
            }
           
            return apires;
        }
       public ActionResult aaa(FormCollection f)
        {
            if (Request.Files.Count > 0)
            {
            }
            else
            {
                return Json(new { mess = "hello" }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }
        public ActionResult b( )
        {
            var apiURL = "http://localhost:3000/users/";
            HttpPostedFileBase file = Request.Files[0];

            using (HttpClient client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] Bytes = target.ToArray(); 

                    file.InputStream.Read(Bytes, 0, Bytes.Length);
                    var fileContent = new ByteArrayContent(Bytes);
                    byte[] fileBytes = new byte[file.InputStream.Length + 1];
                    file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                     fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
                    content.Add(fileContent);
                    var result = client.PostAsync(apiURL, content).Result;
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        //return new
                        //{
                        //    code = result.StatusCode,
                        //    message = "Successful",
                        //    data = new
                        //    {
                        //        success = true,
                        //        filename = file.FileName
                        //    }
                        //};
                    }
                    else
                    {
                        //return new
                        //{
                        //    code = result.StatusCode,
                        //    message = "Error"
                        //};
                    }
                }
            }
            return View(file);
        }
        public async Task<System.IO.Stream> Upload(string actionUrl, string paramString, Stream paramFileStream, byte[] paramFileBytes)
        {
            //HttpContent stringContent = new StringContent(paramString);
            //HttpContent fileStreamContent = new StreamContent(paramFileStream);
            HttpContent bytesContent = new ByteArrayContent(paramFileBytes);
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                //formData.Add(stringContent, "param1", "param1");
                //formData.Add(fileStreamContent, "file1", "file1");
                formData.Add(bytesContent, "file2", "file2");
                var response = await client.PostAsync(actionUrl, formData);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return await response.Content.ReadAsStreamAsync();
            }
        }
        public async Task<string> UploadAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://hello.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes("D:/ASP.NET-MVC-SHOP/WebsiteBanHang/Content/images/chat.png"));
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "b.jpg" };
                content.Add(fileContent);

                HttpResponseMessage response = await client.PostAsync("http://localhost:3000/users/", content);
               return "f";
            }
            
        }

        public ActionResult Upload1()
        {
            

                return View();

            
        }
    }
}