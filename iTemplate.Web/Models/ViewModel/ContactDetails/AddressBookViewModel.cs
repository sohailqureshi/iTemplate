using AutoMapper;
using iTemplate.Web.Models.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace iTemplate.Web.Models.ViewModel
{
  public class AddressBookViewModel
  {
    [Required]
    public virtual int Id { get; set; }

    [Required]
    [Display(Name = "Address Type")]
    public virtual int AddressTypeId { get; set; }
    public virtual IEnumerable<AddressType> AddressType { get; set; }

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

    [Display(Name = "Country")]
    public virtual int CountryId { get; set; }
    public virtual IEnumerable<Country> Country { get; set; }

    [Display(Name = "Latitude")]
    public virtual string Latitude { get; set; }

    [Display(Name = "Longitude")]
    public virtual string Longitude { get; set; }

    [Display(Name = "Contact Details")]
    public virtual ICollection<ContactDetailViewModel> ContactDetails { get; set; }

    public AddressBookViewModel()
    {
      this.Id = 0;
      this.AddressTypeId = 0;
      this.Line1 = string.Empty;
      this.ContactDetails = new Collection<ContactDetailViewModel>();
    }

    public AddressBookViewModel(AddressBook addressBook = null): this()
    {
      if (addressBook == null) {
        var newContact = new ContactDetailViewModel();
        this.ContactDetails.Add(newContact.GetFirst());
        return;
      }

      ApplicationDbContext db = new ApplicationDbContext();
      Mapper.Initialize(cfg => { cfg.CreateMap<AddressBook, AddressBookViewModel>(); });
      Mapper.Map<AddressBook, AddressBookViewModel>(addressBook, this);

      var contactDetails = db.ContactDetails.Where(x => x.AddressBookId == addressBook.Id);
      if (!contactDetails.Any()) {
        var newContact = new ContactDetailViewModel();
        this.ContactDetails.Add(newContact.GetFirst());
        return;
      }

      foreach (var item in contactDetails)
      {
        var contactDetail = new ContactDetailViewModel()
        {
          Id = item.Id,
          ContactTypeId = item.ContactTypeId,
          ContactText = item.ContactText,
          IsDeleted = item.IsDeleted
        };
        this.ContactDetails.Add(contactDetail);
      }

      return;
    }
  }

  public class ContactDetailViewModel
  {
    [Required]
    public virtual int Id { get; set; }

    [Required]
    [Display(Name = "Contact Type")]
    public virtual int ContactTypeId { get; set; }

    public virtual string ContactText { get; set; }

    public virtual bool IsDeleted { get; set; }

    public ContactDetailViewModel()
    {
      this.ContactTypeId = 0;
      this.ContactText = string.Empty;
      this.IsDeleted = false ;
    }

    internal ContactDetailViewModel GetFirst(string key= "Phone")
    {
      var model = new ContactDetailViewModel();
      var contactType = ContactType.GetFirst(key);
      if (contactType != null)
      {
        model.ContactTypeId = contactType.Id;
        model.ContactText = contactType.Name;
        model.IsDeleted = contactType.IsDeleted;
      }

      return model;
    }
  }
}
