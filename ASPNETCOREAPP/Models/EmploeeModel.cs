using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCOREAPP.Models
{
    public class EmploeeModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(11)]
        public string Empid { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateofBirth { get; set; }
        [Required(ErrorMessage ="The Email is required")]
        public string Email { get; set; }

        public string Posti { get; set; }

        public string Statusi { get; set; }
        [DataType(DataType.Date)]
        //[Required(AllowEmptyStrings =false)]
        
        public DateTime? DateofFire { get; set; }
    }
}
