using System.Threading.Tasks;
using BLL.Services;
using BLL.Utils;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Prescriptor.Web.Controllers
{
    public class PatientsController : Controller
    {
        private readonly PrescriptorContext _context;
        private readonly PatientService _patientService;

        public PatientsController(PrescriptorContext context)
        {
            _context = context;
            _patientService = new PatientService(_context);
        }

        // GET: Patients
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["PatientIdSortParam"] = string.IsNullOrEmpty(sortOrder) ? nameof(Support.PatientsSortOrder.PatientIdDesc) : "";
            ViewData["PatientNameSortParam"] = sortOrder == nameof(Support.PatientsSortOrder.PatientName) ? nameof(Support.PatientsSortOrder.PatientNameDesc) : nameof(Support.PatientsSortOrder.PatientName);
            ViewData["PatientLastNameSortParam"] = sortOrder == nameof(Support.PatientsSortOrder.PatientLastName) ? nameof(Support.PatientsSortOrder.PatientLastNameDesc) : nameof(Support.PatientsSortOrder.PatientLastName);
            ViewData["PatientBirthDateSortParam"] = sortOrder == nameof(Support.PatientsSortOrder.PatientBirthDate) ? nameof(Support.PatientsSortOrder.PatientBirthDateDesc) : nameof(Support.PatientsSortOrder.PatientBirthDate);
            ViewData["PatientPhoneNumberSortParam"] = sortOrder == nameof(Support.PatientsSortOrder.PatientPhoneNumber) ? nameof(Support.PatientsSortOrder.PatientPhoneNumberDesc) : nameof(Support.PatientsSortOrder.PatientPhoneNumber);

            var patients = _patientService.GetPatients();

            if (!string.IsNullOrEmpty(searchString))
            {
                patients = _patientService.SearchPatients(searchString, patients);
            }

            patients = _patientService.SortPatients(sortOrder, patients);

            return View(await patients.AsNoTracking().ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _patientService.GetPatient(id);

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
                await _patientService.AddPatient(patient);
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

            var patient = await _patientService.GetPatient(id);
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
                    await _patientService.UpdatePatient(patient);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_patientService.PatientExists(patient.ID))
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

            var patient = await _patientService.GetPatient(id);
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
            await _patientService.DeletePatient(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
