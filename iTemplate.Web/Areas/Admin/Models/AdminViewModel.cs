using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace iTemplate.Web.Areas.Admin.ViewModels
{
  public class RoleViewModel
  {
    public string Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Role Name")]
    public string Name { get; set; }

    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Role Description")]
    public string Description { get; set; }

    [Required]
    [Display(Name = "System Role")]
    public bool IsSystem { get; set; }
  }

  public class EditUserViewModel
  {
    public string Id { get; set; }

    [Required]
    [Display(Name = "First Name")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    public string FirstName { get; set; }

    [Required]
    [Display(Name = "Last Name")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
    public string LastName { get; set; }

    [EmailAddress]
    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Email")]
    public string Email { get; set; }

    public EditUserViewModel()
    {
      this.Id = string.Empty;
      this.FirstName = string.Empty;
      this.LastName = string.Empty;
      this.Email = string.Empty;
    }
  }
}