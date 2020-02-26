using System.Web.Mvc;
using iTemplate.Web.Models;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using iTemplate.Web.ViewModels;
using iTemplate.Web.SignalR;

namespace iTemplate.Web.Controllers
{
  //[RequireHttps]
  public class ProfileController : Controller
  {
    private ApplicationDbContext db = new ApplicationDbContext();
    public ProfileController()
    {
      db = new ApplicationDbContext();
    }

    public ProfileController(ApplicationUserManager userManager)
    {
      UserManager = userManager;
    }

    private ApplicationUserManager _userManager;
    public ApplicationUserManager UserManager
    {
      get
      {
        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
      }
      private set
      {
        _userManager = value;
      }
    }

    [Authorize]
    public ActionResult Edit()
    {
      ApplicationDbContext db = new ApplicationDbContext();
      ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());

      var userId = User.Identity.GetUserId();
      var userProfile = new UserProfileModel();

      var user = UserManager.FindById(userId);
      if (user != null)
      {
        userProfile = new UserProfileModel()
        {
          DateOfBirth = user.DateOfBirth,
          FirstName = user.FirstName,
          LastName = user.LastName,
          GenderId = user.GenderId,
          ProfilePhoto = user.ProfilePhoto,
          TitleId = user.TitleId,
          UserId = user.Id
        };
      }
      return View(userProfile);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(UserProfileModel profile, HttpPostedFileBase fileUpload)
    {
      if (ModelState.IsValid)
      {
        var user = UserManager.FindById(profile.UserId);
        user.FirstName = profile.FirstName;
        user.LastName = profile.LastName;
        user.GenderId = profile.GenderId;
        if (fileUpload != null) {
          user.ProfilePhoto = new ImageInfo(fileUpload);
          profile.ProfilePhoto = user.ProfilePhoto;
        }

        UserManager.Update(user);
        MessageHandler.Instance.BroadcastToCurrentUser("Profile updated!");
      }

      return View(profile);
    }
  }
}
