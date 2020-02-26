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
  public class ContactTypeListController : Controller
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: Admin/ContactType
    public ActionResult Index(int? id)
    {
      object storedId = (id.HasValue) ? id : (TempData["ContactTypeList.Index"] != null) ? TempData["ContactTypeList.Index"] : null;

      int parentId = Convert.ToInt32(storedId);
      var model = db.ContactTypes.Where(w => (w.ParentId == parentId)).OrderBy(ob => ob.Name).ToList();
      TempData["ContactTypeList.Index"] = storedId;
      ViewBag.contactSelected = ContactType.DropDown(parentId);

      if (Request.IsAjaxRequest())
      {
        return PartialView("_Index", model);
      }

      return View(model);
    }

    // GET: Admin/ContactType/Create
    public ActionResult Create(int? id)
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
      return View(new ContactType() { ParentId = contactType.Id });
    }

    // POST: Admin/ContactType/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Id,ParentId,Key,Name,Description,SortOrder,CreatedDate,IsPublished,IsDeleted")] ContactType contactType)
    {
      if (ModelState.IsValid)
      {
        db.ContactTypes.Add(contactType);
        db.SaveChanges();
        return RedirectToAction("Index", new { id = contactType.ParentId });
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
    public ActionResult Edit([Bind(Include = "Id,ParentId,Key,Name,Description,SortOrder,CreatedDate,IsPublished,IsDeleted")] ContactType contactType)
    {
      if (ModelState.IsValid)
      {
        db.Entry(contactType).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index", new { id = contactType.ParentId });
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
