using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using iTemplate.Web.Models.Data;

namespace iTemplate.Web.Models
{
  public class Address
  {
    [Display(Name = "House No/Name")]
    public string Line1 { get; set; }

    [Display(Name = "Street")]
    public string Line2 { get; set; }

    [Display(Name = "Town/City")]
    public string Line3 { get; set; }

    [Display(Name = "County")]
    public string Line4 { get; set; }

    [Display(Name = "City")]
    public string Line5 { get; set; }

    [Display(Name = "Postal Code")]
    public string PostalCode { get; set; }

    [Display(Name = "Country")]
    public int CountryId { get; set; }

    [Display(Name = "Latitude")]
    public string Latitude { get; set; }

    [Display(Name = "Longitude")]
    public string Longitude { get; set; }

    [Display(Name = "Contact Telephone")]
    public string Telephone { get; set; }

    [Display(Name = "Contact Email")]
    public string Email { get; set; }

    public Address()
    {
      ApplicationDbContext db = new ApplicationDbContext();
      RegionInfo regionInfo = new RegionInfo(System.Threading.Thread.CurrentThread.CurrentUICulture.LCID);
      Country c = Country.GetByIso2Code(regionInfo.TwoLetterISORegionName);

      this.CountryId = CountryId;
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual string GoogleUrl
    {
      get
      {
        var url = "http://maps.googleapis.com/maps/api/staticmap?scale=2&size=600x300&maptype=roadmap&format=png&visual_refresh=true&markers=size:mid%7Ccolor:red%7Clabel:1%7C";
        var addr = (Line1 + " " + Line2 + " " + Line3 + " " + Line4 + " " + Line5 + " " + PostalCode).Replace(" ", "+");
        return url + addr;
      }
    }
  }
}
