using iTemplate.Web.Models;
using iTemplate.Web.Models.Data;
using System.Web.Mvc;
using System.Web;
using System.Data.Entity;
using System.Net;

namespace iTemplate.Web.Areas.Admin.Controllers
{
  public class SiteNavigationController : Controller
  {
    ApplicationDbContext db = new ApplicationDbContext();
    public ActionResult Index()
    {
      return View(db.SiteNavigationItems);
    }

    // GET: SiteNavigation/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      SiteNavigationItem siteNavigation = db.SiteNavigationItems.Find(id);
      if (siteNavigation == null)
      {
        return HttpNotFound();
      }
      return View(siteNavigation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "Id,ParentId,Text,Description,AreaName,ControllerName,ActionName,Icon,SortOrder,IsDivider")] SiteNavigationItem siteNavigationItem)
    {
      if (ModelState.IsValid)
      {
        db.Entry(siteNavigationItem).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(siteNavigationItem);
    }
  }
}
