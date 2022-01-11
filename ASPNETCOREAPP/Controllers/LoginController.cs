using ASPNETCOREAPP.Models;
using ASPNETCOREAPP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ASPNETCOREAPP.Controllers
{
    public class LoginController : Controller
    {
        private readonly DatabaseContext db;
        private readonly SignInManager<ApplicationUserc> signInManager;
        private readonly UserManager<ApplicationUserc> userManager;

        public IActionResult Mail ()
        {

            ViewBag.ErrorTitle = "Registration successful";
            ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                    "email, by clicking on the confirmation link we have emailed you";

            return View();

        }

        
        public LoginController(SignInManager<ApplicationUserc> signInManager, DatabaseContext ddb,UserManager<ApplicationUserc> userManager)
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
                    // the user is not exist
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
    }
}
