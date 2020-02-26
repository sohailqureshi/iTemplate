using iTemplate.Web.Models.Data.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace iTemplate.Web.Models.Data
{
  [Table("GeoLanguage")]
  public class Language: CoreEntity
  {
    [Display(Name = "Language")]
    public virtual string Name { get; set; }

    [Display(Name = "ISO Code")]
    public virtual string IsoCode { get; set; }

    public static Language Get(int id)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return db.Languages.Find(id);
    }

    public static IEnumerable<SelectListItem> DropDown(int? selected)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return new SelectList(db.Languages.OrderBy(ob => ob.Name), "Id", "Name", selected);
    }
  }
}
  