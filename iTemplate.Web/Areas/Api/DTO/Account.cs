using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using iTemplate.Web.Models;

namespace iTemplate.Web.Areas.Api.Dto
{
  public class dtoRegisterExternal
  {
    [Required]
    public string Email { get; set; }
  }

  public class dtoAuthenticate
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
  }

  public class dtoRegistered: dtoToken
  {
    public string UserName { get; set; }
  }

  public class dtoRegistration
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string PostalCode { get; set; }

    public byte Avatar { get; set; }

    [Required]
    public string Telephone { get; set; }
  }

  public class dtoUser : dtoToken
  {
    [JsonProperty(PropertyName = "FirstName")]
    public string FirstName { get; set; }

    [JsonProperty(PropertyName = "LastName")]
    public string LastName { get; set; }

    [JsonProperty(PropertyName = "HomeAddress")]
    public Address HomeAddress { get; set; }

    [JsonProperty(PropertyName = "Gender")]
    public string Gender { get; set; }

    [JsonProperty(PropertyName = "Photo")]
    public byte[] Photo { get; set; }

    [JsonProperty(PropertyName = "LastLoggedIn")]
    public DateTime? LastLoggedIn { get; set; }

    [JsonProperty(PropertyName = "RegisteredOn")]
    public DateTime? RegisteredOn { get; set; }
  }

  public class SetPassword
  {
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm new password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
  }

  public class ChangePassword
  {
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Current password")]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm new password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
  }
}