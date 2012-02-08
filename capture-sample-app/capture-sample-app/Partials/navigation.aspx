<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %> 
<%@ Import Namespace="capturesampleapp.Helpers" %>   

<div id="navigation">


<% if ( capturesampleapp.OauthService.getCurrentSessionAccessToken() != null  ) {  %>
  &nbsp;&nbsp;&nbsp; <%= (Session["Welcome_Message"] ) %>!&nbsp;&nbsp;&nbsp;

  <% if (@ApplicationInstance.Application["Janrain_Edit_Profile_With_Fancybox"] == "true") { %>
    <a href="/Profile/ProfileWithTokenRefresh"
            id="profile_link"
            class="iframe">Edit Profile</a>
  <% } else { %>

    <% if (ViewContext.TempData["page_name"] != "editprofile" ) { %>

      <a href="/Profile/EditProfile"
              id="edit_profile">Edit Your Profile</a>

    <% } else { %>
      Edit Your Profile
    <% } %>

  <% } %>

  <% if (Session["Janrain_SSO_Server"] != null) { %>
    <a href="javascript:sso_logout();">Log Out</a>
  <% } else { %>|
    <a href="/Oauth/Logout">Log Out</a>
  <% } %>

<% } else {  %>
  &nbsp;&nbsp;&nbsp;
	<a 
		id="signin_link" 
	   	class='iframe' 
		href="<%=  
		JanrainHelper.SignInHref(System.Convert.ToString(capturesampleapp.conf.JanrainConfiguration.JanrainCaptureUIServer), System.Convert.ToString(capturesampleapp.conf.JanrainConfiguration.ThisAppServerUrl), System.Convert.ToString(capturesampleapp.conf.JanrainConfiguration.JanrainCaptureClientID)) %>">  
		Sign-In / Register</a>
<% } %>

<% if (ViewContext.TempData["captureMessage"] != null ) { %>&nbsp;&nbsp;&nbsp;
  ViewContext.TempData["captureMessage"]
<% } %>

<div id='message' style="color: blue; padding: 20px; display: none"></div>

<% if (ViewContext.TempData["user_friendly_message"] != null ) { %>
  <script type="text/javascript">
    $('#message').show();
    $('#message').text('ViewContext.TempData["user_friendly_message"]');
  </script>
<% } %>

</div>		