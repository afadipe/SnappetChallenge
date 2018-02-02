using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using VATMVCAPPFramework.Models;

using VATMVCAPPFramework.Data;
using VATMVCAPPFramework.Data.IdentityService;
using ApplicationUser = VATMVCAPPFramework.Data.IdentityModel.ApplicationUser;

namespace VATMVCAPPFramework
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(APPContext.Create);
            app.CreatePerOwinContext<VATMVCAPPFramework.Data.IdentityService.ApplicationUserManager>(CreateApplicationUserManager);
            app.CreatePerOwinContext<VATMVCAPPFramework.Data.IdentityService.ApplicationSignInManager>(CreateApplicationSignInManager);
            app.CreatePerOwinContext<VATMVCAPPFramework.Data.IdentityService.ApplicationRoleManager>(CreateApplicationRoleManager);

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
                    //OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                    //    validateInterval: TimeSpan.FromMinutes(30),
                    //    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))

                    OnValidateIdentity =
                          SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, Int64>(
                                  TimeSpan.FromMinutes(30),
                                  regenerateIdentityCallback: (manager, user) =>
                                      user.GenerateUserIdentityAsync(manager),
                                     getUserIdCallback: id => id.GetUserId<int>())
                },
                ExpireTimeSpan = TimeSpan.FromMinutes(20)
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

        public static VATMVCAPPFramework.Data.IdentityService.ApplicationUserManager CreateApplicationUserManager(
      IdentityFactoryOptions<VATMVCAPPFramework.Data.IdentityService.ApplicationUserManager> options,
      IOwinContext context)
        {
            var appUserManager = new VATMVCAPPFramework.Data.IdentityService.ApplicationUserManager(new ApplicationUserStore(new APPContext()));
            appUserManager.UserValidator = new UserValidator<VATMVCAPPFramework.Data.IdentityModel.ApplicationUser, Int64>(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                //Since a parent can have more than one child and we want to use parentemail and phonenumber for student email & phone cant be unique any more
                // RequireUniqueEmail = true
                RequireUniqueEmail = false
            };

            // Configure validation logic for passwords
            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 4,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var dataProtectionProvider = DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser,Int64>(dataProtectionProvider.Create("VATMVC"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(16)
                };
            }

            return appUserManager;
        }


        public static VATMVCAPPFramework.Data.IdentityService.ApplicationSignInManager CreateApplicationSignInManager(
            IdentityFactoryOptions<VATMVCAPPFramework.Data.IdentityService.ApplicationSignInManager> options,
            IOwinContext context)
        {
            return
                new VATMVCAPPFramework.Data.IdentityService.ApplicationSignInManager(context.GetUserManager<VATMVCAPPFramework.Data.IdentityService.ApplicationUserManager>(),
                    context.Authentication);
        }


        // See http://bitoftech.net/2015/03/11/asp-net-identity-2-1-roles-based-authorization-authentication-asp-net-web-api/
        public static ApplicationRoleManager CreateApplicationRoleManager(
            IdentityFactoryOptions<ApplicationRoleManager> options,
            IOwinContext context)
        {
            var appRoleManager =
                new ApplicationRoleManager(
                    new ApplicationRoleStore(
                        context.Get<APPContext>()));

            return appRoleManager;
        }
    }
}