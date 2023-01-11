using System;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using ASPNETCOREAPP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETCOREAPP.Controllers
{
    public class RegisterController : Controller

    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUser model)
        {
            if (model.Password!=model.ConfirmPassword)
            {
                throw new Exception("Confirm password doesn't match the password!");
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    UserName = model.UserName,
                    DateofBirth = model.DateofBirth,
                    Gender = model.Gender,
                    Email = model.Email,
                    Password = model.Password
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    string actionLink = Url.Action("ConfirmEmail", "Register", new { id = user.Id, token = token }, Request.Scheme);
                    string to = user.Email;

                    var mm = new MailMessage();
                    mm.To.Add(to);
                    mm.Body = actionLink;
                    mm.Subject = "congrats";
                    mm.From = new MailAddress("mailforbusiness86@gmail.com");
                    mm.IsBodyHtml = false;
                    var smtp = new SmtpClient("smtp.gmail.com");
                    
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential("mailforbusiness86@gmail.com", "Safrangeti@1986");
                 
                    smtp.Send(mm);  

                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                        return RedirectToAction("ListUsers", "Administration");

                    return RedirectToAction("Mail", "Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


        public async Task<IActionResult> ConfirmEmail(string id, string token)
        {
            if (id == null|| token==null)
            {
                return RedirectToAction("Home", "Index");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user==null)
            {
                ViewBag.ErrorMessage = $"Incoming User ID {id} is invalid";
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ErrorMessage = "Email Can't be Confirmed";

            return View("NotFound");
        }
    }
}