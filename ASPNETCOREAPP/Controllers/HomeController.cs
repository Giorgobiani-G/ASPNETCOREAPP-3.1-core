using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNETCOREAPP.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

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
                    model.Photo.CopyTo(new FileStream(photoPath, FileMode.Create));
                    list.Name = model.Name;
                    _dataBase.Listmodels.Add(list);
                    _dataBase.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Listmodel id)
        {
            var model = await _dataBase.Listmodels.FindAsync(id.ImageId);

            _dataBase.Listmodels.Remove(model);
            await _dataBase.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
