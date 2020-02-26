using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iTemplate.Web.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using iTemplate.Web.Models.Data.Core;

namespace iTemplate.Web.Models
{
  [Table("SiteTheme")]
  public class SiteTheme : CoreEntity
  {
    [Required]
    [Display(Name = "Name")]
    public string Name { get; set; }

    [Required]
    [Display(Name = "Url")]
    public string Url { get; set; }

    public static SiteTheme Get(int id)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return db.SiteThemes.Find(id);
    }

    public static IEnumerable<SelectListItem> DropDown(int? selected = null)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return new SelectList(db.SiteThemes.OrderBy(ob => ob.Name), "Id", "Name", selected);
    }
  }
}
