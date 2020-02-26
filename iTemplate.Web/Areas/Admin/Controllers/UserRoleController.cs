using iTemplate.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using iTemplate.Web.Areas.Admin.ViewModels;

namespace iTemplate.Web.Areas.Admin.Controllers
{
  public class UserRoleController : Controller
  {
    //IdentityManager identityManager = new IdentityManager();
    private ApplicationRoleManager RoleManager
    {
      get
      {
        return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
      }
    }

    //
    // GET: /Roles/
    public ActionResult Index()
    {
      return View(RoleManager.Roles.OrderBy(ob=>ob.Name));
    }

    //
    // GET: /Roles/Create
    public ActionResult Create()
    {
      return View();
    }

    //
    // POST: /Roles/Create
    [HttpPost]
    public async Task<ActionResult> Create(RoleViewModel roleViewModel)
    {
      if (ModelState.IsValid)
      {
        var role = new ApplicationRole(roleViewModel.Name, roleViewModel.Description, roleViewModel.IsSystem);
        var roleresult = await RoleManager.CreateAsync(role);
        if (!roleresult.Succeeded)
        {
          ModelState.AddModelError("", roleresult.Errors.First());
          return View();
        }
        return RedirectToAction("Index");
      }
      return View();
    }

    //
    // GET: /Roles/Edit/Admin
    public async Task<ActionResult> Edit(string id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var role = await RoleManager.FindByIdAsync(id);
      if (role == null)
      {
        return HttpNotFound();
      }
      RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };
      return View(roleModel);
    }

    //
    // POST: /Roles/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "Id, Name, Description")] RoleViewModel roleModel)
    {
      if (ModelState.IsValid)
      {
        var role = await RoleManager.FindByIdAsync(roleModel.Id);
        role.Name = roleModel.Name;
        await RoleManager.UpdateAsync(role);
        return RedirectToAction("Index");
      }
      return View();
    }

    //
    // GET: /Roles/Delete/5
    public async Task<ActionResult> Delete(string id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var role = await RoleManager.FindByIdAsync(id);
      if (role == null)
      {
        return HttpNotFound();
      }
      return View(role);
    }

    //
    // POST: /Roles/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(string id, string deleteUser)
    {
      if (ModelState.IsValid)
      {
        if (id == null)
        {
          return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        var role = await RoleManager.FindByIdAsync(id);
        if (role == null)
        {
          return HttpNotFound();
        }
        IdentityResult result;
        if (deleteUser != null)
        {
          result = await RoleManager.DeleteAsync(role);
        }
        else
        {
          result = await RoleManager.DeleteAsync(role);
        }
        if (!result.Succeeded)
        {
          ModelState.AddModelError("", result.Errors.First());
          return View();
        }
        return RedirectToAction("Index");
      }
      return View();
    }
  }
}
