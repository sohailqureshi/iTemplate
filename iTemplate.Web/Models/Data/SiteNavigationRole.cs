using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using iTemplate.Web.Models.Data.Core;

namespace iTemplate.Web.Models.Data
{
  [Table("SiteNavigationRole")]
  public class SiteNavigationRole: CoreEntity
  {
    [Key]
    [Required]
    [ScaffoldColumn(false)]
    [HiddenInput(DisplayValue = false)]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public virtual int MenuId { get; set; }

    public virtual string RoleId { get; set; }

    private ICollection<int> NavigationRoles { get; set; }

    public SiteNavigationRole()
    {
      ApplicationDbContext db = new ApplicationDbContext();
      this.NavigationRoles = new HashSet<int>();

    }
    private void GetNavigationRoles()
    {
      using (ApplicationDbContext db = new ApplicationDbContext())
      {
        var navigationRoles = db.SiteNavigationRoles.Where(x => x.IsPublished);
        if (navigationRoles.Any())
        {
          foreach (var role in navigationRoles)
          {
            NavigationRoles.Add(role.Id);
          }
        }
      }
    }

    /// <summary>
    /// Single role check
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public bool IsAuthorised(int Id)
    {
      foreach (var roleId in NavigationRoles)
      {
        if (roleId == Id) { return true; }
      }

      return false;
    }
  }
}
