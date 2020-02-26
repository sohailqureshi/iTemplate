using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace iTemplate.Web.Models
{
  // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
  public class ApplicationRoleManager : RoleManager<ApplicationRole>
  {
    public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore): base(roleStore)
    {
    }

    public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
    {
      return new ApplicationRoleManager(new RoleStore<ApplicationRole>(context.Get<ApplicationDbContext>()));
    }
  }
}
