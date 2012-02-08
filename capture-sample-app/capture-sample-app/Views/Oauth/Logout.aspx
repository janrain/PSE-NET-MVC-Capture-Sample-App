<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<div>
		<% Html.RenderPartial("~/Partials/navigation.aspx"); %>
		<br/>
		<%= Html.Encode(ViewData["Message"]) %>
		<br/>
	</div>
	<script type='text/javascript'>

    // (see above) set this to false if you are viewing the data from new_access_token.
    var do_the_redirect = true;

    if (do_the_redirect) {
        if (window.top == window.self) {
        	window.location = "." + '<%= ViewData["Redirect_To"] %>'; 
		}
        else {
            window.parent.location.reload();
        }
    }
	</script>
</body>
