using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace iTemplate.Web.Models.Data
{
  [Table("Connection")]
  public class Connection
  {
    [Key]
    [Required]
    [ScaffoldColumn(false)]
    [HiddenInput(DisplayValue = false)]
    public string Id { get; set; }
    public virtual string UserName { get; set; }
    public virtual string UserAgent { get; set; }
    public virtual string IP { get; set; }
    public virtual string Referrer { get; set; }
    public virtual string CurrentPage { get; set; }
    public virtual DateTime DateConnected { get; set; }
    public virtual DateTime? DateDisconnected { get; set; }
    public virtual bool IsConnected { get; set; }
  }
}
