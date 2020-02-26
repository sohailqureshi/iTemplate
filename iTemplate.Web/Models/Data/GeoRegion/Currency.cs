using iTemplate.Web.Models.Data.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace iTemplate.Web.Models.Data
{
  [Table("GeoCurrency")]
  public class Currency: CoreEntity
  {
    [Required]
    [Display(Name = "Currency")]
    public virtual string Name { get; set; }

    [Display(Name = "ISO Code")]
    public virtual string IsoCode { get; set; }

    [Display(Name = "ISO Numeric")]
    public virtual int IsoNumeric { get; set; }

    [Display(Name = "Minor Entity")]
    public virtual int MinorEntity { get; set; }

    public static Currency Get(int id)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return db.Currencies.Find(id);
    }

    public static IEnumerable<SelectListItem> DropDown(int? selected)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return new SelectList(db.Currencies.OrderBy(ob => ob.Name), "Id", "Name", selected);
    }
  }
}
