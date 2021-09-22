using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(WebChat.Api.Startup))]

namespace WebChat.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Configuring CORS to allow requests from everywhere
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration { };
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}
