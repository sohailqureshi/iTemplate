using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iTemplate.Web.Models;
using iTemplate.Web.Models.Data;

namespace iTemplate.Web.Areas.Admin.Controllers
{
  public class StaticCategoryListController : Controller
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: Admin/StaticCategory
    public ActionResult Index(int? id)
    {
      int categoryId = (id.HasValue) ? Convert.ToInt32(id) : 0;
      ViewBag.StaticCategory = StaticCategory.DropDown(categoryId);
      return View();
    }

    // GET: Admin/StaticCategory/Create
    public ActionResult Create(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      int categoryId = Convert.ToInt32(id);
      return View(new StaticCategoryList() { StaticCategory = StaticCategory.Get(categoryId) });
    }

    // POST: Admin/StaticCategory/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Id,Name,StaticCategoryId,Description,SortOrder,IsPublished")] StaticCategoryList staticCategoryList)
    {
      if (ModelState.IsValid)
      {
        db.StaticCategoryLists.Add(staticCategoryList);
        db.SaveChanges();

        return RedirectToAction("Index", new { id= staticCategoryList.StaticCategory.Id });
      }

      return View(staticCategoryList);
    }

    // GET: Admin/StaticCategory/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null )
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      StaticCategoryList staticCategoryList = db.StaticCategoryLists.Find(id);
      if (staticCategoryList == null)
      {
        return HttpNotFound();
      }

      return View(staticCategoryList);
    }

    // POST: Admin/StaticCategoryList/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "Id,Name,StaticCategoryId,Description,SortOrder,IsPublished")] StaticCategoryList staticCategoryList)
    {
      if (ModelState.IsValid)
      {
        db.Entry(staticCategoryList).State = EntityState.Modified;
        db.SaveChanges();

        return RedirectToAction("Index", new { id = staticCategoryList.StaticCategory.Id });
      }
      return View(staticCategoryList);
    }

    // GET: Admin/StaticCategoryList/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      StaticCategoryList staticCategoryList = db.StaticCategoryLists.Find(id);
      if (staticCategoryList == null)
      {
        return HttpNotFound();
      }
      return View(staticCategoryList);
    }

    // POST: Admin/StaticCategoryList/Delete/5
    [ValidateAntiForgeryToken]
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      StaticCategoryList staticCategoryList = db.StaticCategoryLists.Find(id);
      db.StaticCategoryLists.Remove(staticCategoryList);
      db.SaveChanges();

      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult GetCategoryLists(int id)
    {
      var model = db.StaticCategoryLists.Where(w => (w.StaticCategory.Id == id)).OrderBy(ob => ob.Name).ToList();
      return PartialView("_Index", model);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
