using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNETCOREAPP.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Threading;
using ASPNETCOREAPP.Exceptions;

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
                var listModel = new Listmodel();

                if (model.Photo != null)
                {
                    var folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                    var photoName = Guid.NewGuid() + "_" + model.Photo.FileName;
                    var photoPath = Path.Combine(folderPath, photoName);
                    listModel.Photopath = photoName;
                    listModel.Name = model.Name;

                    using (var fileStream = new FileStream(photoPath, FileMode.Create))
                    {
                        model.Photo.CopyTo(fileStream); 
                    }
                    
                    _dataBase.Listmodels.Add(listModel);
                    _dataBase.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Listmodel model)
        {
            if (ModelState.IsValid)
            {
                var entity = await _dataBase.Listmodels.FindAsync(model.ImageId);

                if (entity is null)
                throw new EntityNotFoundException();

                var photoPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", entity.Photopath);
                FileInfo fileInfo = new FileInfo(photoPath);
                fileInfo.Delete();

                _dataBase.Listmodels.Remove(entity);
                await _dataBase.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
