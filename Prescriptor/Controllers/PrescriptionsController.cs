using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prescriptor.Data;
using Prescriptor.Models;

namespace Prescriptor.Controllers
{
    public class PrescriptionsController : Controller
    {
        private readonly PrescriptorContext _context;
        private static Dictionary<string, string> DataBag;
        public PrescriptionsController(PrescriptorContext context)
        {
            _context = context;
            DataBag = new Dictionary<string, string>();
        }

        // GET: Prescriptions
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["IdSortParam"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["DrugNameSortParam"] = sortOrder == "drug_name" ? "drug_name_desc" : "drug_name";
            ViewData["DrugSubmissionDateSortParam"] = sortOrder == "drug_submission_date" ? "drug_submission_date_desc" : "drug_submission_date";
            ViewData["PatientIdSortParam"] = sortOrder == "patient_id" ? "patient_id_desc" : "patient_id";
            ViewData["PatientNameSortParam"] = sortOrder == "patient_name" ? "patient_name_desc" : "patient_name";
            ViewData["PatientLastNameSortParam"] = sortOrder == "patient_last_name" ? "patient_last_name_desc" : "patient_last_name";
            ViewData["PaymentMethodSortParam"] = sortOrder == "payment_method" ? "payment_method_desc" : "payment_method";
            ViewData["OutdatedSortParam"] = sortOrder == "outdated" ? "outdated_desc" : "outdated";
            ViewData["CurrentFilter"] = searchString;

            var prescriptions = from p in _context.Prescriptions.Include(p => p.Patient)
                                select p;

            Dictionary<string, string> payments = new Dictionary<string, string>
            {
                { Prescription.Payment.PrivatePay.ToString(), "PrivatePay" },
                { Prescription.Payment.Medicare.ToString(), "Medicare" },
                { Prescription.Payment.Other.ToString(), "Other" }
            };
            
            if (!string.IsNullOrEmpty(searchString))
            {
                prescriptions = prescriptions.Where(p => p.ID.ToString() == searchString
                                                 || p.DrugName.Contains(searchString)
                                                 || p.DrugSubmissionDate.ToShortDateString().Contains(searchString)
                                                 || p.PatientID.ToString().Contains(searchString)
                                                 || p.Patient.Name.Contains(searchString)
                                                 || p.Patient.LastName.Contains(searchString)
                                                 || payments[p.PaymentMethod.ToString()].Contains(searchString));
            }
            
            switch(sortOrder)
            {
                default:
                    prescriptions = prescriptions.OrderBy(p => p.ID);
                    break;
                case "id_desc":
                    prescriptions = prescriptions.OrderByDescending(p => p.ID);
                    break;
                case "drug_name":
                    prescriptions = prescriptions.OrderBy(p => p.DrugName);
                    break;
                case "drug_name_desc":
                    prescriptions = prescriptions.OrderByDescending(p => p.DrugName);
                    break;
                case "drug_submission_date":
                    prescriptions = prescriptions.OrderBy(p => p.DrugSubmissionDate);
                    break;
                case "drug_submission_date_desc":
                    prescriptions = prescriptions.OrderByDescending(p => p.DrugSubmissionDate);
                    break;
                case "patient_id":
                    prescriptions = prescriptions.OrderBy(p => p.Patient.ID);
                    break;
                case "patient_id_desc":
                    prescriptions = prescriptions.OrderByDescending(p => p.Patient.ID);
                    break;
                case "patient_name":
                    prescriptions = prescriptions.OrderBy(p => p.Patient.Name);
                    break;
                case "patient_name_desc":
                    prescriptions = prescriptions.OrderByDescending(p => p.Patient.Name);
                    break;
                case "patient_last_name":
                    prescriptions = prescriptions.OrderBy(p => p.Patient.LastName);
                    break;
                case "patient_last_name_desc":
                    prescriptions = prescriptions.OrderByDescending(p => p.Patient.LastName);
                    break;
                case "payment_method":
                    prescriptions = prescriptions.OrderBy(p => p.PaymentMethod);
                    break;
                case "payment_method_desc":
                    prescriptions = prescriptions.OrderByDescending(p => p.PaymentMethod);
                    break;
                case "outdated":
                    prescriptions = prescriptions.OrderBy(p => p.Outdated);
                    break;
                case "outdated_desc":
                    prescriptions = prescriptions.OrderByDescending(p => p.Outdated);
                    break;
            }
            
            
            return View(await prescriptions.AsNoTracking().ToListAsync());
        }

        // GET: Prescriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions
                .Include(p => p.Patient)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        // GET: Prescriptions/Create
        public IActionResult Create()
        {
            ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "ID");
            return View();
        }

        // POST: Prescriptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientID,DrugName,DrugSubmissionDate,PaymentMethod")] Prescription prescription)
        {
            try {
                if (ModelState.IsValid)
                {
                    prescription.Outdated = Extension.IsOutdated(prescription.DrugSubmissionDate);
                    _context.Add(prescription);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }catch(DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if problem persists " +
                    "see your system administrator.");
            }
            ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "ID", prescription.PatientID);
            return View(prescription);
        }

        // GET: Prescriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions.SingleOrDefaultAsync(m => m.ID == id);
            if (prescription == null)
            {
                return NotFound();
            }
            ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "ID", prescription.PatientID);
            return View(prescription);
        }

        // POST: Prescriptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PatientID,DrugName,DrugSubmissionDate,PaymentMethod")] Prescription prescription)
        {
            if (id != prescription.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrescriptionExists(prescription.ID))
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
            ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "ID", prescription.PatientID);
            return View(prescription);
        }

        // GET: Prescriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions
                .Include(p => p.Patient)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        // POST: Prescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prescription = await _context.Prescriptions.SingleOrDefaultAsync(m => m.ID == id);
            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrescriptionExists(int id)
        {
            return _context.Prescriptions.Any(e => e.ID == id);
        }
    }
}
