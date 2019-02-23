using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using Koowoo.Services;

namespace Koowoo.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Global : HttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Application_Start(object sender, EventArgs e)
        {

            //  DbInitService.Init();
            AutoMapperConfiguration.ConfigExt();
            AutofacExt.InitAutofac();
           // Log.Info("log", "Application_Start：" + DateTime.Now.ToString()); // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            RouteTable.Routes.Ignore(""); //Allow index.html to load     
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //取消注释默认返回 json
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            JobManager.Initialize(new TaskTime());
        }

        void Application_End(object sender, EventArgs e)
        {
           // Log.Info("log", "Application_End：" + DateTime.Now.ToString());
            //string url = Services.WebUtils.GetConfig().SiteUrl + "/KeepLive";
            //HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            //Stream receiveStream = myHttpWebResponse.GetResponseStream();//得到回写的字节流
        }
    }
}