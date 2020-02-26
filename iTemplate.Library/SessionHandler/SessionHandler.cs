using System.Web;

namespace iTemplate.Library
{
  public class SessionHandler
  {
    public static void Save(string sName, object value)
    {
      HttpContext.Current.Session["iTemplate." + sName.ToLower()] = value;
    }

    public static object Get(string sName, object value)
    {
      return value ?? HttpContext.Current.Session["iTemplate." + sName.ToLower()];
    }
  }
}
