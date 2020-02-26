using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using iTemplate.Web.Models;
using Owin;
using System;
using System.Web.Configuration;
using Microsoft.Owin.Security.OAuth;
using iTemplate.Web.Providers;

namespace iTemplate.Web.Configuration
{
  public partial class Startup
  {
    const string XmlSchemaString = "http://www.w3.org/2001/XMLSchema#string";
    public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
    public static string PublicClientId { get; private set; }

    // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
    public void ConfigureAuth(IAppBuilder app)
    {
      //Enables SignalR
      app.MapSignalR();

      // Configure the db context, user manager and role manager to use a single instance per request
      app.CreatePerOwinContext(ApplicationDbContext.Create);
      app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
      app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
      app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

      // Enable the application to use a cookie to store information for the signed in user
      // and to use a cookie to temporarily store information about a user logging in with a third party login provider
      // Configure the sign in cookie
      app.UseCookieAuthentication(new CookieAuthenticationOptions
      {
        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        LoginPath = new PathString("/Account/Login"),
        Provider = new CookieAuthenticationProvider
        {
          // Enables the application to validate the security stamp when the user logs in.
          // This is a security feature which is used when you change a password or add an external login to your account.  
          OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
              validateInterval: TimeSpan.FromMinutes(30),
              regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
        }
      });

      // Configure the application for OAuth based flow
      PublicClientId = "self";
      OAuthOptions = new OAuthAuthorizationServerOptions
      {
        TokenEndpointPath = new PathString("/Token"),
        Provider = new ApplicationOAuthProvider(PublicClientId),
        AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
        AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
        // In production mode set AllowInsecureHttp = false
        AllowInsecureHttp = true
      };

      // Enable the application to use bearer tokens to authenticate users
      app.UseOAuthBearerTokens(OAuthOptions);

      app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

      // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
      app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

      // Enables the application to remember the second login verification factor such as phone or email.
      // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
      // This is similar to the RememberMe option when you log in.
      app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

      // Uncomment the following lines to enable logging in with third party login providers

      if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings.Get("MicrosoftClientId")))
      {
        app.UseMicrosoftAccountAuthentication(
            clientId: "",
            clientSecret: "");
      }

      if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings.Get("TwitterConsumerKey")))
      {
        app.UseTwitterAuthentication(
           consumerKey: "",
           consumerSecret: "");
      }

      if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings.Get("FacebookAppId")))
      {
        app.UseFacebookAuthentication(
           appId: "",
           appSecret: "");
      }

     
      //app.UseLinkedInAuthentication(new LinkedInAuthenticationOptions
      //{
      //  ClientId = "77x9cqcd9c5w7a",
      //  ClientSecret = "bbYtQQdxRDEw34HW",
      //  Provider = new LinkedInAuthenticationProvider
      //  {
      //    OnAuthenticated = async context =>
      //    {
      //      // Retrieve the OAuth access token to store for subsequent API calls
      //      string accessToken = context.AccessToken;

      //      // Retrieve the username
      //      string linkedInUserName = context.UserName;

      //      // Retrieve the user's email address
      //      string linkedInEmailAddress = context.Email;

      //      // You can even retrieve the full JSON-serialized user
      //      var serializedUser = context.User;
      //    }
      //  }
      //});

      app.UseMicrosoftAccountAuthentication(
        clientId: "000000000000000",
        clientSecret: "000000000000000");

      //app.UseTwitterAuthentication(
      //  consumerKey: "M4EZdTfARgbX2UAgD69JO3Oz8",
      //  consumerSecret: "bPt19Xh4no7Q6i14Yydxb5HLCHgdyCtr4A1J2zOHnJa417reLq");
      //app.UseTwitterAuthentication(
      //    new TwitterAuthenticationOptions()
      //    {
      //      ConsumerKey = "M4EZdTfARgbX2UAgD69JO3Oz8",
      //      ConsumerSecret = "bPt19Xh4no7Q6i14Yydxb5HLCHgdyCtr4A1J2zOHnJa417reLq",
      //      Provider = new TwitterAuthenticationProvider()
      //      {
      //        OnAuthenticated = async context =>
      //        {
      //          context.Identity.AddClaim(new System.Security.Claims.Claim("urn:tokens:twitter:accesstoken", context.AccessToken));
      //          context.Identity.AddClaim(new System.Security.Claims.Claim("urn:tokens:twitter:accesstokensecret", context.AccessTokenSecret));
      //        }
      //      }
      //    }
      //    );

      app.UseFacebookAuthentication(
         appId: "1441785599462379",
         appSecret: "423092a502dd79441d3925bb43e553be");

      app.UseGoogleAuthentication(
          clientId: "000-000.apps.googleusercontent.com",
          clientSecret: "00000000000");


        // Microsoft : Create application
        // https://account.live.com/developers/applications
        //if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings.Get("MicrosoftClientId")))
        //{
        //    var msaccountOptions = new Microsoft.Owin.Security.MicrosoftAccount.MicrosoftAccountAuthenticationOptions()
        //    {
        //        ClientId = WebConfigurationManager.AppSettings.Get("MicrosoftClientId"),
        //        ClientSecret = WebConfigurationManager.AppSettings.Get("MicrosoftClientSecret"),
        //        Provider = new Microsoft.Owin.Security.MicrosoftAccount.MicrosoftAccountAuthenticationProvider()
        //        {
        //            OnAuthenticated = (context) =>
        //            {
        //                context.Identity.AddClaim(new System.Security.Claims.Claim("urn:microsoftaccount:access_token", context.AccessToken, XmlSchemaString, "Microsoft"));
 
        //                return Task.FromResult(0);
        //            }
        //        }
        //    };
 
        //    app.UseMicrosoftAccountAuthentication(msaccountOptions);
        //}

        // Twitter : Create a new application
        // https://dev.twitter.com/apps
        //if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings.Get("TwitterConsumerKey")))
        //{
        //  var twitterOptions = new Microsoft.Owin.Security.Twitter.TwitterAuthenticationOptions
        //  {
        //    ConsumerKey = WebConfigurationManager.AppSettings.Get("TwitterConsumerKey"),
        //    ConsumerSecret = WebConfigurationManager.AppSettings.Get("TwitterConsumerSecret"),
        //    Provider = new Microsoft.Owin.Security.Twitter.TwitterAuthenticationProvider
        //    {
        //      OnAuthenticated = (context) =>
        //      {
        //        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:twitter:access_token", context.AccessToken, XmlSchemaString, "Twitter"));
        //        return Task.FromResult(0);
        //      }
        //    }
        //  };

        //  app.UseTwitterAuthentication(twitterOptions);
        //}

        // Facebook : Create New App
        // https://developers.facebook.com/apps
        //if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings.Get("FacebookAppId")))
        //{
        //  var facebookOptions = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions
        //  {
        //    AppId = WebConfigurationManager.AppSettings.Get("FacebookAppId"),
        //    AppSecret = WebConfigurationManager.AppSettings.Get("FacebookAppSecret"),
        //    Provider = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationProvider
        //    {
        //      OnAuthenticated = (context) =>
        //      {
        //        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:facebook:access_token", context.AccessToken, XmlSchemaString, "Facebook"));
        //        foreach (var x in context.User)
        //        {
        //          var claimType = string.Format("urn:facebook:{0}", x.Key);
        //          string claimValue = x.Value.ToString();
        //          if (!context.Identity.HasClaim(claimType, claimValue))
        //            context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, XmlSchemaString, "Facebook"));

        //        }
        //        return Task.FromResult(0);
        //      }
        //    }
        //  };
        //  facebookOptions.Scope.Add("email");
        //  app.UseFacebookAuthentication(facebookOptions);
        //}

    }
  }
}