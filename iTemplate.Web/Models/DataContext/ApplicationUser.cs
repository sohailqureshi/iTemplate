using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Security.Principal;
using iTemplate.Web.Data.Site;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Web;

namespace iTemplate.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
  public class ApplicationUser : IdentityUser
  {
    #region Custom fields

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "Title")]
    public virtual int TitleId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [Display(Name = "First Name")]
    public virtual string FirstName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "Last Name")]
    public virtual string LastName { get; set; }

    /// <summary>
    /// Gets users full name.
    /// </summary>
    [NotMapped]
    [Display(Name = "Full Name")]
    public virtual string FullName
    {
      get { return FirstName + " " + LastName; }
    }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "Profile Photo")]
    public virtual ImageInfo ProfilePhoto { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "D.O.B")]
    public virtual DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [Display(Name = "Gender")]
    public virtual int GenderId { get; set; }

    /// <summary>
    /// Gets or sets the users last login date and time.
    /// </summary>
    [Display(Name = "Last Login")]
    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm}")]
    public virtual DateTime? DateLoggedIn { get; set; }

    /// <summary>
    /// Gets or sets the users registration date and time.
    /// </summary>
    [Required]
    [Display(Name = "Registered On")]
    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm}")]
    public virtual DateTime? DateRegistered { get; set; }

    /// <summary>
    /// True if the email is confirmed, default is false
    /// </summary>
    [Display(Name = "Email Confirmed")]
    public override bool EmailConfirmed { get; set; }

    /// <summary>
    /// True if the phone number is confirmed, default is false
    /// </summary>
    [Display(Name = "Phone Confirmed")]
    public override bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    /// Is lockout enabled for this user
    /// </summary>
    [Display(Name = "Lockout Enabled")]
    public override bool LockoutEnabled { get; set; }

    /// <summary>
    /// Gets a list of all the roles a user has access to
    /// </summary>
    public ICollection<ApplicationRole> UserRoles { get; set; }

    public bool IsAuthenticated
    {
      get
      { return HttpContext.Current.User.Identity.IsAuthenticated; }
    }

    #endregion

    public ApplicationUser()
    {
      this.DateRegistered = DateTime.Now;
      this.GenderId = 0;
      this.ProfilePhoto = new ImageInfo();
      this.UserRoles = new HashSet<ApplicationRole>();

      GetDatabaseUserRolesPermissions();
    }

    private void GetDatabaseUserRolesPermissions()
    {
      using (ApplicationDbContext db = new ApplicationDbContext())
      {
        ApplicationUser user = db.Users.Where(u => u.UserName == this.UserName).FirstOrDefault();
        if (user != null)
        {
          this.Id = user.Id;
          foreach (var role in user.UserRoles)
          {
            var userRole = new ApplicationRole { Id = user.Id, Name = role.Name };
            foreach (var permission in userRole.Permissions)
            {
              userRole.Permissions.Add(new ApplicationPermission { Id = permission.Id, Name = permission.Name, Description = permission.Description });
            }
            this.UserRoles.Add(userRole);
          }
        }
      }
    }

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    {
      // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
      var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
      // Add custom user claims here
      return userIdentity;
    }

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
    {
      // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
      var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
      // Add custom user claims here
      return userIdentity;
    }
  }

  public static class GenericPrincipalExtensions
  {
    public static bool IsAuthenticated(this IPrincipal user)
    {
      return user.Identity.IsAuthenticated;
    }

    public static bool IsAdministrator(this IPrincipal user)
    {
      ApplicationDbContext context = new ApplicationDbContext();
      return IsInRole(user, "Administrator");
    }

    public static bool IsInRole(this IPrincipal user, string roleName)
    {
      ApplicationDbContext db = new ApplicationDbContext();
      var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
      var RoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));

      if (user.Identity.IsAuthenticated)
      {
        {
          try { return (UserManager.IsInRole(user.Identity.GetUserId(), roleName)); }
          catch (Exception e) { return false; }
        }
      }
      return false;
    }
  }
}