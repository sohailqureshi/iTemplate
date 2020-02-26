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
  public class StaticCategoryController : Controller
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: Admin/StaticCategory
    public ActionResult Index(int selected = 0)
    {
      return View(db.StaticCategories.OrderBy(ob => ob.SortOrder).ToList());
    }

    // GET: Admin/StaticCategory/Create
    public ActionResult Create()
    {
      return View(new StaticCategory());
    }

    // POST: Admin/StaticCategory/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Id,Name,Description,SortOrder,IsPublished")] StaticCategory staticCategory)
    {
      if (ModelState.IsValid)
      {
        db.StaticCategories.Add(staticCategory);
        db.SaveChanges();

        return RedirectToAction("Index");
      }

      return View(staticCategory);
    }

    // GET: Admin/StaticCategory/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      StaticCategory staticCategory = db.StaticCategories.Find(id);
      if (staticCategory == null)
      {
        return HttpNotFound();
      }
      return View(staticCategory);
    }

    // POST: Admin/StaticCategory/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "Id,Name,Description,SortOrder,IsPublished")] StaticCategory staticCategory)
    {
      if (ModelState.IsValid)
      {
        db.Entry(staticCategory).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(staticCategory);
    }

    // GET: Admin/StaticCategory/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      StaticCategory staticCategory = db.StaticCategories.Find(id);
      if (staticCategory == null)
      {
        return HttpNotFound();
      }
      return View(staticCategory);
    }

    // POST: Admin/StaticCategory/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      StaticCategory staticCategory = db.StaticCategories.Find(id);
      db.StaticCategories.Remove(staticCategory);
      db.SaveChanges();
      return RedirectToAction("Index");
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
