using iTemplate.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using iTemplate.Web.Areas.Admin.ViewModels;
using System.Web.Security;

namespace iTemplate.Web.Areas.Admin.Controllers
{
  public class UserController : Controller
 {
    private ApplicationUserManager UserManager
    {
      get
      {
        return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
      }
    }
    //
    // GET: /Users/
    public async Task<ActionResult> Index()
    {
      return PartialView(await UserManager.Users.OrderBy(ob => ob.FirstName).ToListAsync());
    }

    //
    // GET: /Users/Create
    public  ActionResult Create()
    {
      ViewBag.RolesSelected = new List<string>();
      return PartialView(new EditUserViewModel());
    }

    //
    // POST: /Users/Create
    [HttpPost]
    public async Task<ActionResult> Create(EditUserViewModel newUser, params string[] selectedRoles)
    {
      ViewBag.RolesSelected = selectedRoles == null ? new List<string>() : selectedRoles.ToList();
      if (ModelState.IsValid && selectedRoles != null)
      {
        var user = new ApplicationUser {
          UserName = newUser.Email,
          Email = newUser.Email,
          FirstName = newUser.FirstName,
          LastName= newUser.LastName
        };

        string password = Membership.GeneratePassword(Membership.MinRequiredPasswordLength, Membership.MinRequiredNonAlphanumericCharacters);
        var adminresult = await UserManager.CreateAsync(user, password);

        //Add User to the selected Roles 
        if (adminresult.Succeeded)
        {
          var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
          if (!result.Succeeded)
          {
            ModelState.AddModelError("", result.Errors.First());
            return PartialView(newUser);
          }
        }
        else
        {
          ModelState.AddModelError("", adminresult.Errors.First());
          return PartialView(newUser);

        }
        return RedirectToAction("Index");
      }

      if(selectedRoles == null) { ModelState.AddModelError("", "Please select a Role for the user"); }
      return PartialView(newUser);
    }

    //
    // GET: /Users/Edit/1
    public async Task<ActionResult> Edit(string id)
    {
      var user = await UserManager.FindByIdAsync(id);
      if (id == null || user == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      ViewBag.RolesSelected = await UserManager.GetRolesAsync(id);
      return PartialView(new EditUserViewModel()
      {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
      });
    }

    //
    // POST: /Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "Email,Id,FirstName,LastName")] EditUserViewModel editUser, params string[] selectedRoles)
    {
      ViewBag.RolesSelected = selectedRoles == null ? new List<string>() : selectedRoles.ToList();
      if (ModelState.IsValid && selectedRoles != null)
      {
        var user = await UserManager.FindByIdAsync(editUser.Id);
        if (user == null){ return HttpNotFound(); }

        user.FirstName = editUser.FirstName;
        user.LastName = editUser.LastName;
        user.UserName = editUser.Email;
        user.Email = editUser.Email;

        var userRoles = await UserManager.GetRolesAsync(user.Id);
        var result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.ToArray<string>());
        if (!result.Succeeded)
        {
          ModelState.AddModelError("", result.Errors.First());
          return PartialView(editUser);
        }

        //selectedRole = selectedRole ?? new string[] { };
        result = await UserManager.AddToRolesAsync(user.Id, selectedRoles.ToArray<string>());
        if (!result.Succeeded)
        {
          ModelState.AddModelError("", result.Errors.First());
          return PartialView(editUser);
        }

        return RedirectToAction("Index");
      }
      ModelState.AddModelError("", "Something failed.");
      return PartialView(editUser);
    }

    //
    // GET: /Users/Delete/5
    public async Task<ActionResult> Delete(string id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var user = await UserManager.FindByIdAsync(id);
      if (user == null)
      {
        return HttpNotFound();
      }
      return PartialView(user);
    }

    //
    // POST: /Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(string id)
    {
      if (ModelState.IsValid)
      {
        if (id == null)
        {
          return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        var user = await UserManager.FindByIdAsync(id);
        if (user == null)
        {
          return HttpNotFound();
        }
        var result = await UserManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
          ModelState.AddModelError("", result.Errors.First());
          return PartialView();
        }
        return RedirectToAction("Index");
      }
      return PartialView();
    }
  }
}
