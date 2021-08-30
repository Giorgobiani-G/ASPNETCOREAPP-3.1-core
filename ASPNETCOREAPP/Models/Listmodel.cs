using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCOREAPP.Models
{
    public class Listmodel
    {

        //public List<string> lst = new List<string>()
        //{
        //     "sisharp",
        //         "java",
        //         "pyton"

        //};
        [Key]
        public int ImageId { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }

        public string Photopath { get; set; }



    }
}
