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
  public class StaticCategoryListItemController : Controller
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: Admin/StaticCategoryListItem
    public ActionResult Index(int? id)
    {
      int categoryListId = (id.HasValue) ? Convert.ToInt32(id) : 0;
      int categoryId = (categoryListId > 0) ? db.StaticCategoryLists.Find(categoryListId).StaticCategory.Id : 0;

      ViewBag.StaticCategory = StaticCategory.DropDown(categoryId);
      ViewBag.StaticCategoryList = StaticCategoryList.DropDown(categoryId, categoryListId);

      return View();
    }

    // GET: Admin/StaticCategoryListItem/Create
    public ActionResult Create(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      return View(new StaticCategoryListItem());
    }

    // POST: Admin/StaticCategoryListItem/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Id,StaticCategoryListId,Name,Description,SortOrder,IsPublished")] StaticCategoryListItem staticCategoryListItem)
    {
      if (ModelState.IsValid)
      {
        db.StaticCategoryListItems.Add(staticCategoryListItem);
        db.SaveChanges();
        return RedirectToAction("Index", new { id = staticCategoryListItem.StaticCategoryList.Id });
      }
      return View(staticCategoryListItem);
    }

    // GET: Admin/StaticCategoryListItem/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      StaticCategoryListItem staticCategoryListItem = db.StaticCategoryListItems.Find(id);
      if (staticCategoryListItem == null)
      {
        return HttpNotFound();
      }
      return View(staticCategoryListItem);
    }

    // POST: Admin/StaticCategoryListItem/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "Id,StaticCategoryListId,Name,Description,SortOrder,IsPublished")] StaticCategoryListItem staticCategoryListItem)
    {
      if (ModelState.IsValid)
      {
        db.Entry(staticCategoryListItem).State = EntityState.Modified;
        db.SaveChanges();

        return RedirectToAction("Index", new { id = staticCategoryListItem.StaticCategoryList.Id });
      }
      return View(staticCategoryListItem);
    }

    // GET: Admin/StaticCategoryListItem/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      StaticCategoryListItem staticCategoryListItem = db.StaticCategoryListItems.Find(id);
      if (staticCategoryListItem == null)
      {
        return HttpNotFound();
      }
      return View(staticCategoryListItem);
    }

    // POST: Admin/StaticCategoryListItem/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      StaticCategoryListItem staticCategoryListItem = db.StaticCategoryListItems.Find(id);
      db.StaticCategoryListItems.Remove(staticCategoryListItem);
      db.SaveChanges();
      return RedirectToAction("Index");
    }


    public JsonResult GetCategoryList(int categoryId) {
      return Json(StaticCategoryList.DropDown(categoryId));
    }

    [HttpPost]
    public ActionResult GetCategoryListItems(int id)
    {
      var model = db.StaticCategoryListItems.Where(w => w.StaticCategoryList.Id == id).OrderBy(ob => ob.Name).ToList();
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
