using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Ochi_ss
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // アプリケーションのスタートアップで実行するコードです
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_End(Object sender, EventArgs e)
        {
            Session.Abandon();
        }

        void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("", "{pid}", "~/UserList.aspx");
        }
    }

}