using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTemplate.Web.Models.Data
{
  [Table("ContactDetail")]
  public class ContactDetail : BaseEntity
  {
    [ForeignKey("AddressBook")]
    public virtual int AddressBookId { get; set; }
    public virtual AddressBook AddressBook { get; set; }

    [Display(Name = "Contact Details")]
    public virtual string ContactText { get; set; }

    public virtual int ContactTypeId { get; set; }

    public ContactDetail(): this(0, string.Empty)
    {
    }

    public ContactDetail(int id = 0, string text="")
    {
      this.ContactTypeId = id;
      this.ContactText = text;
    }

    public static ContactDetail Get(int id)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      return db.ContactDetails.Find(id);
    }
  }
}
