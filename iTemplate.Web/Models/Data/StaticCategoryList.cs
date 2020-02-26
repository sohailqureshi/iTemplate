using iTemplate.Web.Models.Data.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace iTemplate.Web.Models.Data
{
  [Table("StaticCategoryList")]
  public class StaticCategoryList: CoreEntity
  {
    [Required]
    [Display(Name = "Name")]
    public virtual string Name { get; set; }

    [Display(Name = "Description")]
    public virtual string Description { get; set; }

    [Display(Name = "Sort Order")]
    public virtual int SortOrder { get; set; }

    public virtual StaticCategory StaticCategory { get; set; }

    public StaticCategoryList()
    {
      this.Description = string.Empty;
    }

    public static StaticCategoryList Get(int id)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return db.StaticCategoryLists.Find(id);
    }

    public static IEnumerable<SelectListItem> DropDown(int categoryId, int? selected = -1)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return new SelectList(db.StaticCategoryLists
        .Where(w => w.StaticCategory.Id == categoryId)
        .OrderBy(ob => ob.Name), "Id", "Name", selected);
    }
  }
}