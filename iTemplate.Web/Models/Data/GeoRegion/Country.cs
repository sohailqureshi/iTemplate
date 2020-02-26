using iTemplate.Web.Models.Data.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace iTemplate.Web.Models.Data
{
  [Table("GeoCountry")]
  public class Country : CoreEntity
  {
    [Required]
    [Display(Name = "Continent")]
    public virtual int ContinentId { get; set; }

    [Required]
    [Display(Name = "Country Name")]
    public virtual string Name { get; set; }

    [Display(Name = "ISO-2 Code")]
    public virtual string Iso2Code { get; set; }

    [Display(Name = "ISO-3 Code")]
    public virtual string Iso3Code { get; set; }

    [Display(Name = "ISO Numeric")]
    public virtual int IsoNumeric { get; set; }

    [Display(Name = "Currency")]
    public virtual int CurrencyId { get; set; }

    [Display(Name = "Language Codes")]
    public virtual string LanguageCodes { get; set; }

    public static Country Get(int id)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return db.Countries.Find(id);
    }

    public static Country GetByIso2Code(string isoCode ="GB")
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return db.Countries.FirstOrDefault(x => x.Iso2Code.Equals(isoCode));
    }

    public static IEnumerable<SelectListItem> DropDown(int? selected)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return new SelectList(db.Countries.OrderBy(ob => ob.Name), "Id", "Name", selected);
    }
  }
}
  