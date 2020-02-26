using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity;
using iTemplate.Web.Models.Data.Core;

namespace iTemplate.Web.Models.Data
{
  public class BaseEntity: CoreEntity
  {
    [Required]
    [ScaffoldColumn(false)]
    [HiddenInput(DisplayValue = false)]
    public virtual string CreatedBy { get; set; }

    [ScaffoldColumn(false)]
    [HiddenInput(DisplayValue = false)]
    public virtual string UpdatedBy { get; set; }

    [ScaffoldColumn(false)]
    [HiddenInput(DisplayValue = false)]
    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm}")]
    public virtual DateTime? UpdatedDate { get; set; }

    public BaseEntity()
    {
      try
      {
        CreatedBy = HttpContext.Current.User.Identity.GetUserId();
      }
      catch
      {
        CreatedBy = string.Empty;
      }
    }
  }
}
