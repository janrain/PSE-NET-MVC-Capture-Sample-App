using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace capturesampleapp
{
	
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes (RouteCollection routes)
		{
			routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");

			routes.MapRoute (
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" }
            );

		}

		protected void Application_Start ()
		{
			RegisterRoutes (RouteTable.Routes);

			//Application["This_App_Server_Url"] = "http://127.0.0.1:8080/";
			//Application["Janrain_Capture_Client_Id"] = "";
			//Application["Janrain_Capture_Client_Secret"] = "";
			//Application["Janrain_Capture_Domain"] = "";
			//Application["Janrain_Capture_UI_Server"] = "";
			//Application["Janrain_SSO_Server"] = "";
			//Application["Janrain_Edit_Profile_With_Fancybox"] = "true";
			//Application["Janrain_Do_Capture_Profile_Sync"] = "true";

		}
	}
}
