using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTemplate.Web.Models.Data
{
  [Table("AddressBook")]
  public class AddressBook : BaseEntity
  {
    public virtual int AddressTypeId { get; set; }

    [Required]
    [Display(Name = "House No/Name")]
    public virtual string Line1 { get; set; }

    [Display(Name = "Street")]
    public virtual string Line2 { get; set; }

    [Display(Name = "Town/City")]
    public virtual string Line3 { get; set; }

    [Display(Name = "County")]
    public virtual string Line4 { get; set; }

    [Display(Name = "City")]
    public virtual string Line5 { get; set; }

    [Display(Name = "Postal Code")]
    public virtual string PostalCode { get; set; }

    public virtual int CountryId { get; set; }

    [Display(Name = "Latitude")]
    public virtual string Latitude { get; set; }

    [Display(Name = "Longitude")]
    public virtual string Longitude { get; set; }

    [Display(Name = "Address")]
    public virtual string AddressFull{
      get {
        var addr = Line1;
        addr += string.IsNullOrEmpty(addr) ? Line2 : string.IsNullOrEmpty(Line2) ? "" : ", " + Line2;
        addr += string.IsNullOrEmpty(addr) ? Line3 : string.IsNullOrEmpty(Line3) ? "" : ", " + Line3;
        addr += string.IsNullOrEmpty(addr) ? Line4 : string.IsNullOrEmpty(Line4) ? "" : ", " + Line4;
        addr += string.IsNullOrEmpty(addr) ? Line5 : string.IsNullOrEmpty(Line5) ? "" : ", " + Line5;
        addr += string.IsNullOrEmpty(addr) ? PostalCode : string.IsNullOrEmpty(PostalCode) ? "" : ", " + PostalCode;
        //addr += string.IsNullOrEmpty(addr) ? Country.Name : string.IsNullOrEmpty(Country.Name) ? "" : ", " + Country.Name;
        return addr;
      }
    }

    public virtual string GoogleUrl
    {
      get
      {
        var url = "http://maps.googleapis.com/maps/api/staticmap?scale=2&size=600x300&maptype=roadmap&format=png&visual_refresh=true&markers=size:mid%7Ccolor:red%7Clabel:1%7C";
        var addr = (Line1 + " " + Line2 + " " + Line3 + " " + Line4 + " " + Line5 + " " + PostalCode).Replace(" ", "+");
        return url + addr;
      }
    }

    internal AddressBook Get(int id)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return db.Addresses.Find(id);
    }
  }
}
