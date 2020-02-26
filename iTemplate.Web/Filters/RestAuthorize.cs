using System;
using System.Security;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace iTemplate.Web.Filters
{
  public class RESTAuthorizeAttribute : AuthorizeAttribute
  {
    private const string _securityToken = "token"; // Name of the url parameter.

    public override void OnAuthorization(AuthorizationContext filterContext)
    {
      if (Authorize(filterContext))
      {
        return;
      }

      HandleUnauthorizedRequest(filterContext);
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
      base.HandleUnauthorizedRequest(filterContext);
    }

    private bool Authorize(AuthorizationContext actionContext)
    {
      try
      {
        HttpRequestBase request = actionContext.RequestContext.HttpContext.Request;
        string token = request.Params[_securityToken];

        return CommonManager.IsTokenValid(token, CommonManager.GetIP(request), request.UserAgent);
      }
      catch (Exception)
      {
        return false;
      }
    }
  }

  public static class CommonManager
  {
    private const int _expirationMinutes = 10;

    public static bool IsTokenValid(string token, string ip, string userAgent)
    {
      bool result = false;

      try
      {
        // Base64 decode the string, obtaining the token:username:timeStamp.
        string key = Encoding.UTF8.GetString(Convert.FromBase64String(token));

        // Split the parts.
        string[] parts = key.Split(new char[] { ':' });
        //if (parts.Length == 3)
        //{
        //  // Get the hash message, username, and timestamp.
        //  string hash = parts[0];
        //  string username = parts[1];
        //  long ticks = long.Parse(parts[2]);
        //  DateTime timeStamp = new DateTime(ticks);

        //  // Ensure the timestamp is valid.
        //  bool expired = Math.Abs((DateTime.UtcNow - timeStamp).TotalMinutes) > _expirationMinutes;
        //  if (!expired)
        //  {
        //    //
        //    // Lookup the user's account from the db.
        //    //
        //    if (username == "john")
        //    {
        //      string password = "password";

        //      // Hash the message with the key to generate a token.
        //      string computedToken = GenerateToken(username, password, ip, userAgent, ticks);

        //      // Compare the computed token with the one supplied and ensure they match.
        //      result = (token == computedToken);
        //    }
        //  }
        //}
      }
      catch
      {
      }

      return result;
    }

    public static string GetIP(HttpRequestBase request)
    {
      string ip = request.Headers["X-Forwarded-For"]; // AWS compatibility

      if (string.IsNullOrEmpty(ip))
      {
        ip = request.UserHostAddress;
      }

      return ip;
    }
  }
}
