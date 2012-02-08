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
  
using capturesampleapp.conf;

namespace capturesampleapp
{
	public class OauthService
	{
		public static JObject NewAccessToken (String auth_code, String redirect_uri)
		{
			
			String command = "oauth/token";

			Dictionary<string, string> arg_array = new Dictionary<string, string> (); 
			if (!arg_array.ContainsKey ("code")) {
				arg_array.Add ("code", auth_code);
			}
			if (!arg_array.ContainsKey ("redirect_uri")) {
				arg_array.Add ("redirect_uri", redirect_uri);
			}
			if (!arg_array.ContainsKey ("grant_type")) {
				arg_array.Add ("grant_type", "authorization_code");
			}
			if (!arg_array.ContainsKey ("client_secret")) {
				arg_array.Add ("client_secret", capturesampleapp.conf.JanrainConfiguration.JanrainCaptureClientSecret);
			}
			if (!arg_array.ContainsKey ("client_id")) {
				arg_array.Add ("client_id", capturesampleapp.conf.JanrainConfiguration.JanrainCaptureClientID);
			}
			
			string json_data = Pipeline.ApiCall (command, arg_array, "json");
			Console.WriteLine ("************ NewAccessToken-json_data: " + json_data);
			return(JObject.Parse (json_data));	  
		}
		
		public static JObject RefreshAccessToken (String refresh_token)
		{
			String command = "oauth/token";

			Dictionary<string, string> arg_array = new Dictionary<string, string> (); 
			if (!arg_array.ContainsKey ("refresh_token")) {
				arg_array.Add ("refresh_token", refresh_token);
			}
			if (!arg_array.ContainsKey ("grant_type")) {
				arg_array.Add ("grant_type", "refresh_token");
			}
			if (!arg_array.ContainsKey ("client_secret")) {
				arg_array.Add ("client_secret", capturesampleapp.conf.JanrainConfiguration.JanrainCaptureClientSecret);
			}
			if (!arg_array.ContainsKey ("client_id")) {
				arg_array.Add ("client_id", capturesampleapp.conf.JanrainConfiguration.JanrainCaptureClientID);
			}
			
			
			string json_data = Pipeline.ApiCall (command, arg_array, "json");
			Console.WriteLine ("************ RefreshAccessToken-json_data: " + json_data);
			return(JObject.Parse (json_data));
		}
		
		// entity API call via authorization token.
		public static JObject getUserFromCapture (String token)
		{
			String command = "entity";
			
			Dictionary<string, string> arg_array = new Dictionary<string, string> (); 
			arg_array.Add ("authorization_token", token);
			/*
			if (!arg_array.ContainsKey ("client_secret")) {
				arg_array.Add ("client_secret", capturesampleapp.conf.JanrainConfiguration.JanrainCaptureClientSecret);
			}
			*/
			if (!arg_array.ContainsKey ("client_id")) {
				arg_array.Add ("client_id", capturesampleapp.conf.JanrainConfiguration.JanrainCaptureClientID);
			}
			
			string json_data = Pipeline.ApiCall (command, arg_array, "json");	
			Console.WriteLine ("************ string: " + json_data);
			return(JObject.Parse (json_data));
		}
		
		public static String getProfileEditor (String token)
		{
			
			Dictionary<string, string> arg_array = new Dictionary<string, string> (); 
			arg_array.Add ("token", token);
			
			/*
			if (!arg_array.ContainsKey ("client_secret")) {
				arg_array.Add ("client_secret", capturesampleapp.conf.JanrainConfiguration.JanrainCaptureClientSecret);
			}
			*/
			if (!arg_array.ContainsKey ("client_id")) {
				arg_array.Add ("client_id", capturesampleapp.conf.JanrainConfiguration.JanrainCaptureClientID);
			}
			
			if (!arg_array.ContainsKey ("callback")) {
				arg_array.Add ("callback", "CAPTURE.closeProfileEditor");
			}
			if (!arg_array.ContainsKey ("xd_receiver")) {
				arg_array.Add ("xd_receiver", capturesampleapp.conf.JanrainConfiguration.ThisAppServerUrl + "/Content/xdcomm.html");
			}  
			
			
			return "<iframe class='profile' frameborder='0' scrolling='yes' src='" + capturesampleapp.conf.JanrainConfiguration.JanrainCaptureDomain + "/oauth/profile?" + Pipeline.BuildDataString (arg_array) + "'></iframe>";
		}
		
		public static String getPasswordReset (String token)
		{
			
			Dictionary<string, string> arg_array = new Dictionary<string, string> (); 
			arg_array.Add ("token", token);
			
			if (!arg_array.ContainsKey ("client_id")) {
				arg_array.Add ("client_id", capturesampleapp.conf.JanrainConfiguration.JanrainCaptureClientID);
			}
			if (!arg_array.ContainsKey ("callback")) {
				arg_array.Add ("callback", "CAPTURE.closeChangePassword");
			}
			if (!arg_array.ContainsKey ("xd_receiver")) {
				arg_array.Add ("xd_receiver", capturesampleapp.conf.JanrainConfiguration.ThisAppServerUrl + "/Content/xdcomm.html");
			}
			
			return "<iframe class='profile' frameborder='0' scrolling='no' src='" + capturesampleapp.conf.JanrainConfiguration.JanrainCaptureDomain + "/oauth/profile_change_password?" + Pipeline.BuildDataString (arg_array) + "'></iframe>";
		}
		
		public static Dictionary<string, string> updateCaptureSession (JObject json_data)
		{
			try {
				String stat = (String)json_data ["stat"];
				if (stat != null && stat == "error") {
					Console.WriteLine ("*** updateCaptureSession: input has an error<br>\n");
					Console.WriteLine (json_data);	
					return null; 	
				} else {
					Dictionary<string, string> session_record = new Dictionary<string, string> ();
					session_record.Add ("expires_in", Convert.ToString(DateTime.Now.AddSeconds((double)json_data["expires_in"])));
					session_record.Add ("access_token", (string)json_data ["access_token"]);
					session_record.Add ("refresh_token", (string)json_data ["refresh_token"]);                  
					return session_record;
				}
				
			} catch (Exception e) {
				throw new System.Exception ("in updateCaptureSession: " + e.Message);
	 
			}
		}
		
		
		public static string getCurrentSessionAccessToken ()
		{
			try {
				HttpSessionState current_session = HttpContext.Current.Session;
				Dictionary<string, string> session_record = (Dictionary<string, string>) current_session ["capture_session"];
				if (session_record != null) {
					string session_token = session_record ["access_token"];	
					return session_token;
				} else {
					return null;  	
				}
				
			} catch (Exception e) {
				throw new System.Exception ("in getCurrentSessionAccessToken:" + e.Message);
			}
		}
		
		public static string getCurrentSessionRefreshToken ()
		{
			try {
				HttpSessionState current_session = HttpContext.Current.Session;
				Dictionary<string, string> session_record = (Dictionary<string, string>) current_session ["capture_session"];
				if (session_record != null) {
					string refresh_token = session_record ["refresh_token"];	
					return refresh_token;
				} else {
					return null;  	
				}
				
			} catch (Exception e) {
				throw new System.Exception ("in getCurrentSessionRefreshToken:" + e.Message);
			}
		}
		
		public static DateTime getCurrentSessionExpiresIn ()
		{
			try {
				HttpSessionState current_session = HttpContext.Current.Session;
				Dictionary<string, string> session_record = (Dictionary<string, string>) current_session ["capture_session"];
				if (session_record != null) {
					DateTime expires_in = Convert.ToDateTime (session_record ["expires_in"]);	
					return expires_in;
				} else {
					return DateTime.Now;  	
				}
				
			} catch (Exception e) {
				throw new System.Exception ("in getCurrentSessionExpiresIn:" + e.Message);
			}
		}
		
		public static string captureProfileSync() {
			try {
				string session_token = getCurrentSessionAccessToken();
				JObject json_data = getUserFromCapture(session_token);
				string message = "";
				if(json_data == null) {
					message = "Could not retrieve profile data.";
					Console.WriteLine(message);
					return message;
				}
				
				if(updateUser(json_data) == true) {
					message = "Profile successfully updated.";
					Console.WriteLine(message);
					return message; 
				} else {
					// just a sample - you would handle this according to your own practices.
					message = "Could not update profile data.";
					Console.WriteLine(message);
					return message;
				}

			} catch (Exception e) {
				throw new System.Exception ("in captureProfileSync: " + e.Message);
			}
			
		}
		
		// update_user is call you would implement, to transfer data from
		// the returned json data user object into the matching row in your database.
		public static bool updateUser(JObject json_data) {
			try {
				// for now, we simply return true.  
				return true; 
			} catch (Exception e) {
				throw new System.Exception ("in updateUser: " + e.Message);
			}
		}
		
		
//		public static string CaptureApiCall (String command, IDictionary<string, string> arg_array)
//		{
		//			var json = "{\"center\":{\"latitude\":\"49.266214\",\"longitude\":\"-122.998577\"},\"zoom\":\"12\"}";
		//			JavaScriptSerializer serializer = new JavaScriptSerializer ();
		//			var dictionary = serializer.Deserialize<IDictionary<string, object>>(json);
		//        	dynamic mapdata = dictionary.Expando();
		//			var center = mapdata.center.latitude;
		//			return center;
//			return null;
//		}
		
		static internal class Pipeline
		{
			/// <summary>
			/// Posts the api call and returns the raw string returned.
			/// </summary>
			/// <param name="methodName"></param>
			/// <param name="partialQuery"></param>
			/// <param name="responseDataFormat"></param>
			/// <returns></returns>
			static public string ApiCall (string methodName, IDictionary<string, string> partialQuery, string responseDataFormat)
			{
				string requestData = Pipeline.BuildRequestData (partialQuery, responseDataFormat);

				Uri requestUrl = new Uri (capturesampleapp.conf.JanrainConfiguration.JanrainCaptureDomain + "/" + methodName + "?" + requestData);
				Console.WriteLine ("************ requestUrl: " + requestUrl);
				Console.WriteLine ("************ requestData: " + requestData);
				HttpWebResponse response = Pipeline.PostRequest (requestData, requestUrl, partialQuery);
				Console.WriteLine ("************ ApiCall-response: " + response);
				string json_response = Pipeline.ExtractResponseData (response); 
				Console.WriteLine ("************ ApiCall-json_response:" + json_response);
				return json_response;
			}

			private static string ExtractResponseData (HttpWebResponse response)
			{
				// Console.WriteLine("************ ExtractResponseData-response: " + response);
				// StreamReader responseStream = new StreamReader(response.GetResponseStream());
				// Console.WriteLine("************ ExtractResponseData-responseStream.ReadToEnd (): " + responseStream.ReadToEnd ());
				if (response != null) {
					using (StreamReader responseStream = new StreamReader(response.GetResponseStream())) {
						return responseStream.ReadToEnd ();
					}
				} else {
					return ""; 	
				}
			}

			private static HttpWebResponse PostRequest (string data, Uri url, IDictionary<string, string> partialQuery)
			{

				Dictionary<string, string> query = new Dictionary<string, string> (partialQuery);
				
				Console.WriteLine ("*** url.AbsoluteUri: " + url.AbsoluteUri);
				if (query.ContainsKey ("authorization_token")) {
					Console.WriteLine ("*** authorization_token: " + query ["authorization_token"]);
				}
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
				request.Method = "POST";
				
				request.ContentType = "application/x-www-form-urlencoded";
				
				if (query.ContainsKey ("authorization_token")) {
					request.Headers ["Authorization"] = "OAuth " + query ["authorization_token"];
				}
				
				return (HttpWebResponse)request.GetResponse ();

			}

			private static string BuildRequestData (IDictionary<string, string> partialQuery, string format)
			{
				var query = Pipeline.CompileCompleteQuery (partialQuery, format);
				return Pipeline.BuildDataString (query);
			}

			public static string BuildDataString (IDictionary<string, string> query)
			{
				StringBuilder sb = new StringBuilder ();
				foreach (KeyValuePair<string, string> e in query) {
					if (sb.Length > 0) {
						sb.Append ('&');
					}

					sb.Append (HttpUtility.UrlEncode (e.Key, Encoding.UTF8));
					sb.Append ('=');
					sb.Append (HttpUtility.UrlEncode (e.Value, Encoding.UTF8));
				}
				return sb.ToString ();
			}

			private static IDictionary<string, string> CompileCompleteQuery (IDictionary<string, string> partialQuery, string format)
			{
				Dictionary<string, string> query = new Dictionary<string, string> (partialQuery);
				if (!query.ContainsKey ("format")) {
					query.Add ("format", format);
				}
				if (!query.ContainsKey ("client_id")) {
					query.Add ("client_id", capturesampleapp.conf.JanrainConfiguration.JanrainCaptureClientID);
				}
				return query;
			}
		}
		
	}
}

