using iTemplate.Web.Models;
using iTemplate.Web.Models.Data;
using System.Web.Mvc;
using System.Web;
using System.Data.Entity;

namespace iTemplate.Web.Areas.Admin.Controllers
{
  public class SiteThemeController : Controller
  {
    ApplicationDbContext db = new ApplicationDbContext();
    public ActionResult Index()
    {
      return View(db.SiteThemes);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(string fileTitle, HttpPostedFileBase fileUpload)
    {
      if (fileUpload != null && fileUpload.ContentLength > 0 && fileTitle.Trim().Length>0)
      {
        string fileExtension = System.IO.Path.GetExtension(fileUpload.FileName);
        if (fileExtension != ".css") { return RedirectToAction("Index"); }

        var fileName = fileUpload.FileName;
        var theme = new SiteTheme()
        {
          Name = fileTitle,
          Url = string.Format("~/Content/Themes/{0}", fileName)
        };

        var fileLocation = System.IO.Path.Combine(Server.MapPath("~/Content/Themes/"), fileName);
        if (System.IO.File.Exists(fileLocation))
        {
          System.IO.File.Delete(fileLocation);
        }

        fileUpload.SaveAs(fileLocation);

        db.SiteThemes.Add(theme);
        db.SaveChanges();
      }

      return RedirectToAction("Index");
    }
  }
}
