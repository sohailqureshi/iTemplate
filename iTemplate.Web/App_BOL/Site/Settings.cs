using System.Web;
using iTemplate.Web.Models;

namespace iTemplate.Web.Data.Site
{
  public class Settings : Configuration
  {
    public static void Initialise()
    {
      Configuration.Reset();
    }
  }
}