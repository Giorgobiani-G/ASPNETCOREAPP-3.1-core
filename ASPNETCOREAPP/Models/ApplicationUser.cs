using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPNETCOREAPP.Models
{
    public class ApplicationUserc: IdentityUser
    {
        
        [Required]
        public string Name { get; set; }
       
        [StringLength(11, MinimumLength =11, ErrorMessage ="lengt should be 11")]
        //[Display(Name = "Userid")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }
        
        
        [Required(ErrorMessage = "The Last Name field is required")]
        public string Surname { get; set; }
        [Required]
        
        public override string Email { get => base.Email; set => base.Email = value; }
        //[Required]
        //public string Mail { get; set; }
        
        public string Gender { get; set; }
        [Required]
        public DateTime DateofBirth { get; set; }

        [Required]
        public string Password { get; set; }
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        
        public string ConfirmPassword { get; set; }
    }
}
