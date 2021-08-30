using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNETCOREAPP.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace ASPNETCOREAPP.Controllers
{
    public class HomeController : Controller

    {
        DatabaseContext database;

        private readonly IWebHostEnvironment _hostingEnvironment;
        public HomeController(IWebHostEnvironment hostingEnvironment,DatabaseContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            database = context;
        }
        public IActionResult Index()
        {
           
            return View();
        }
        [HttpPost]
        public IActionResult Index( Listmodel model)
        {


            
            
            if (ModelState.IsValid)
            {
                Listmodel lst = new Listmodel();
                string photoname = null;
                if (model.Photo!=null)
                {
                    
                    string folderpath = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                    photoname = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string photopath = Path.Combine(folderpath, photoname);
                    lst.Photopath = photoname;
                    model.Photo.CopyTo(new FileStream(photopath, FileMode.Create));
                    lst.Name = model.Name;
                    database.Listmodels.Add(lst);
                    database.SaveChanges();

                }

                
                
            }


            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete( Listmodel id )
        {
            
            var model = await database.Listmodels.FindAsync(id.ImageId);
            
            database.Listmodels.Remove(model);
            await database.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
