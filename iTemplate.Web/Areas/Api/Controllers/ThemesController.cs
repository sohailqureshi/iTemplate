using iTemplate.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace iTemplate.Web.Areas.Controllers.Api
{
  public class ThemesController : ApiController
  {
    // GET api/themes/5
    public string GetUrl(int id)
    {
      return Url.Content(SiteTheme.Get(id).Url);
    }
  }
}
