using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iTemplate.Web.Models.Data
{
  [Table("SiteLookUp")]
  public class SiteLookUp
  {
    [Key]
    [Required]
    [ScaffoldColumn(false)]
    [HiddenInput(DisplayValue = false)]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public virtual int Key { get; set; }
    public virtual string Text { get; set; }
    public virtual string Value { get; set; }
    public virtual string ValueType { get; set; }
    public virtual int SortOrder { get; set; }
  }
}