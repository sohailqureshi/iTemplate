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
  public class ContactTypeController : Controller
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: Admin/ContactType
    public ActionResult Index()
    {
      return View(db.ContactTypes.Where(w => w.ParentId == 0).OrderBy(ob => ob.Name).ToList());
    }

    // GET: Admin/ContactType/Details/5
    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ContactType contactType = db.ContactTypes.Find(id);
      if (contactType == null)
      {
        return HttpNotFound();
      }
      return View(contactType);
    }

    // GET: Admin/ContactType/Create
    public ActionResult Create()
    {
      return View(new ContactType());
    }

    // POST: Admin/ContactType/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Id,ParentId,Name,Description,SortOrder,CreatedDate,IsPublished,IsDeleted")] ContactType contactType)
    {
      if (ModelState.IsValid)
      {
        db.ContactTypes.Add(contactType);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(contactType);
    }

    // GET: Admin/ContactType/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ContactType contactType = db.ContactTypes.Find(id);
      if (contactType == null)
      {
        return HttpNotFound();
      }
      return View(contactType);
    }

    // POST: Admin/ContactType/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "Id,ParentId,Name,Description,SortOrder,CreatedDate,IsPublished,IsDeleted")] ContactType contactType)
    {
      if (ModelState.IsValid)
      {
        db.Entry(contactType).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(contactType);
    }

    // GET: Admin/ContactType/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ContactType contactType = db.ContactTypes.Find(id);
      if (contactType == null)
      {
        return HttpNotFound();
      }
      return View(contactType);
    }

    // POST: Admin/ContactType/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      ContactType contactType = db.ContactTypes.Find(id);
      db.ContactTypes.Remove(contactType);
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
