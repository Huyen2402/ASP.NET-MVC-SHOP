using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using System.Web.Management;

[assembly: OwinStartup(typeof(WebsiteBanHang.Startup))]

namespace WebsiteBanHang
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            
            
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }

       
    }
}
