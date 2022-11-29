using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNETCOREAPP.Models;
using Microsoft.AspNetCore.DataProtection;
using ASPNETCOREAPP.Security;
using static System.String;

namespace ASPNETCOREAPP.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IDataProtector _protector;
        public EmployeeController(DatabaseContext context,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _context = context;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);
        }
        

        public async Task<IActionResult> Index(string searchText)
        {
            var employees = from em in _context.Emploees
                       select em;

            foreach (var item in employees)
            {
                item.EncryptedId = _protector.Protect(item.Id.ToString());
            }

            if (!IsNullOrEmpty(searchText))
            {
                employees = employees.Where(n => n.Name.Contains(searchText) || n.Surname.Contains(searchText) || (n.Name + " " + n.Surname).Contains(searchText)|| (n.Surname + " " + n.Name).Contains(searchText));
                return View(await employees.ToListAsync());
            }

            return View(await _context.Emploees.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: EmploeeModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmploeeModel employModel)
        {
            
            if (ModelState.IsValid)
            {

                
                bool checkmail = (from email in _context.Emploees
                                  where email.Email == employModel.Email
                                  select email).Any();
                

                var checkid = (from id in _context.Emploees
                                where id.Empid == employModel.Empid
                                select id).Count();
                if (checkmail)
                {
                    
                    ViewBag.Message = "Aseti Meilit Registrirebuli tanamshromeli ukve arsebobs";
                    return View(employModel);
                }
                
                if (checkid>0)
                {
                    ViewBag.Message = "Aseti tanamshromeli ukve arsebobs";
                    return View(employModel);
                }



                ViewBag.Message = "tanamshromeli Warmatebit Damaemata";
                _context.Add(employModel);
                
                await _context.SaveChangesAsync();

                //return RedirectToAction(nameof(Index));

                
            }
            
            return View(employModel);
            
        }

        // GET: EmploeeModels/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string decryptedId = _protector.Unprotect(id);
            int decryptedIntId = Convert.ToInt32(decryptedId);
            

            var emploeeModel = await _context.Emploees.FindAsync(decryptedIntId);
            if (emploeeModel == null)
            {
                return NotFound();
            }

            EmploeeModel model = new EmploeeModel
            {
                Email = emploeeModel.Email,
                EncryptedId = id,
                DateofBirth = emploeeModel.DateofBirth,
                Gender = emploeeModel.Gender,
                Id = decryptedIntId,
                Statusi = emploeeModel.Statusi,
                Surname = emploeeModel.Surname,
                Empid = emploeeModel.Empid,
                DateofFire = emploeeModel.DateofFire,
                Name = emploeeModel.Name,
                Posti = emploeeModel.Posti
            };

            return View(model);
        }

        [HttpPost]
        
        public async Task<IActionResult> Edit(EmploeeModel employeeModel)
        {
            if (employeeModel.EncryptedId==null)
            {
                return NotFound();
            }
            string decryptedId = _protector.Unprotect(employeeModel.EncryptedId);
            int decryptedIntId = Convert.ToInt32(decryptedId);

           if (ModelState.IsValid)
            {
                try
                {
                    var checkMail = (from email in _context.Emploees
                                      where email.Email == employeeModel.Email&& email.Id!= decryptedIntId
                                     select email).Any();

                    var checkid = (from emps in _context.Emploees
                                   where emps.Empid == employeeModel.Empid && emps.Id != decryptedIntId
                                   select emps).Any();

                    if (checkid&& checkMail)
                    {
                        TempData["errormessageId"] = "Aseti tanamshromeli ukve arsebobs";
                        TempData["errormessageEmail"] = "Aseti Meilit Registrirebuli tanamshromeli ukve arsebobs";
                        return RedirectToAction(nameof(Edit));
                    }

                    if (checkMail)
                    {
                        TempData["errormessageEmail"] = "Aseti Meilit Registrirebuli tanamshromeli ukve arsebobs";
                       return RedirectToAction(nameof(Edit));
                    }

                    if (checkid)
                    {
                        TempData["errormessageId"] = "Aseti tanamshromeli ukve arsebobs";
                        return RedirectToAction(nameof(Edit));
                    }

                    _context.Update(employeeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeModelExists(decryptedIntId))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var decryptedId = _protector.Unprotect(id);

            var decryptedIntId = Convert.ToInt32(decryptedId);

            var employModel = await _context.Emploees
                .FirstOrDefaultAsync(m => m.Id == decryptedIntId);
            if (employModel == null)
            {
                return NotFound();
            }

            return View(employModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            string decryptedId = _protector.Unprotect(id);
            int decryptedIntId = Convert.ToInt32(decryptedId);

            var model = await _context.Emploees.FindAsync(decryptedIntId);
            _context.Emploees.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeModelExists(int id)
        {
            return _context.Emploees.Any(e => e.Id == id);
        }
    }
}
