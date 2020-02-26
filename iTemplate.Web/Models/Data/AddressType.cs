using iTemplate.Web.Models.Data.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace iTemplate.Web.Models.Data
{
  [Table("AddressType")]
  public class AddressType : CoreEntity
  {
    [Required]
    [Display(Name = "Name")]
    public virtual string Name { get; set; }

    [Display(Name = "Description")]
    public virtual string Description { get; set; }

    [Display(Name = "Sort Order")]
    public virtual int SortOrder { get; set; }

    public static AddressType Get(int id)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return db.AddressTypes.Find(id);
    }

    public static IEnumerable<SelectListItem> DropDown(int? selected)
    {
      ApplicationDbContext db = new ApplicationDbContext();

      return new SelectList(db.AddressTypes
        .OrderBy(ob => ob.Name), "Id", "Name", selected);
    }
  }
}
