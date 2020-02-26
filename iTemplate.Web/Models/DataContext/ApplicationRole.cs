using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iTemplate.Web.Models
{
  public class ApplicationRole : IdentityRole
  {
    public virtual string Description { get; set; }

    [Display(Name = "System Role?")]
    public virtual bool IsSystem { get; set; }

    public virtual ICollection<ApplicationPermission> Permissions { get; set; }

    public ICollection<ApplicationUser> RoleUsers { get; set; }

    public ApplicationRole() : base() { }

    public ApplicationRole(string name) : base(name)
    {
      RoleUsers = new HashSet<ApplicationUser>();
      Permissions = new HashSet<ApplicationPermission>();
      this.IsSystem = false;
    }

    public ApplicationRole(string name, string description) : base(name)
    {
      this.Description = description;
    }

    public ApplicationRole(string name, string description, bool isSystem) : base(name) {
      this.Description = description;
      this.IsSystem = isSystem;
    }
  }
}
