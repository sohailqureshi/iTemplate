using iTemplate.Web.Models.Data.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace iTemplate.Web.Models.Data
{ 
  [Table("GeoContinent")]
  public class Continent: CoreEntity
  {
    [Required]
    [Display(Name = "ISO Code")]
    public virtual string IsoCode { get; set; }

    [Required]
    [Display(Name = "Continent Name")]
    public virtual string Name { get; set; }

    public static Continent Get(int id)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return db.Continents.Find(id);
    }

    public static IEnumerable<SelectListItem> DropDown()
    {
      return DropDown(null);
    }

    public static IEnumerable<SelectListItem> DropDown(int? selected)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return new SelectList(db.Continents.OrderBy(ob => ob.Name), "Id", "Name", selected);
    }
  }
}
  