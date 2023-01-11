using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNETCOREAPP.Models
{
    public class EmploeeModel
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        [Required]
        [MaxLength(11)]
        public string Empid { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateofBirth { get; set; }
        [Required(ErrorMessage = "The Email is required")]
        public string Email { get; set; }

        public string Posti { get; set; }

        public string Statusi { get; set; }
        [DataType(DataType.Date)]
        //[Required(AllowEmptyStrings =false)]

        public DateTime? DateofFire { get; set; }
    }
}
