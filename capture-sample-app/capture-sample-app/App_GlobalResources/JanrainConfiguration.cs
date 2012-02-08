using System;

namespace capturesampleapp.conf
{
	
	public class JanrainConfiguration
	{
		public static string ThisAppServerUrl
        {
            get
            {
                return "http://127.0.0.1:8080/";
            }
        }
		public static string JanrainCaptureClientID
        {
            get
            {
                return "YOUR_CLIENT_ID"; 
				
            }
        }
		public static string JanrainCaptureClientSecret
        {
            get
            {
                return "YOUR_CLIENT_SECRET";
				
            }
        }
		public static string JanrainCaptureDomain
        {
            get
            {
                return "https://YOUR_CAPTURE_INSTANCE.janraincapture.com";
				
            }
        }
		public static string JanrainCaptureUIServer
        {
            get
            {
                return "https://YOUR_CAPTURE_INSTANCE.janraincapture.com";
				
            }
        }
		public static string JanrainCaptureSSOServer
        {
            get
            {
                return "https://YOUR_CAPTURE_INSTANCE.janrainsso.com";
				
            }
        }
		public static string JanrainCaptureEditProfileWithFancybox
        {
            get
            {
                return "true";
            }
        }
		public static string JanrainCaptureDoProfileSync
        {
            get
            {
                return "true";
            }
        }	
	}
}