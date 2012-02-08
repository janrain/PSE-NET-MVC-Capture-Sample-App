using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Script.Serialization;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web.SessionState;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using capturesampleapp;
using capturesampleapp.conf;

namespace Controllers
{
	[HandleError]
	public class OauthController : Controller
	{
		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult OauthRedirect (String code, String from_sso)
		{
			Console.WriteLine ("*** OauthRedirect");
			String redirect_uri = capturesampleapp.conf.JanrainConfiguration.ThisAppServerUrl + "oauth/OauthRedirect";
			
			// SSO
			if ((String)capturesampleapp.conf.JanrainConfiguration.JanrainCaptureSSOServer != "" && from_sso != null && from_sso != "") 
			{
				String my_uri = (String)capturesampleapp.conf.JanrainConfiguration.ThisAppServerUrl;
				redirect_uri = redirect_uri + "?from_sso=1&origin=" + Server.UrlEncode (my_uri);
			}
			
			Console.WriteLine("code: " + code);
			Console.WriteLine("redirect_uri: " + redirect_uri);
						
			JObject json_data = OauthService.NewAccessToken (code, redirect_uri);
			
			// access token stored into session context.
			Session ["capture_session"] = OauthService.updateCaptureSession(json_data);
			
			// bool password_recover = false; 
			// password recovery.
			if (json_data["transaction_state"]["capture"]["password_recover"] != null) {
				Console.WriteLine("json_data[\"transaction_state\"][\"capture\"][\"password_recover\"]  " + (bool)json_data["transaction_state"]["capture"]["password_recover"] );

				if((bool)json_data["transaction_state"]["capture"]["password_recover"] == true) {
					//password_recover = true;
					ViewData ["Redirect_To"] = "ChangePassword";
					return View ();
				}
			}
			
			bool needToRefresh = false; 
			bool canRefresh = true;
			
			// TODO: implement correct behavior for token expiration, in-progress development. 
			DateTime expires_in = OauthService.getCurrentSessionExpiresIn();
			if(DateTime.Now >= expires_in) {
				needToRefresh = true; 
				json_data = OauthService.RefreshAccessToken (OauthService.getCurrentSessionRefreshToken());
				// access token stored into session context.
				Session ["capture_session"] = OauthService.updateCaptureSession(json_data);
			}
			// $user_entity = get_entity($capture_session['access_token']);
    		// if (isset($user_entity['code']) && $user_entity['code'] == '414')
      		// $need_to_refresh = true;
			
			// TODO: If necessary, refresh the access token and try to fetch the entity again.
			if (needToRefresh == true) {
				Console.WriteLine ("*** Access token expired: " + (String)json_data["access_token"]);
			}
			if (canRefresh == true) {
				Console.WriteLine ("*** Refreshing using refresh token: " + (String)json_data["access_token"]);
				
				JObject user_data = OauthService.getUserFromCapture ((String)json_data["access_token"]);  // get user record via Capture API. 
				
				Console.WriteLine ("*** user_data: " + user_data);
				// at this point, we have access to the user record.  
				
				ViewData ["Welcome_Message"] = "Welcome, " + (String)user_data["result"]["givenName"];
				Session ["capture_user_entity_displayName"] = (String)user_data["result"]["givenName"];
				Session ["Welcome_Message"] = "Welcome, " + (String)user_data["result"]["givenName"];
				ViewData ["Redirect_To"] = "Home"; 
	
				return View ();
			} else {
				Console.WriteLine ("*** Not refreshing access token<br>\n");
					
			}
			return View ();
	
		}
		
		public ActionResult Logout ()
		{
			Console.WriteLine ("*** Logout");
			ViewData ["Message"] = "Logging out...";
			Session.Clear(); 
			ViewData ["Redirect_To"] = "./Home";
			return View ();
		}
		
		public ActionResult ChangePassword ()
		{
			Console.WriteLine ("*** ChangePassword");
			
			Dictionary<string, string> capture_session = (Dictionary<string, string>) Session ["capture_session"]; 
			
			JObject user_data = OauthService.getUserFromCapture ((String)capture_session["access_token"]);
			if(user_data != null) {
				if(user_data["stat"] != null && (String)user_data["stat"] == "ok") {
						ViewData ["Message"] = "Hello, " + (String)user_data["result"]["displayName"] + ".";
					    ViewData ["PasswordReset"] = OauthService.getPasswordReset((String)capture_session["Token"]);
				}
			} else {
				ViewData ["Message"] = "You must be logged-in to access this page.";
			}
			return View ();
		}

		
	}
}

