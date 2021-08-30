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
        private readonly UserManager<ApplicationUserc> userManager;
        private readonly SignInManager<ApplicationUserc> signInManager;



        public RegisterController(UserManager<ApplicationUserc> userManager,
            SignInManager<ApplicationUserc> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;

        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUserc model)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUserc
                {
                    Name = model.Name.ToString(),
                    Surname = model.Surname,
                    UserName = model.UserName,
                    DateofBirth = model.DateofBirth,
                    Gender = model.Gender,
                    Email = model.Email,
                    Password = model.Password

                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim("Name", user.Name));
                    await signInManager.SignInAsync(user, isPersistent: false);

                    string to = user.Email;

                    MailMessage mm = new MailMessage();
                    mm.To.Add(to);
                    mm.Body = "Login";
                    mm.Subject = "congrats";
                    mm.From = new MailAddress("mailforbusiness86@gmail.com");
                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential("mailforbusiness86@gmail.com", "giorgobiani1986");
                    smtp.Send(mm);
                    ViewBag.message = "mail has been sent to " + mm.To + " succesfully";



                    return RedirectToAction("Mail", "Login");

                    //return RedirectToAction("index", "EmploeeModels");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
    
    }
}