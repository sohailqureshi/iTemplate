using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace iTemplate.Library.Captcha.Models
{
  public class Captcha
  {
    public string CapImage { get; set; }

    [Display(Name = "Verification Code")]
    [Required(ErrorMessage = "Verification code is required.")]
    [Compare("CapImageText",      ErrorMessage = "Captcha code Invalid")]
    public string CaptchaCodeText { get; set; }

    public string CapImageText { get; set; }
  }
}