using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace iTemplate.Library.Filter
{
  [AttributeUsage(AttributeTargets.Class)]
  public class ValidateAntiForgeryTokenOnAllPosts : AuthorizeAttribute
  {
    public override void OnAuthorization(AuthorizationContext filterContext)
    {
      var request = filterContext.HttpContext.Request;

      //  Only validate POSTs
      if (request.HttpMethod == WebRequestMethods.Http.Post)
      {
        //  Ajax POSTs and normal form posts have to be treated differently when it comes
        //  to validating the AntiForgeryToken
        if (request.IsAjaxRequest())
        {

          //var headers = filterContext.Request.Headers;
          //var cookie = headers
          //    .GetCookies()
          //    .Select(c => c[AntiForgeryConfig.CookieName])
          //    .FirstOrDefault();
          //var rvt = headers.GetValues("__RequestVerificationToken").FirstOrDefault();
          //AntiForgery.Validate(cookie != null ? cookie.Value : null, rvt);



          var antiForgeryCookie = request.Cookies[System.Web.Helpers.AntiForgeryConfig.CookieName];

          var cookieValue = antiForgeryCookie != null
              ? antiForgeryCookie.Value
              : null;

          System.Web.Helpers.AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
        }
        else
        {
          new ValidateAntiForgeryTokenAttribute()
              .OnAuthorization(filterContext);
        }
      }
    }
  }
}
