using iTemplate.Web.Models.Data.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace iTemplate.Web.Models.Data
{
  [Table("ContactType")]
  public class ContactType : CoreEntity
  {
    [Display(Name = "Parent")]
    public virtual int ParentId { get; set; }

    [Display(Name = "Parent")]
    public virtual string Parent
    {
      get
      {
        ApplicationDbContext db = new ApplicationDbContext();
        return (ParentId > 0) ? db.ContactTypes.Find(this.ParentId).Name : string.Empty;
      }
    }

    [Required]
    [Display(Name = "Key")]
    public virtual string Key { get; set; }

    [Required]
    [Display(Name = "Name")]
    public virtual string Name { get; set; }

    [Display(Name = "Description")]
    public virtual string Description { get; set; }

    [Display(Name = "Sort Order")]
    public virtual int SortOrder { get; set; }

    public ContactType()
    {
      this.ParentId = 0;
      this.Key = Guid.NewGuid().ToString();
    }

    public static ContactType Get(int id)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return db.ContactTypes.Find(id);
    }

    public static ContactType GetFirst(string key)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return db.ContactTypes
        .Where(w => w.Key.Equals(key))
        .OrderBy(ob => ob.SortOrder)
        .FirstOrDefault();
    }

    public static IEnumerable<SelectListItem> DropDown(int? selected =null)
    {
      ApplicationDbContext db = new ApplicationDbContext();

      return new SelectList(db.ContactTypes
        .Where(w => w.ParentId == 0)
        .OrderBy(ob => ob.Name), "Id", "Name", selected);
    }

    public static IEnumerable<SelectListItem> DropDown(string key, int? selected=null)
    {
      ApplicationDbContext db = new ApplicationDbContext();

      var parentId = db.ContactTypes.FirstOrDefault(w => w.Key.Equals(key)).Id;
      return new SelectList(db.ContactTypes
        .Where(w => w.ParentId == parentId)
        .OrderBy(ob => ob.SortOrder), "Id", "Name", selected);
    }
  }
}
