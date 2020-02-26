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
using Microsoft.AspNet.Identity;
using System.Collections.ObjectModel;
using iTemplate.Web.Models.ViewModel;
using AutoMapper;

namespace iTemplate.Web.Controllers
{
  public class ContactDetailController : Controller
  {
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: ContactDetail
    public ActionResult Index()
    {
      ApplicationDbContext db = new ApplicationDbContext();
      ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());
      return View(db.Addresses.Where(w => w.CreatedBy.Equals(currentUser.Id)).ToList());
    }

    // GET: ContactDetail/Create
    public ActionResult Create()
    {
      var model = new AddressBookViewModel(null);

      return View(model);
    }

    // POST: ContactDetail/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(AddressBookViewModel model)
    {
      if (ModelState.IsValid)
      {
        ApplicationDbContext db = new ApplicationDbContext();
        AddressBook addressBook = new AddressBook();
        Mapper.Initialize(cfg => { cfg.CreateMap<AddressBookViewModel, AddressBook>(); });
        Mapper.Map<AddressBookViewModel, AddressBook>(model, addressBook);

        foreach (var item in model.ContactDetails)
        {
          db.ContactDetails.Add(new ContactDetail(item.ContactTypeId, item.ContactText));
        }

        db.Addresses.Add(addressBook);
        db.SaveChanges();

        return RedirectToAction("Index");
      }

      // Somethings gone wrong - check the errors and resume page
      MvcEmpty.Library.ModelState.Error.Show(ViewData.ModelState.Values);
      return View(model);
    }

    // GET: ContactDetail/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      
      AddressBook addressBook = db.Addresses.Find(id);
      if (addressBook == null)
      {
        return HttpNotFound();
      }
      var currentUser = db.Users.Find(User.Identity.GetUserId());
      if (!addressBook.CreatedBy.Equals(currentUser.Id))
      {
        return HttpNotFound();
      }

      var model = new AddressBookViewModel(addressBook);

      //Mapper.Initialize(cfg => { cfg.CreateMap<AddressBook, AddressBookViewModel>(); });
      //Mapper.Map<AddressBook, AddressBookViewModel>(addressBook, model);
      //model.LoadContacts();

      return View(model);
    }

    // POST: ContactDetail/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(AddressBookViewModel model)
    {
      if (ModelState.IsValid)
      {
        AddressBook abExists = db.Addresses.Find(model.Id);
        Mapper.Initialize(cfg => { cfg.CreateMap<AddressBookViewModel, AddressBook>(); });
        Mapper.Map<AddressBookViewModel, AddressBook>(model, abExists);

        //db.Entry(abExists).CurrentValues.SetValues(model);
        //db.Entry(abExists).State = EntityState.Modified;

        Mapper.Initialize(cfg => cfg.CreateMap<ContactDetailViewModel, ContactDetail>());
        var cdList = model.ContactDetails.ToList();
        foreach (var item in cdList)
        {
          var cdExists = db.ContactDetails.Find(item.Id);
          if (cdExists == null) {
            // Existing contact details not found (new one)
            if (!item.IsDeleted) //Check if new one has not been deleted
            {
              var contactDetail = new ContactDetail()
              {                
                AddressBook = abExists,
                AddressBookId = abExists.Id,
                ContactTypeId = item.ContactTypeId,
                ContactText = item.ContactText,
              };

              db.Entry(contactDetail).State = EntityState.Added;
              db.SaveChanges();
            }
          }
          else
          {
            if (item.IsDeleted)
            {
              db.Entry(cdExists).State = EntityState.Deleted;
            }
            else
            {
              cdExists.ContactTypeId = item.ContactTypeId;
              cdExists.ContactText = item.ContactText;

              db.Entry(cdExists).State = EntityState.Modified;
            }
          }
        }

        db.SaveChanges();

        return RedirectToAction("Index");
      }

      // Somethings gone wrong - check the errors and resume page
      MvcEmpty.Library.ModelState.Error.Show(ViewData.ModelState.Values);
      return View(model);
    }

    // GET: ContactDetail/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      AddressBook addressBook = db.Addresses.Find(id);
      if (addressBook == null)
      {
        return HttpNotFound();
      }
      return View(addressBook);
    }

    // POST: ContactDetail/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      AddressBook addressBook = db.Addresses.Find(id);
      db.Addresses.Remove(addressBook);
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
