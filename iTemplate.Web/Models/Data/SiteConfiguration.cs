using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using iTemplate.Web.Data;
using System.Data.Entity;
using iTemplate.Web.Data.Site;
using iTemplate.Web.Constants;

namespace iTemplate.Web.Models.Data
{
  [Table("SiteConfiguration")]
  public class SiteConfiguration : BaseEntity
  {
    [Required]
    [Display(Name = "Site Name")]
    public virtual string Name { get; set; }

    [Display(Name = "Logo Url")]
    public virtual string LogoUrl { get; set; }

    [Display(Name = "Address")]
    public virtual Address Address { get; set; }

    [Display(Name = "Registration Method")]
    public virtual Enums.RegistrationMethods RegistrationMethod { get; set; }

    [Display(Name = "Disclaimer")]
    public virtual string Disclaimer { get; set; }

    [Display(Name = "Default Theme")]
    public virtual int ThemeId { get; set; }

    [Display(Name = "Admin Role ID")]
    public virtual string AdministratorRoleID { get; set; }

    public SiteConfiguration()
    {
      this.Address = new Address();
    }

    public static SiteConfiguration Get()
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return db.SiteConfigurations.Find(1);
    }

    public static void ChangeTheme(int themeId)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      var sitesetting = db.SiteConfigurations.Find(1);

      sitesetting.ThemeId = themeId;
      db.Entry(sitesetting).State = EntityState.Modified;
      db.SaveChanges();

      Settings.Reset();
    }
  }
}