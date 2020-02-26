using System.Web.Mvc;

namespace iTemplate.Web.Areas.Admin.Controllers
{
  public class DashboardController : Controller
  {
    public ActionResult Index()
    {
        return PartialView();
    }
  }
}
