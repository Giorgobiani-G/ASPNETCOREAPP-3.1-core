using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNETCOREAPP.Models
{
    public class Listmodel
    {
        [Key]
        public int ImageId { get; set; }

        public string Name { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }

        public string Photopath { get; set; }
    }
}
