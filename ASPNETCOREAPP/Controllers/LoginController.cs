using ASPNETCOREAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ASPNETCOREAPP.Controllers
{
    public class LoginController : Controller
    {
        private readonly DatabaseContext db;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public IActionResult Mail ()
        {

            ViewBag.ErrorTitle = "Registration successful";
            ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                    "email, by clicking on the confirmation link we have emailed you";

            return View();

        }

        
        public LoginController(SignInManager<ApplicationUser> signInManager, DatabaseContext ddb,UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            db = ddb;
            this.userManager = userManager;
           
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {   
                try
                {                  
                    var user = (from users in db.Users
                                where users.Email == model.Email&& users.Password == model.Password
                                select users).FirstOrDefault();

                    if (user == null)
                    {
                        ModelState.AddModelError("", "Invalid Mail Or Password");
                        return View(model) ;
                    }

                    if (user!=null&&!user.EmailConfirmed&&(await userManager.CheckPasswordAsync(user,model.Password)))
                    {
                        ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                        return View(model);
                    }
                    
                    var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {             
                        return RedirectToAction("Index", "Home");
                    }

                }
                catch (InvalidOperationException  ex)
                {
                   
                }           
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var user = await userManager.FindByEmailAsync(model.Email);
                
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action("ResetPassword", "Login",
                            new { email = model.Email, token = token }, Request.Scheme);

                    string to = user.Email;

                    MailMessage mm = new MailMessage();
                    mm.To.Add(to);
                    mm.Body = passwordResetLink;
                    mm.Subject = "Reset Password";
                    mm.From = new MailAddress("mailforbusiness86@gmail.com");
                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");

                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential("mailforbusiness86@gmail.com", "Safrangeti@1986");

                    smtp.Send(mm);

                    // Send the user to Forgot Password Confirmation view
                    return View("ForgotPasswordConfirmation");
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            // If password reset token or email is null, most likely the
            // user tried to tamper the password reset link
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // reset the user password
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist

                return View("ResetPasswordConfirmation");
            }
            // Display validation errors if model state is not valid
            return View(model);
        }
    }
}
