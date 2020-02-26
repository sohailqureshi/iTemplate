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
  public class ApplicationUserRole : IdentityUserRole
  {
    [ForeignKey("Users")]
    new public virtual string UserId { get; set; }
    public virtual ApplicationUser User { get; set; }

    [ForeignKey("Roles")]
    new public virtual string RoleId { get; set; }
    public virtual ApplicationRole Role { get; set; }
  }
}
