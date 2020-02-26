using iTemplate.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iTemplate.Web.Models.Data
{
  [Table("SiteNavigationItem")]
  public class SiteNavigationItem
  {
    [Key]
    [Required]
    [ScaffoldColumn(false)]
    [HiddenInput(DisplayValue = false)]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public virtual string Text { get; set; }
    public virtual string Description { get; set; }

    [Display(Name = "Area Name")]
    public virtual string AreaName { get; set; }

    [Display(Name = "Controller Name")]
    public virtual string ControllerName { get; set; }

    [Display(Name = "Action Name")]
    public virtual string ActionName { get; set; }
    public virtual string Icon { get; set; }

    [Display(Name = "Sort Order")]
    public virtual int SortOrder { get; set; }

    [Display(Name = "Is Divider?")]
    public virtual bool IsDivider { get; set; }


    [Display(Name = "In Nav Bar?")]
    public virtual bool InNavBar { get; set; }

    [Display(Name = "In Dashboard?")]
    public virtual bool InDashboard { get; set; }

    [ForeignKey("Parent")]
    public int? ParentId { get; set; }
    public virtual SiteNavigationItem Parent { get; set; }
    public virtual ICollection<SiteNavigationItem> Children { get; set; }

    public SiteNavigationItem()
    {
      this.SortOrder = 0;
      this.IsDivider = false;
      this.InNavBar = true;
      this.InDashboard = false;
    }

    public static IEnumerable<SiteNavigationItem> Get(bool navBar=true)
    {
      ApplicationDbContext db = new ApplicationDbContext();

      List<string> userRoles = new List<string>();
      var currentUser = db.Users.Find(System.Web.HttpContext.Current.User.Identity.GetUserId());
      if (currentUser != null)
      {
        foreach (var role in currentUser.Roles) { userRoles.Add(role.RoleId); }
      }

      var navigationItems = (
        from menuItems in db.SiteNavigationItems.Where(x => (x.InNavBar && navBar) || (x.InDashboard && !navBar))
        join restricted in db.SiteNavigationRoles on menuItems.Id equals restricted.MenuId into allItems
        from restrictedItems in allItems.DefaultIfEmpty()
        where userRoles.Contains(restrictedItems.RoleId) || restrictedItems.RoleId == null
        select menuItems);

      return navigationItems;
    }
  }
}
