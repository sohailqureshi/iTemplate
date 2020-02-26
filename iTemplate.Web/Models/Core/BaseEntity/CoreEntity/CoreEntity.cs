using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity;

namespace iTemplate.Web.Models.Data.Core
{
  public class CoreEntity
  {
    [Key]
    [Required]
    [ScaffoldColumn(false)]
    [HiddenInput(DisplayValue = false)]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [ScaffoldColumn(false)]
    [HiddenInput(DisplayValue = false)]
    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm}")]
    public virtual DateTime CreatedDate { get; set; }

    [ScaffoldColumn(false)]
    [Display(Name = "Is Published")]
    [HiddenInput(DisplayValue = false)]
    public virtual bool IsPublished { get; set; }

    [ScaffoldColumn(false)]
    [HiddenInput(DisplayValue = false)]
    public virtual bool IsDeleted { get; set; }

    public CoreEntity()
    {
      IsPublished = true;
      IsDeleted = false;
      CreatedDate = DateTime.Now;
    }
  }
}
