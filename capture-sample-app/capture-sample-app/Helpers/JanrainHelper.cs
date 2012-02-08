using System;

namespace capturesampleapp.Helpers
{
	public class JanrainHelper
	{
		public static string SignInHref (
			String janrain_capture_ui_server,
			String this_app_server_url,
			String client_id)
		{
			String url = janrain_capture_ui_server + "/oauth/signin";
			String responseType = "response_type=code";
			String redirectUri = "redirect_uri=" + this_app_server_url + "oauth/OauthRedirect";
			String c_id = "client_id=" + client_id;
			String xd_receiver = "xd_receiver=" + this_app_server_url + "Content/xdcomm.html";
			String recover_password_callback = "recover_password_callback=CAPTURE.recoverPasswordCallback";

			String href = url + "?" + responseType + "&" + redirectUri + "&" + c_id + "&" + xd_receiver + "&" + recover_password_callback;
			Console.WriteLine("**** href: " + href); 
			return String.Format (href);
		}
		
	}
}

