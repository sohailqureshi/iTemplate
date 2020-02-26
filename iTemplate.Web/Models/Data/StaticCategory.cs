using iTemplate.Web.Models.Data.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace iTemplate.Web.Models.Data
{
  [Table("StaticCategory")]
  public class StaticCategory : CoreEntity
  {
    [Required]
    [Display(Name = "Name")]
    public virtual string Name { get; set; }

    [Display(Name = "Description")]
    public virtual string Description { get; set; }

    [Display(Name = "Sort Order")]
    public virtual int SortOrder { get; set; }

    public StaticCategory() {
      this.Description = string.Empty;
    }

    public static IEnumerable<SelectListItem> DropDown(int selected = -1)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      //return new SelectList(db.StaticCategories
      //  .GroupBy(x => x.Key)
      //  .Select(x => x.FirstOrDefault())
      //  .OrderBy(ob => ob.Key)
      //  .Distinct(), "Id", "Text", selected);

      return new SelectList(db.StaticCategories
        .OrderBy(ob => ob.Name), "Id", "Name", selected);
    }

    public static StaticCategory Get(int id)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return db.StaticCategories.Find(id);
    }
  }
}