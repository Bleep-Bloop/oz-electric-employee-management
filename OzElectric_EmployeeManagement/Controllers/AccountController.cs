using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OzElectric_EmployeeManagement.Models;
using System.Collections.Generic;
using System.IO;

//added for logging
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Filter;
using log4net.Core;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using log4net.Config;
using log4net.Plugin;
using log4net.DateFormatter;
using log4net.ObjectRenderer;
using log4net.Util;


namespace OzElectric_EmployeeManagement.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
                                                                //aspNetUser?
        public ILog logger = log4net.LogManager.GetLogger(typeof(AccountController));
        //public ILog employeeLogger = log4net.LogManager.GetLogger("aosud");
        public ILog employeeLogger = log4net.LogManager.GetLogger("loggerNames");
        

        //Dynamic user log creation
        public ILog log = LogManager.GetLogger(typeof(AccountController));



        //Util functions for dynamically creating appenders

        // Set the level for a named logger
        public static void SetLevel(string loggerName, string levelName)
        {
            ILog log = log4net.LogManager.GetLogger(loggerName);
            Logger l = (Logger)log.Logger;

            l.Level = l.Hierarchy.LevelMap[levelName];
        }

        // Add an appender to a logger
        public static void AddAppender(string loggerName, IAppender appender)
        {

            ILog log = LogManager.GetLogger(loggerName);
            Logger l = (Logger)log.Logger;

            l.AddAppender(appender);

        }

        public static void AddAppender2(ILog log, IAppender appender)
        {
            // ILog log = LogManager.GetLogger(loggerName);
            Logger l = (Logger)log.Logger;

            l.AddAppender(appender);
        }





        // Find a named appender already attached to a logger //GOTTA FIX //MIGHT NOT NEED
        public static log4net.Appender.IAppender FindAppender(string
        appenderName)
        {
            foreach (log4net.Appender.IAppender appender in
          log4net.LogManager.GetRepository().GetAppenders())
            {
                if (appender.Name == appenderName)
                {
                    return appender;
                }
            }
            return null;
        }

        // Create a new file appender
        public static IAppender CreateFileAppender(string name, string fileName)
        {
            FileAppender appender = new FileAppender();
            appender.Name = name;
            appender.File = fileName;
            appender.AppendToFile = true;

            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = "%d [%t] %-5p %c [%x] - %m%n";
            layout.ActivateOptions();

            appender.Layout = layout;
            appender.ActivateOptions();

            return appender;
        }




        //another create version

        public static log4net.Appender.IAppender createAnotherThingy()
        {

            string folderPath = "C:\\OzzElectricLogs";
            string instanceName = "instanceNameGod";

            //Layout Pattern
            PatternLayout layout = new PatternLayout("% date{ MMM / dd / yyyy HH:mm: ss,fff}[%thread] %-5level %logger %ndc – %message%newline");

            //Level Filter
            LevelMatchFilter filter = new LevelMatchFilter();
            filter.LevelToMatch = Level.All;
            filter.ActivateOptions();

            RollingFileAppender appender = new RollingFileAppender();
            appender.File = string.Format("{0}\\{1}", folderPath, "common.log");
            appender.ImmediateFlush = true;
            appender.AppendToFile = true;
            appender.RollingStyle = RollingFileAppender.RollingMode.Date;
            appender.DatePattern = "-yyyy-MM-dd";
            appender.LockingModel = new FileAppender.MinimalLock();
            appender.Name = string.Format("{0}Appender", instanceName);
            appender.AddFilter(filter);
            appender.ActivateOptions();

            //Populate the log instance
            string repositoryName = string.Format("{0}Repository", instanceName);
            ILoggerRepository repository = LoggerManager.CreateRepository(repositoryName);
            string loggerName = string.Format("{0}Logger", instanceName);
            BasicConfigurator.Configure(repository, appender);

            ILog loggering = LogManager.GetLogger(repositoryName, loggerName);
            loggering.Debug("test print yo");


            return appender;

        } 




        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

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


            // Require the user to have a confirmed email before they can log on.
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {

                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account-Resend");

                    ModelState.AddModelError(string.Empty,
                              "You must have a confirmed email to log in. Resending ");

                    //ViewBag.errorMessage = "You must have a confirmed email to log on.";
                    return View();
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
          


        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
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

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
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
        // GET: /Account/Register
        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            //Query all roles that have been made
            List<SelectListItem> allRoles = (new ApplicationDbContext()).Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Roles = allRoles;

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email, firstName = model.firstName, lastName = model.lastName};
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //  Comment the following line to prevent log in until the user is confirmed.
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    

                    await UserManager.AddToRoleAsync(user.Id, model.Role.ToString());

                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account");

                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");


                    #region
                    /*
                    string folderPath = "C:\\OzzElectricLogs";
                    string instanceName = "testgg";

                    //Layout Pattern
                    PatternLayout layout = new PatternLayout("% date{ MMM / dd / yyyy HH:mm: ss,fff}[%thread] %-5level %logger %ndc – %message%newline");

                    //Level Filter
                    LevelMatchFilter filter = new LevelMatchFilter();
                    filter.LevelToMatch = Level.All;
                    filter.ActivateOptions();

                    RollingFileAppender appender = new RollingFileAppender();
                    appender.File = string.Format("{0}\\{1}", folderPath, "common.txt"); 
                    appender.ImmediateFlush = true; 
                    appender.AppendToFile = true;
                    appender.RollingStyle = RollingFileAppender.RollingMode.Date;
                    appender.DatePattern = "-yyyy-MM-dd";
                    appender.LockingModel = new FileAppender.MinimalLock();
                    appender.Name = string.Format("{0}Appender", instanceName);
                    appender.AddFilter(filter);
                    appender.ActivateOptions();

                    //Populate the log instance
                    string repositoryName = string.Format("{0}Repository", instanceName);
                    ILoggerRepository repository = LoggerManager.CreateRepository(repositoryName);
                    string loggerName = string.Format("{0}Logger", instanceName);
                    BasicConfigurator.Configure(repository, appender);

                    
                    ILog newLoggerName = LogManager.GetLogger(repositoryName, loggerName);
                    newLoggerName.Info("Test print");
                    logger.Debug("Writing here*/
                    #endregion

                    

                    //  CreateFileAppender("AppenderName", "C:\\OzzElectricLogs\\test.
                    
                    //Create log file for new users using their first and last name
                    BasicConfigurator.Configure();
                    SetLevel("Log4net.MainForm", "ALL");
                    AddAppender2(log, CreateFileAppender("appenderName", "C:\\OzzElectricLogs\\" + model.firstName + model.lastName + "ActivityLog.txt"));
                    log.Info(User.Identity.Name + " Created " + model.firstName + " " + model.lastName);



                return RedirectToAction("Index", "Home"); 
                }
                else
                {
                    //If user is not able to be created, requery roles for view
                    List<SelectListItem> allRoles = (new ApplicationDbContext()).Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                        new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

                    ViewBag.Roles = allRoles;
                }
                AddErrors(result);

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }


        //Resending Email Confirmation
        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userID, subject,
               "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            return callbackUrl;
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

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                 string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                 var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);


                string templateFileEmail;


                string usersBrowser = Request.Browser.Type;
                string usersOS = Request.UserAgent;

                //Get and sort users computer/browser information
                //Probably switch case this
                if (usersBrowser.Contains("Chrome"))
                {
                    usersBrowser = "Google Chrome";
                }
                else if (usersBrowser.Contains("Edge"))
                {
                    usersBrowser = "Internet Edge";
                }
                else if (usersBrowser.Contains("Firefox"))
                {
                    usersBrowser = "Firefox";
                }
                else
                {
                    usersBrowser = Request.Browser.Type;
                }


                //Probably switch case this
                if (Request.UserAgent.IndexOf("Windows NT 5.1") > 0)
                {
                    usersOS = "Windows XP";
                }
                else if (Request.UserAgent.IndexOf("Windows NT 6.0") > 0)
                {
                    usersOS = "Windows Vista";
                }
                else if (Request.UserAgent.IndexOf("Windows NT 6.1") > 0)
                {
                    usersOS = "Windows 7";
                }
                else if (Request.UserAgent.IndexOf("Windows NT 6.2") > 0)
                {
                    usersOS = "Windows 8";
                }
                else if (Request.UserAgent.IndexOf("Windows NT 6.3") > 0)
                {
                    usersOS = "Windows 8.1";
                }
                else if (Request.UserAgent.IndexOf("Windows NT 10.0") > 0)
                {
                    usersOS = "Windows 10";
                }
                else
                {
                    usersOS = Request.UserAgent;
                }


                string templatePath = Environment.ExpandEnvironmentVariables(@"%HOME%\site\wwwroot\Views\Account\ForgotPasswordEmailTemplate.html");
                using (StreamReader sr = new StreamReader(templatePath))
                {

                    templateFileEmail = sr.ReadToEnd();

                    //Replace lines with variables
                    templateFileEmail = templateFileEmail.Replace("IncomingUserID", user.firstName );
                    templateFileEmail = templateFileEmail.Replace("IncomingPasswordResetLink", callbackUrl);
                    templateFileEmail = templateFileEmail.Replace("IncomingBrowserName", usersBrowser);
                    templateFileEmail = templateFileEmail.Replace("IncomingOperatingSystem", usersOS);

                    sr.Close();
                }

                await UserManager.SendEmailAsync(user.Id, "Reset Password", templateFileEmail);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
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
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
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
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
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
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
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
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
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
        #endregion
    }
}