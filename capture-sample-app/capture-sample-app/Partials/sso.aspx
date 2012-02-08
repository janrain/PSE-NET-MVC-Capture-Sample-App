<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<% if (capturesampleapp.conf.JanrainConfiguration.JanrainCaptureSSOServer != "") { %>

  <script src='<%= capturesampleapp.conf.JanrainConfiguration.JanrainCaptureSSOServer %>/sso.js' type="text/javascript"></script>
  <script type="text/javascript">
  JANRAIN.SSO.CAPTURE.check_login({
      sso_server: '<%= capturesampleapp.conf.JanrainConfiguration.JanrainCaptureSSOServer %>',
      client_id: '<%= capturesampleapp.conf.JanrainConfiguration.JanrainCaptureClientID %>',
      redirect_uri: '<%= capturesampleapp.conf.JanrainConfiguration.ThisAppServerUrl %>/oauth/oauth_redirect?from_sso=1',
      logout_uri: '<%= capturesampleapp.conf.JanrainConfiguration.ThisAppServerUrl %>/oauth/logout',
      xd_receiver: '<%= capturesampleapp.conf.JanrainConfiguration.ThisAppServerUrl %>/Content/xdcomm.html'
  });
  </script>

  <script type="text/javascript">
  function sso_logout() {
    JANRAIN.SSO.CAPTURE.logout({
        sso_server: '<%= capturesampleapp.conf.JanrainConfiguration.JanrainCaptureSSOServer %>',
        logout_uri: '<%= capturesampleapp.conf.JanrainConfiguration.ThisAppServerUrl %>/oauth/logout'
    });
  };
  </script>


<% } %>
