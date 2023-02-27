using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNETCOREAPP.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace ASPNETCOREAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _dataBase;

        private readonly IWebHostEnvironment _hostingEnvironment;
        public HomeController(IWebHostEnvironment hostingEnvironment, DatabaseContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _dataBase = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Listmodel model)
        {
            if (ModelState.IsValid)
            {
                var list = new Listmodel();

                if (model.Photo != null)
                {
                    var folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                    var photoName = Guid.NewGuid() + "_" + model.Photo.FileName;
                    var photoPath = Path.Combine(folderPath, photoName);
                    list.Photopath = photoName;

                    using (var fileStream = new FileStream(photoPath, FileMode.Create))
                    {
                        model.Photo.CopyTo(fileStream); 
                    }

                    list.Name = model.Name;
                    _dataBase.Listmodels.Add(list);
                    _dataBase.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Listmodel model)
        {
            var entity = await _dataBase.Listmodels.FindAsync(model.ImageId);

                var photoPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", entity.Photopath);
                FileInfo fileInfo = new FileInfo(photoPath);
                fileInfo.Delete();

            _dataBase.Listmodels.Remove(entity);
            await _dataBase.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
