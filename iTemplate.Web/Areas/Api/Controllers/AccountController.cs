using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using iTemplate.Web.Models;
using iTemplate.Web.Providers;
using iTemplate.Web.Results;
using iTemplate.Web.Api.Models;
using iTemplate.Web.Configuration;
using iTemplate.Web.Areas.Api.Dto;
using iTemplate.Web.ViewModels;
using System.Web.Http;
using System.Linq;

namespace iTemplate.Web.Areas.Api.Controllers
{
  [Authorize]
  [RoutePrefix("api/account")]
  public class AccountController : ApiController
  {
    private const string LocalLoginProvider = "Local";

    private ApplicationUserManager UserManager
    {
      get
      {
        return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
      }
    }

    public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

    // GET api/Account/UserInfo
    //[Route("UserInfo")]
    //[OverrideAuthentication]
    //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
    public IHttpActionResult GetUserInfo()
    {
      ApplicationDbContext db = new ApplicationDbContext();
      ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
      if (externalLogin == null)
      {
        ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());
        var userIfo = new UserInfoViewModel
        {
          Email = User.Identity.GetUserName(),
          HasRegistered = externalLogin == null,
          LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null,
          Profile = new UserProfileModel
          {
            DateOfBirth = currentUser.DateOfBirth,
            FirstName = currentUser.FirstName,
            LastName = currentUser.LastName,
            GenderId = currentUser.GenderId,
            TitleId = currentUser.TitleId,
            ProfilePhoto = currentUser.ProfilePhoto
          }
        };
        return Ok(userIfo);

      }

      return BadRequest("Request Failed");
    }

    // POST api/Account/Logout
    [Route("Logout")]
    public IHttpActionResult Logout()
    {
      Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
      return Ok();
    }

    // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
    [Route("ManageInfo")]
    public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
    {
      IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

      if (user == null)
      {
        return null;
      }

      List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

      foreach (IdentityUserLogin linkedAccount in user.Logins)
      {
        logins.Add(new UserLoginInfoViewModel
        {
          LoginProvider = linkedAccount.LoginProvider,
          ProviderKey = linkedAccount.ProviderKey
        });
      }

      if (user.PasswordHash != null)
      {
        logins.Add(new UserLoginInfoViewModel
        {
          LoginProvider = LocalLoginProvider,
          ProviderKey = user.UserName,
        });
      }

      return new ManageInfoViewModel
      {
        LocalLoginProvider = LocalLoginProvider,
        Email = user.UserName,
        Logins = logins,
        ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
      };
    }

    // POST api/Account/ChangePassword
    [Route("ChangePassword")]
    public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
          model.NewPassword);

      if (!result.Succeeded)
      {
        return GetErrorResult(result);
      }

      return Ok();
    }

    // POST api/Account/SetPassword
    [Route("SetPassword")]
    public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

      if (!result.Succeeded)
      {
        return GetErrorResult(result);
      }

      return Ok();
    }

    // POST api/Account/AddExternalLogin
    [Route("AddExternalLogin")]
    public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

      AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

      if (ticket == null || ticket.Identity == null || (ticket.Properties != null
          && ticket.Properties.ExpiresUtc.HasValue
          && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
      {
        return BadRequest("External login failure.");
      }

      ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

      if (externalData == null)
      {
        return BadRequest("The external login is already associated with an account.");
      }

      IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
          new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

      if (!result.Succeeded)
      {
        return GetErrorResult(result);
      }

      return Ok();
    }

    // POST api/Account/RemoveLogin
    [Route("RemoveLogin")]
    public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      IdentityResult result;

      if (model.LoginProvider == LocalLoginProvider)
      {
        result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
      }
      else
      {
        result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
            new UserLoginInfo(model.LoginProvider, model.ProviderKey));
      }

      if (!result.Succeeded)
      {
        return GetErrorResult(result);
      }

      return Ok();
    }

    // GET api/Account/ExternalLogin
    [OverrideAuthentication]
    [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
    [AllowAnonymous]
    [Route("ExternalLogin", Name = "ExternalLogin")]
    public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
    {
      if (error != null)
      {
        return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
      }

      if (!User.Identity.IsAuthenticated)
      {
        return new ChallengeResult(provider, this);
      }

      ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

      if (externalLogin == null)
      {
        return InternalServerError();
      }

      if (externalLogin.LoginProvider != provider)
      {
        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        return new ChallengeResult(provider, this);
      }

      ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
          externalLogin.ProviderKey));

      bool hasRegistered = user != null;

      if (hasRegistered)
      {
        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager, OAuthDefaults.AuthenticationType);
        ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager, CookieAuthenticationDefaults.AuthenticationType);
        AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
        Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
      }
      else
      {
        IEnumerable<Claim> claims = externalLogin.GetClaims();
        ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
        Authentication.SignIn(identity);
      }

      return Ok();
    }

    // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
    [AllowAnonymous]
    [Route("ExternalLogins")]
    public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
    {
      IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
      List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

      string state;

      if (generateState)
      {
        const int strengthInBits = 256;
        state = RandomOAuthStateGenerator.Generate(strengthInBits);
      }
      else
      {
        state = null;
      }

      foreach (AuthenticationDescription description in descriptions)
      {
        ExternalLoginViewModel login = new ExternalLoginViewModel
        {
          Name = description.Caption,
          Url = Url.Route("ExternalLogin", new
          {
            provider = description.AuthenticationType,
            response_type = "token",
            client_id = Startup.PublicClientId,
            redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
            state = state
          }),
          State = state
        };
        logins.Add(login);
      }

      return logins;
    }

    // POST api/Account/Register
    //[HttpGet]
    //[AllowAnonymous]
    //[Route("Register")]
    //public async Task<IHttpActionResult> Register(RegisterBindingModel model)
    //{
    //  if (!ModelState.IsValid)
    //  {
    //    return BadRequest(ModelState);
    //  }

    //  var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

    //  IdentityResult result = await UserManager.CreateAsync(user, model.Password);

    //  if (!result.Succeeded)
    //  {
    //    return GetErrorResult(result);
    //  }

    //  return Ok();
    //}

    /// <summary>
    /// POST api/Account/Register
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ActionName("Register")]
    public async Task<IHttpActionResult> Register(dtoRegistration user)
    {
      if (user == null)
        return BadRequest("Invalid Request");

      if (string.IsNullOrEmpty(user.Email))
        return BadRequest("Invalid email address");

      if (string.IsNullOrEmpty(user.PostalCode))
        return BadRequest("Invalid post/zip code");

      if (string.IsNullOrEmpty(user.Telephone))
        return BadRequest("Invalid telephone number");

      var userIdentity = UserManager.FindByEmailAsync(user.Email).Result;

      if (userIdentity != null)
        return BadRequest("A user for that e-mail address already exists. Please enter a different e-mail address.");

      var newUser = new ApplicationUser()
      {
        Email = user.Email,
        UserName = user.Email,
        PhoneNumber = user.Telephone
      };

      var result = await UserManager.CreateAsync(newUser, user.Password);
      if (result.Succeeded)
      {
        var code = await UserManager.GenerateEmailConfirmationTokenAsync(newUser.Id);
        var callbackUrl = this.Url.Link("Default", new { Controller = "Account", Action = "ConfirmEmail", userId = newUser.Id, code = code });
        await UserManager.SendEmailAsync(newUser.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">Activate Account</a>");

        var identity = new ClaimsIdentity(Startup.OAuthOptions.AuthenticationType);
        identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, newUser.Id));
        AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
        var currentUtc = new Microsoft.Owin.Infrastructure.SystemClock().UtcNow;
        ticket.Properties.IssuedUtc = currentUtc;
        ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));

        dtoRegistered registered = new dtoRegistered
        {
          UserName = user.Email,
          Token = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket),
          IssuedUtc = ticket.Properties.IssuedUtc,
          ExpiresUtc = ticket.Properties.ExpiresUtc
        };

        return Ok(registered);
      }

      var errors = string.Join(",", result.Errors);
      return BadRequest(errors);
    }

    // POST api/Account/RegisterExternal
    [OverrideAuthentication]
    [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
    [Route("RegisterExternal")]
    public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var info = await Authentication.GetExternalLoginInfoAsync();
      if (info == null)
      {
        return InternalServerError();
      }

      var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

      IdentityResult result = await UserManager.CreateAsync(user);
      if (!result.Succeeded)
      {
        return GetErrorResult(result);
      }

      result = await UserManager.AddLoginAsync(user.Id, info.Login);
      if (!result.Succeeded)
      {
        return GetErrorResult(result);
      }
      return Ok();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && UserManager != null)
      {
        UserManager.Dispose();
      }

      base.Dispose(disposing);
    }

    #region Helpers

    private IAuthenticationManager Authentication
    {
      get { return Request.GetOwinContext().Authentication; }
    }

    private IHttpActionResult GetErrorResult(IdentityResult result)
    {
      if (result == null)
      {
        return InternalServerError();
      }

      if (!result.Succeeded)
      {
        if (result.Errors != null)
        {
          foreach (string error in result.Errors)
          {
            ModelState.AddModelError("", error);
          }
        }

        if (ModelState.IsValid)
        {
          // No ModelState errors are available to send, so just return an empty BadRequest.
          return BadRequest();
        }

        return BadRequest(ModelState);
      }

      return null;
    }

    private class ExternalLoginData
    {
      public string LoginProvider { get; set; }
      public string ProviderKey { get; set; }
      public string UserName { get; set; }

      public IList<Claim> GetClaims()
      {
        IList<Claim> claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

        if (UserName != null)
        {
          claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
        }

        return claims;
      }

      public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
      {
        if (identity == null)
        {
          return null;
        }

        Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

        if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)|| String.IsNullOrEmpty(providerKeyClaim.Value))
        {
          return null;
        }

        if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
        {
          return null;
        }

        return new ExternalLoginData
        {
          LoginProvider = providerKeyClaim.Issuer,
          ProviderKey = providerKeyClaim.Value,
          UserName = identity.FindFirstValue(ClaimTypes.Name)
        };
      }
    }

    private static class RandomOAuthStateGenerator
    {
      private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

      public static string Generate(int strengthInBits)
      {
        const int bitsPerByte = 8;

        if (strengthInBits % bitsPerByte != 0)
        {
          throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
        }

        int strengthInBytes = strengthInBits / bitsPerByte;

        byte[] data = new byte[strengthInBytes];
        _random.GetBytes(data);
        return HttpServerUtility.UrlTokenEncode(data);
      }
    }

    private string GetToken(ApplicationUser userIdentity)
    {
      if (userIdentity != null)
      {
        var identity = new ClaimsIdentity(Startup.OAuthOptions.AuthenticationType);
        identity.AddClaim(new Claim(ClaimTypes.Name, userIdentity.Email));
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userIdentity.Id));
        AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
        var currentUtc = new Microsoft.Owin.Infrastructure.SystemClock().UtcNow;
        ticket.Properties.IssuedUtc = currentUtc;
        ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));
        string AccessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);

        //ClaimsIdentity identity = new ClaimsIdentity(Startup.OAuthBearerOptions.AuthenticationType);
        //identity.AddClaim(new Claim(ClaimTypes.Name, userIdentity.UserName));
        //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userIdentity.Id));
        //AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
        //DateTime currentUtc = DateTime.UtcNow;
        //ticket.Properties.IssuedUtc = currentUtc;
        //ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));
        //string AccessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

        return AccessToken;
      }

      return "no token";
    }

    #endregion
  }
}
