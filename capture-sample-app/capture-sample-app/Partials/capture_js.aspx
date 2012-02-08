<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<script type='text/javascript'>

  var CAPTURE = {};

  CAPTURE.closeProfileEditor = function() {

  <% if (capturesampleapp.conf.JanrainConfiguration.JanrainCaptureDoProfileSync == "true") { %> 

    // if you ARE syncing data, then you need to call the profile_sync function:
    // window.location.href = '${createLink(controller: 'profile', action: 'profile_edit_finished')}';
 
	
 	window.location = './EditProfileFinished/'; 
  <% } else { %> 

    alert ("not syncing");
    // if NOT syncing data from Capture to your user table, you can just refresh the page:
    window.location = ".";

  <% } %> 


  };

  CAPTURE.resize = function(jargs) {

    var args = JSON.parse(jargs);

    jQuery("#fancybox-inner").css({
      "width": args.w + "px",
      "height": args.h + "px"
    });
    jQuery("#fancybox-wrap").css({
      "width": args.w + "px",
      "height": args.h + "px"
    });
    jQuery("#fancybox-content").css({
      "height": args.h + "px",
      "width": args.w + "px"
    });
    jQuery("#fancybox-frame").css({
      "width": args.w + "px",
      "height": args.h + "px"
    });

    jQuery.fancybox.resize();
    jQuery.fancybox.center();
  };

  //Callback fired after recover_password form is submitted
  // if recover_password_callback param passed in with capture signin link (see make_signin_link())
  CAPTURE.recoverPasswordCallback = function() {
    $.fancybox.close();
    $('#message').show();
    $('#message').text("We've sent an email with instructions for creating a new password. Your existing password has not been changed.");
  };

  //For custom redirect url for email verification.
  // 'settings/set_default' api call for setting the verify_email_redirect key with this url and params (<domain>/index.php?email_verified=true)
  CAPTURE.emailVerifiedCallback = function() {
    $.urlParam = function(name) {
      var results = new RegExp('[\\?&]' + name + '=([^&#]*)').exec(window.location.href);
      if (results !== null) {
        return results[1];
      } else {
        return 0;
      }
    }
    if ($.urlParam('email_verification') === 'true' && !$.urlParam('verification')) {
      $('#message').show();
      $('#message').text("Thank you. Your email has been verified.");
    } else if ($.urlParam('email_verification') === 'true' && $.urlParam('verification') === 'invalid') {
      $('#message').show();
      $('#message').text("Your email was not verified. Please try again.");
    }
  }();
</script>
