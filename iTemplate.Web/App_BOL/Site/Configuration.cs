using System.Web;
using iTemplate.Web.Models;
using iTemplate.Web.Models.Data;
using iTemplate.Web.Constants;
using Microsoft.AspNet.Identity;

namespace iTemplate.Web.Data.Site
{
  public class Configuration
  {
    const string SessionKey = "iTemplate.Site.Configuration";

    private static SiteConfiguration siteSettings
    {
      get
      {
        ApplicationDbContext db = new ApplicationDbContext();
        if (HttpContext.Current.Session[SessionKey] == null)
        {
          HttpContext.Current.Session[SessionKey] = db.SiteConfigurations.Find(1);
        }
        return (SiteConfiguration)HttpContext.Current.Session[SessionKey];
      }
    }

    public static string SiteName
    {
      get { return siteSettings.Name; }
    }

    public static string SiteLogoUrl
    {
      get { return siteSettings.LogoUrl; }
    }

    public static string AdminRoleID
    {
      get { return siteSettings.AdministratorRoleID; }
    }

    public static Address Address
    {
      get { return siteSettings.Address; }
    }

    public static bool IsEmailEnabled
    {
      get { return (RegistrationMethod == Enums.RegistrationMethods.EmailConfirmation); }
    }

    public static Enums.RegistrationMethods RegistrationMethod
    {
      get { return (Enums.RegistrationMethods)siteSettings.RegistrationMethod; }
    }

    public static string Disclaimer
    {
      get { return siteSettings.Disclaimer; }
    }

    public static int ThemeId
    {
      get { return siteSettings.ThemeId; }
    }

    public static string ThemeUrl
    {
      get
      {
        ApplicationDbContext db = new ApplicationDbContext();
        return db.SiteThemes.Find(ThemeId).Url;
      }
    }

    /// <summary>
    /// Get currently logged in user ID
    /// </summary>
    public static string UserId
    {
      get { return HttpContext.Current.User.Identity.GetUserId(); }
    }

    /// <summary>
    /// Get currently logged in user name
    /// </summary>
    public static string UserName
    {
      get{ return HttpContext.Current.User.Identity.Name; }
    }

    public static void Reset()
    {
      HttpContext.Current.Session[SessionKey] = null;
    }
  }
}