using System.ComponentModel.DataAnnotations;

namespace ASPNETCOREAPP.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
