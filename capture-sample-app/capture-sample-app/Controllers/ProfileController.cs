using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Newtonsoft.Json.Linq;
using System.Web.SessionState;

using capturesampleapp;
using capturesampleapp.conf;

namespace Controllers
{

	[HandleError]
	public class ProfileController : Controller
	{
		public ActionResult ProfileWithTokenRefresh ()
		{
			// TODO: implement token refresh. 
			ViewData ["Message"] = "this is ProfileWithTokenRefresh";
			return View ();
		}
		
		public ActionResult EditProfile ()
		{
			JObject user_data = OauthService.getUserFromCapture (OauthService.getCurrentSessionAccessToken());
			if(user_data != null) {
				if(user_data["stat"] != null && (String)user_data["stat"] == "ok") {
						ViewData ["Message"] = "Hello, " + (String)user_data["result"]["displayName"] + ".";
					    ViewData ["ProfileEditor"] =  OauthService.getProfileEditor(OauthService.getCurrentSessionAccessToken());
				}
			}	
			return View ();
		}
		
		public ActionResult EditProfileFinished() {
			ViewData["Message"] = OauthService.captureProfileSync();
			return View ("~/Views/Home/Index.aspx");
		}
	}
}

