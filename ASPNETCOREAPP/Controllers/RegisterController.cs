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
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    string ActionLink = Url.Action("ConfirmEmail", "Register", new { id = user.Id, token = token }, Request.Scheme);
                    string to = user.Email;

                    MailMessage mm = new MailMessage();
                    mm.To.Add(to);
                    mm.Body = ActionLink;
                    mm.Subject = "congrats";
                    mm.From = new MailAddress("mailforbusiness86@gmail.com");
                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential("mailforbusiness86@gmail.com", "Safrangeti@1986");
                 
                    smtp.Send(mm);  

                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                     
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
            var user = await userManager.FindByIdAsync(id);
            if (user==null)
            {
                ViewBag.ErrorMessage = $"Incoming User ID {id} is invalid";
                return View("NotFound");
            }
            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {

                return View();
            }
            ViewBag.ErrorMessage = "Email Can't be Confirmed";
            return View("NotFound");

            
          
        }


    }
}