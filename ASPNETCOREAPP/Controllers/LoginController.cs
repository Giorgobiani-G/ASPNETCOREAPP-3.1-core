using ASPNETCOREAPP.Models;
using ASPNETCOREAPP.Services;
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
        
        DatabaseContext db;

        
        public IActionResult Mail ()
        {
            //string to = email.To;
            //string subject = email.Subject;
            //string body = email.Body;
            //MailMessage mm = new MailMessage();
            //mm.To.Add(to);
            //mm.Body = "Login";
            //mm.Subject = "congrats";
            //mm.From = new MailAddress("mailforbusiness86@gmail.com");
            //mm.IsBodyHtml = false;
            //SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            //smtp.Port = 587;
            //smtp.UseDefaultCredentials = true;
            //smtp.EnableSsl = true;
            //smtp.Credentials = new System.Net.NetworkCredential("mailforbusiness86@gmail.com", "giorgobiani1986");
            //smtp.Send(mm);
            ViewBag.message = "mail has been sent to " +  " succesfully";
            
            return View();

        }

        private readonly SignInManager<ApplicationUserc> signInManager;
        public LoginController(SignInManager<ApplicationUserc> signInManager, DatabaseContext ddb)
        {
            this.signInManager = signInManager;
            db = ddb;
           
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
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
                                where users.Email == model.Email
                                select users).FirstOrDefault();

                    if (user == null)
                    {
                        throw new Exception("invalid user or password");
                    }
                    
                    var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        //string to = user.Email;
                       
                        //MailMessage mm = new MailMessage();
                        //mm.To.Add(to);
                        //mm.Body = "Login";
                        //mm.Subject = "congrats";
                        //mm.From = new MailAddress("mailforbusiness86@gmail.com");
                        //mm.IsBodyHtml = false;
                        //SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                        //smtp.Port = 587;
                        //smtp.UseDefaultCredentials = false;
                        //smtp.EnableSsl = true;
                        //smtp.Credentials = new System.Net.NetworkCredential("mailforbusiness86@gmail.com", "giorgobiani1986");
                        //smtp.Send(mm);
                        //ViewBag.message = "mail has been sent to " + mm.To + " succesfully";



                        return RedirectToAction("Index", "Home");
                    }

                }
                catch (InvalidOperationException  ex)
                {
                    // the user is not exist
                }
            

               




                ModelState.AddModelError(string.Empty, "Invalid Email Or Password");
            }

            return View(model);
        }
    }
}