using System.Linq;
using System.Threading.Tasks;
using BLL.Models;
using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Prescriptor.Web.Controllers
{
    public class PatientsController : Controller
    {
        private readonly PrescriptorContext _context;

        public PatientsController(PrescriptorContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["PatientIdSortParam"] = string.IsNullOrEmpty(sortOrder) ? "patient_id_desc" : "";
            ViewData["PatientNameSortParam"] = sortOrder == "patient_name" ? "patient_name_desc" : "patient_name";
            ViewData["PatientLastNameSortParam"] = sortOrder == "patient_last_name" ? "patient_last_name_desc" : "patient_last_name";
            ViewData["PatientBirthDateSortParam"] = sortOrder == "patient_birth_date" ? "patient_birth_date_desc" : "patient_birth_date";
            ViewData["PatientPhoneNumberSortParam"] = sortOrder == "patient_phone_number" ? "patient_phone_number_desc" : "patient_phone_number";

            var patients = from p in _context.Patients
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                patients = patients.Where(p => p.ID.ToString() == searchString
                                                 || p.ID.ToString().Contains(searchString)
                                                 || (!string.IsNullOrEmpty(p.Name) && p.Name.Contains(searchString))
                                                 || (!string.IsNullOrEmpty(p.LastName) && p.LastName.Contains(searchString))
                                                 || (!string.IsNullOrEmpty(p.BirthDate.ToLongDateString()) && p.BirthDate.ToShortDateString().Contains(searchString))
                                                 || (!string.IsNullOrEmpty(p.PhoneNumber) && p.PhoneNumber.Contains(searchString)));
            }

            switch (sortOrder)
            {
                default:
                    patients = patients.OrderBy(p => p.ID);
                    break;
                case "patient_id_desc":
                    patients = patients.OrderByDescending(p => p.ID);
                    break;
                case "patient_name":
                    patients = patients.OrderBy(p => p.Name);
                    break;
                case "patient_name_desc":
                    patients = patients.OrderByDescending(p => p.Name);
                    break;
                case "patient_last_name":
                    patients = patients.OrderBy(p => p.LastName);
                    break;
                case "patient_last_name_desc":
                    patients = patients.OrderByDescending(p => p.LastName);
                    break;
                case "patient_birth_date":
                    patients = patients.OrderBy(p => p.BirthDate);
                    break;
                case "patient_birth_date_desc":
                    patients = patients.OrderByDescending(p => p.BirthDate);
                    break;
                case "patient_phone_number":
                    patients = patients.OrderBy(p => p.PhoneNumber);
                    break;
                case "patient_phone_number_desc":
                    patients = patients.OrderByDescending(p => p.PhoneNumber);
                    break;
            }
            return View(await patients.AsNoTracking().ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .SingleOrDefaultAsync(m => m.ID == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,LastName,BirthDate,PhoneNumber")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.SingleOrDefaultAsync(m => m.ID == id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,LastName,BirthDate,PhoneNumber")] Patient patient)
        {
            if (id != patient.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .SingleOrDefaultAsync(m => m.ID == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.SingleOrDefaultAsync(m => m.ID == id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.ID == id);
        }
    }
}
