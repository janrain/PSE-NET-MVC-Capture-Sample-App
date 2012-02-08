<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
		
	<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.4/jquery.min.js"></script>
	<script src="/Content/javascript/json/json2.js"></script>
    <script src="/Content/javascript/fancybox/jquery.fancybox-1.3.4.pack.js"></script>
	<link rel="stylesheet" href="/Content/javascript/fancybox/jquery.fancybox-1.3.4.css"/>
	<link rel="stylesheet" href="/Content/css/style.css"/>
  	<script src='<%= @ApplicationInstance.Application["Janrain_Capture_UI_Server"] %>/cdn/javascripts/capture_client.js'></script>

	<% Html.RenderPartial("~/Partials/capture_js.aspx"); %>		
	<% Html.RenderPartial("~/Partials/sso.aspx"); %>		

	<script type="text/javascript">
		jQuery(document).ready(function($) {
		
			$('#signin_link').fancybox({
				padding: 0,
				scrolling: 'no',
				autoScale: true,
				autoDimensions: false
			});
			
			$('#profile_link').fancybox({
				padding: 0,
				scrolling: 'no',
				autoScale: true,
				autoDimensions: false
			});
		
		})
	</script>
		
</head>
<body>
	<div>
		<% Html.RenderPartial("~/Partials/navigation.aspx"); %>
		<br/>
		<%= Html.Encode(ViewData["Message"]) %>
		<br/>
		<%= ViewData["ProfileEditor"]
		%>
	</div>
</body>
</html>	
