using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iTemplate.Web.Models
{
  public class IdentityManager
  {

    ApplicationDbContext db = new ApplicationDbContext();
    RoleManager<ApplicationRole> roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext()));
    UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

    public UserManager<ApplicationUser> UserManager
    {
      get
      {
        return userManager;
      }
    }

    public RoleManager<ApplicationRole> RoleManager
    {
      get
      {
        return roleManager;
      }
    }

    public bool RoleExists(string name)
    {
      return roleManager.RoleExists(name);
    }

    public async Task<ApplicationRole> FindRoleByIdAsync(string id)
    {
      return await roleManager.FindByIdAsync(id);
    }

    public bool CreateRole(string name)
    {
      // Swap ApplicationRole for IdentityRole:
      var idResult = roleManager.Create(new ApplicationRole(name));
      return idResult.Succeeded;
    }

    //public async Task<IdentityResult> CreateRoleAsync(ApplicationRole role)
    //{
    //  return await roleManager.CreateAsync(role);
    //}

    //public async Task<IdentityResult> UpdateRoleAsync(ApplicationRole role)
    //{
    //  return await roleManager.UpdateAsync(role);
    //}

    //public async Task<IdentityResult> DeleteRoleAsync(ApplicationRole role)
    //{
    //  return await roleManager.DeleteAsync(role);
    //}

    public bool CreateUser(ApplicationUser user, string password)
    {
      var idResult = userManager.Create(user, password);
      return idResult.Succeeded;
    }

    //public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
    //{
    //  return await userManager.CreateAsync(user, password);
    //}

    public bool AddUserToRole(string userId, string roleName)
    {
      var idResult = userManager.AddToRole(userId, roleName);
      return idResult.Succeeded;
    }

    public void ClearUserRoles(string userId)
    {
      var user = userManager.FindById(userId);
      var currentRoles = new List<IdentityUserRole>();

      currentRoles.AddRange(user.Roles);
      foreach (var role in currentRoles)
      {
        userManager.RemoveFromRole(userId, role.RoleId);
      }
    }

    public void RemoveFromRole(string userId, string roleName)
    {
      userManager.RemoveFromRole(userId, roleName);
    }

    public void DeleteRole(string roleId)
    {
      var roleUsers = db.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId));
      var role = db.Roles.Find(roleId);

      foreach (var user in roleUsers)
      {
        this.RemoveFromRole(user.Id, role.Name);
      }

      db.Roles.Remove(role);
      db.SaveChanges();
    }

    public IEnumerable<ApplicationRole> Roles
    {
      get { return roleManager.Roles; }
    }
  }

}
