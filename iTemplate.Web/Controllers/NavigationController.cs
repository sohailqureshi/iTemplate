using System.Web.Mvc;
using iTemplate.Web.Models;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using iTemplate.Web.Data.Site;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using iTemplate.Web.Models.Data;
using System.Web.Security;

namespace iTemplate.Web.Controllers
{
  public class NavigationController : Controller
  {
    private const bool NavbarMenu = true;
    private const bool DashboardMenu = false;

    protected ApplicationDbContext db { get; set; }
    protected UserManager<ApplicationUser> UserManager { get; set; }
    protected RoleManager<ApplicationRole> RoleManager { get; set; }

    public NavigationController()
    {
      this.db = new ApplicationDbContext();
      this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
      this.RoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));
    }

    [ChildActionOnly]
    public PartialViewResult MenuNavigation()
    {
      var navigationItems = SiteNavigationItem.Get(NavbarMenu);
      return PartialView("MenuNavigation", navigationItems);
    }

    [ChildActionOnly]
    public PartialViewResult MenuDashboard()
    {
      var navigationItems = SiteNavigationItem.Get(DashboardMenu);
      return PartialView("MenuDashboard", navigationItems);
    }
  }
}
