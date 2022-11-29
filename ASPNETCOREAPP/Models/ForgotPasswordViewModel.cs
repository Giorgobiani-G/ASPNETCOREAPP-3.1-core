using System.ComponentModel.DataAnnotations;

namespace ASPNETCOREAPP.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
