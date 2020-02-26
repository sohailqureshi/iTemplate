using System.Globalization;
using iTemplate.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using iTemplate.Web.ViewModels;

namespace iTemplate.Web.Controllers
{
  [Authorize]
  public class AccountController : Controller
  {
    private ApplicationUserManager UserManager
    {
      get
      {
        return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
      }
    }

    private ApplicationRoleManager RoleManager
    {
      get
      {
        return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
      }
    }

    public ApplicationSignInManager SignInManager
    {
      get
      {
        return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
      }
    }

    //
    // GET: /Account/Login
    [AllowAnonymous]
    public ActionResult Login(string returnUrl)
    {
      ViewBag.ReturnUrl = returnUrl;
      return View();
    }

    //
    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      // This doen't count login failures towards lockout only two factor authentication
      // To enable password failures to trigger lockout, change to shouldLockout: true
      var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
      switch (result)
      {
        case SignInStatus.Success:
          return RedirectToLocal(returnUrl);
        case SignInStatus.LockedOut:
          return View("Lockout");
        case SignInStatus.RequiresVerification:
          return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
        case SignInStatus.Failure:
        default:
          ModelState.AddModelError("", "Invalid login attempt.");
          return View(model);
      }
    }

    //
    // GET: /Account/VerifyCode
    [AllowAnonymous]
    public async Task<ActionResult> VerifyCode(string provider, string returnUrl)
    {
      // Require that the user has already logged in via username/password or external login
      if (!await SignInManager.HasBeenVerifiedAsync())
      {
        return View("Error");
      }
      var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
      if (user != null)
      {
        ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
      }
      return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
    }

    //
    // POST: /Account/VerifyCode
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
      switch (result)
      {
        case SignInStatus.Success:
          return RedirectToLocal(model.ReturnUrl);
        case SignInStatus.LockedOut:
          return View("Lockout");
        case SignInStatus.Failure:
        default:
          ModelState.AddModelError("", "Invalid code.");
          return View(model);
      }
    }

    //
    // GET: /Account/PricingPlan
    [AllowAnonymous]
    public ActionResult PricingPlan()
    {
      return View();
    }

    //
    // GET: /Account/Register
    [AllowAnonymous]
    public ActionResult Register(string pp = "TN")
    {
      switch (pp)
      {
        case "TN":
          TempData["RegisterAs"] = "Tenant";
          TempData["panel-color"] = "grey";
          break;
        case "LL":
          TempData["RegisterAs"] = "Landlord";
          TempData["panel-color"] = "green";
          break;

        case "AG":
          TempData["RegisterAs"] = "Agent";
          TempData["panel-color"] = "blue";
          break;

        case "SP":
          TempData["RegisterAs"] = "Service Provider";
          TempData["panel-color"] = "red";
          break;

        default:
          return View();
      }

      var roleName = TempData["RegisterAs"].ToString().Replace(" ", string.Empty);
      RegisterViewModel model = new RegisterViewModel() { RoleID = RoleManager.FindByName(roleName).Id };
      model.Captcha = new iTemplate.Library.Captcha.Models.Captcha();
      model.Captcha.CapImage = "data:image/png;base64," + Convert.ToBase64String(Library.Captcha.Utility.VerificationTextGenerator());
      model.Captcha.CapImageText = Convert.ToString(Session["iTemplate.Captcha"]);

      return View(model);
    }

    //
    // POST: /Account/Register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(RegisterViewModel model, bool sendEmail=true)
    {
      if (RoleManager.FindById(model.RoleID) == null) {
        return View(model);
      }

      if (ModelState.IsValid)
      {
        var user = new ApplicationUser {
          UserName = model.Email,
          Email = model.Email,
          FirstName = model.FirstName,
          LastName = model.LastName
        };

        var result = await UserManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          var roleName = RoleManager.FindById(model.RoleID).Name;
          var rolesForUser = UserManager.GetRoles(user.Id);
          if (!rolesForUser.Contains(roleName))
          {
            UserManager.AddToRole(user.Id, roleName);
          }

          var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
          var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token=code }, protocol: Request.Url.Scheme);

          if (sendEmail)
          {
            await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
          }

          ViewBag.Link = callbackUrl;
          return View("DisplayEmail");
        }
        AddErrors(result);
      }

      // If we got this far, something failed, redisplay form
      model.Captcha.CapImage = "data:image/png;base64," + Convert.ToBase64String(Library.Captcha.Utility.VerificationTextGenerator());
      model.Captcha.CapImageText = Convert.ToString(Session["iTemplate.Captcha"]);
      return View(model);
    }

    //
    // GET: /Account/ConfirmEmail
    [AllowAnonymous]
    public async Task<ActionResult> ConfirmEmail(string userId, string token)
    {
      if (userId == null || token==null)
      {
        return View("Error");
      }
      var result = await UserManager.ConfirmEmailAsync(userId, token);
      return View(result.Succeeded ? "ConfirmEmail" : "Error");
    }

    //
    // GET: /Account/ForgotPassword
    [AllowAnonymous]
    public ActionResult ForgotPassword()
    {
      return View();
    }

    //
    // POST: /Account/ForgotPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
      if (ModelState.IsValid)
      {
        var user = await UserManager.FindByNameAsync(model.Email);
        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
        {
          // Don't reveal that the user does not exist or is not confirmed
          return View("ForgotPasswordConfirmation");
        }

        var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
        ViewBag.Link = callbackUrl;
        return View("ForgotPasswordConfirmation");
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    //
    // GET: /Account/ForgotPasswordConfirmation
    [AllowAnonymous]
    public ActionResult ForgotPasswordConfirmation()
    {
      return View();
    }

    //
    // GET: /Account/ResetPassword
    [AllowAnonymous]
    public ActionResult ResetPassword(string code)
    {
      return code == null ? View("Error") : View();
    }

    //
    // POST: /Account/ResetPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      var user = await UserManager.FindByNameAsync(model.Email);
      if (user == null)
      {
        // Don't reveal that the user does not exist
        return RedirectToAction("ResetPasswordConfirmation", "Account");
      }
      var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
      if (result.Succeeded)
      {
        return RedirectToAction("ResetPasswordConfirmation", "Account");
      }
      AddErrors(result);
      return View();
    }

    //
    // GET: /Account/ResetPasswordConfirmation
    [AllowAnonymous]
    public ActionResult ResetPasswordConfirmation()
    {
      return View();
    }

    //
    // POST: /Account/ExternalLogin
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public ActionResult ExternalLogin(string provider, string returnUrl)
    {
      // Request a redirect to the external login provider
      return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
    }

    //
    // GET: /Account/SendCode
    [AllowAnonymous]
    public async Task<ActionResult> SendCode(string returnUrl)
    {
      var userId = await SignInManager.GetVerifiedUserIdAsync();
      if (userId == null)
      {
        return View("Error");
      }
      var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
      var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
      return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl });
    }

    //
    // POST: /Account/SendCode
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> SendCode(SendCodeViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View();
      }

      // Generate the token and send it
      if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
      {
        return View("Error");
      }
      return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl });
    }

    //
    // GET: /Account/ExternalLoginCallback
    [AllowAnonymous]
    public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
    {
      var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
      if (loginInfo == null)
      {
        return RedirectToAction("Login");
      }

      // Sign in the user with this external login provider if the user already has a login
      var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
      switch (result)
      {
        case SignInStatus.Success:
          return RedirectToLocal(returnUrl);
        case SignInStatus.LockedOut:
          return View("Lockout");
        case SignInStatus.RequiresVerification:
          return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
        case SignInStatus.Failure:
        default:
          // If the user does not have an account, then prompt the user to create an account
          ViewBag.ReturnUrl = returnUrl;
          ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
          return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
      }
    }

    //
    // POST: /Account/ExternalLoginConfirmation
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
    {
      if (User.Identity.IsAuthenticated)
      {
        return RedirectToAction("Index", "Manage");
      }

      if (ModelState.IsValid)
      {
        // Get the information about the user from the external login provider
        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
          return View("ExternalLoginFailure");
        }
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await UserManager.CreateAsync(user);
        if (result.Succeeded)
        {
          result = await UserManager.AddLoginAsync(user.Id, info.Login);
          if (result.Succeeded)
          {
            await StoreAuthTokenClaims(user);

            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            return RedirectToLocal(returnUrl);
          }
        }
        AddErrors(result);
      }

      ViewBag.ReturnUrl = returnUrl;
      return View(model);
    }

    //
    // POST: /Account/LogOff
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult LogOff()
    {
      AuthenticationManager.SignOut();
      return RedirectToAction("Index", "Home");
    }

    //
    // GET: /Account/ExternalLoginFailure
    [AllowAnonymous]
    public ActionResult ExternalLoginFailure()
    {
      return View();
    }

    #region Helpers
    // Used for XSRF protection when adding external logins
    private const string XsrfKey = "XsrfId";

    private IAuthenticationManager AuthenticationManager
    {
      get
      {
        return HttpContext.GetOwinContext().Authentication;
      }
    }

    private void AddErrors(IdentityResult result)
    {
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError("", error);
      }
    }

    private ActionResult RedirectToLocal(string returnUrl)
    {
      if (Url.IsLocalUrl(returnUrl))
      {
        return Redirect(returnUrl);
      }
      return RedirectToAction("Index", "Home");
    }

    internal class ChallengeResult : HttpUnauthorizedResult
    {
      public ChallengeResult(string provider, string redirectUri)
        : this(provider, redirectUri, null)
      {
      }

      public ChallengeResult(string provider, string redirectUri, string userId)
      {
        LoginProvider = provider;
        RedirectUri = redirectUri;
        UserId = userId;
      }

      public string LoginProvider { get; set; }
      public string RedirectUri { get; set; }
      public string UserId { get; set; }

      public override void ExecuteResult(ControllerContext context)
      {
        var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
        if (UserId != null)
        {
          properties.Dictionary[XsrfKey] = UserId;
        }
        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
      }
    }

    private async Task StoreAuthTokenClaims(ApplicationUser user)
    {
      // Get the claims identity
      ClaimsIdentity claimsIdentity =
          await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

      if (claimsIdentity != null)
      {
        // Retrieve the existing claims
        var currentClaims = await UserManager.GetClaimsAsync(user.Id);

        // Get the list of access token related claims from the identity
        var tokenClaims = claimsIdentity.Claims
            .Where(c => c.Type.StartsWith("urn:tokens:"));

        // Save the access token related claims
        foreach (var tokenClaim in tokenClaims)
        {
          if (!currentClaims.Contains(tokenClaim))
          {
            await UserManager.AddClaimAsync(user.Id, tokenClaim);
          }
        }
      }
    }
    protected override void Dispose(bool disposing)
    {
      if (disposing && UserManager != null)
      {
        UserManager.Dispose();
      }

      base.Dispose(disposing);
    }

    //private async Task SignInAsync(ApplicationUser user, bool isPersistent)
    //{
    //  AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
    //  var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

    //  await SetExternalProperties(identity);

    //  AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);

    //  await SaveAccessToken(user, identity);
    //}

    //private async Task SetExternalProperties(ClaimsIdentity identity)
    //{
    //  // get external claims captured in Startup.ConfigureAuth
    //  ClaimsIdentity ext = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

    //  if (ext != null)
    //  {
    //    var ignoreClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims";
    //    // add external claims to identity
    //    foreach (var c in ext.Claims)
    //    {
    //      if (!c.Type.StartsWith(ignoreClaim))
    //        if (!identity.HasClaim(c.Type, c.Value))
    //          identity.AddClaim(c);
    //    }
    //  }
    //}

    //private async Task SaveAccessToken(ApplicationUser user, ClaimsIdentity identity)
    //{
    //  var userclaims = await UserManager.GetClaimsAsync(user.Id);

    //  foreach (var at in (
    //      from claims in identity.Claims
    //      where claims.Type.EndsWith("access_token")
    //      select new Claim(claims.Type, claims.Value, claims.ValueType, claims.Issuer)))
    //  {

    //    if (!userclaims.Contains(at))
    //    {
    //      await UserManager.AddClaimAsync(user.Id, at);
    //    }
    //  }
    //}
    #endregion
  }
}