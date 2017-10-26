using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Services;
using BLL.Utils;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prescriptor.Web.Configuration;

namespace Prescriptor.Web.Controllers
{
    public class PrescriptionsController : Controller
    {
        private readonly PrescriptorContext _context;
        private readonly PrescriptionService _prescriptionService;

        private Settings ConfigurationSettings { get; set; }

        public PrescriptionsController(PrescriptorContext context, IOptions<Settings> settings)
        {
            _context = context;
            ConfigurationSettings = settings?.Value;
            _prescriptionService = new PrescriptionService(_context);
        }

        // GET: Prescriptions
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            if(ConfigurationSettings !=null)
                ViewData["ExpirationTime"] = ConfigurationSettings.ExpirationTime;
            ViewData["IdSortParam"] = String.IsNullOrEmpty(sortOrder) ? nameof(Support.PrescriptionsSortOrder.IdDesc) : "";
            ViewData["DrugNameSortParam"] = sortOrder == nameof(Support.PrescriptionsSortOrder.DrugName) ? nameof(Support.PrescriptionsSortOrder.DrugNameDesc) : nameof(Support.PrescriptionsSortOrder.DrugName);
            ViewData["PrescriptionCreationDateSortParam"] = sortOrder == nameof(Support.PrescriptionsSortOrder.PrescriptionCreationDate) ? nameof(Support.PrescriptionsSortOrder.PrescriptionCreationDateDesc) : nameof(Support.PrescriptionsSortOrder.PrescriptionCreationDate);
            ViewData["PatientIdSortParam"] = sortOrder == nameof(Support.PrescriptionsSortOrder.PatientId) ? nameof(Support.PrescriptionsSortOrder.PatientIdDesc) : nameof(Support.PrescriptionsSortOrder.PatientId);
            ViewData["PatientNameSortParam"] = sortOrder == nameof(Support.PrescriptionsSortOrder.PatientName) ? nameof(Support.PrescriptionsSortOrder.PatientNameDesc) : nameof(Support.PrescriptionsSortOrder.PatientName);
            ViewData["PatientLastNameSortParam"] = sortOrder == nameof(Support.PrescriptionsSortOrder.PatientLastName) ? nameof(Support.PrescriptionsSortOrder.PatientLastNameDesc) : nameof(Support.PrescriptionsSortOrder.PatientLastName);
            ViewData["PaymentMethodSortParam"] = sortOrder == nameof(Support.PrescriptionsSortOrder.PaymentMethod) ? nameof(Support.PrescriptionsSortOrder.PaymentMethodDesc) : nameof(Support.PrescriptionsSortOrder.PaymentMethod);
            ViewData["CurrentFilter"] = searchString;

            var prescriptions = _prescriptionService.GetPrescriptions();

            Dictionary<string, string> payments = new Dictionary<string, string>
            {
                { Prescription.Payment.PrivatePay.ToString(), "PrivatePay" },
                { Prescription.Payment.Medicare.ToString(), "Medicare" },
                { Prescription.Payment.Other.ToString(), "Other" }
            };
            
            if (!string.IsNullOrEmpty(searchString))
            {
                prescriptions = _prescriptionService.SearchPrescriptions(searchString, prescriptions, payments);
            }
            
            prescriptions = _prescriptionService.SortPrescriptions(sortOrder, prescriptions);
            
            
            return View(await prescriptions.AsNoTracking().ToListAsync());
        }

        // GET: Prescriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _prescriptionService.GetPrescription(id);

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
        public async Task<IActionResult> Create([Bind("PatientID,DrugName,PrescriptionCreationDate,PaymentMethod")] Prescription prescription)
        {
            try {
                if (ModelState.IsValid)
                {
                    await _prescriptionService.AddPrescription(prescription);
                    return RedirectToAction(nameof(Index));
                }
            }catch(DbUpdateException ex)
            {
                ModelState.AddModelError(ex.Message, "Unable to save changes. " +
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

            var prescription = await _prescriptionService.EditGetPrescription(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,PatientID,DrugName,PrescriptionCreationDate,PaymentMethod")] Prescription prescription)
        {
            if (id != prescription.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _prescriptionService.EditUpdatePrescription(prescription);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_prescriptionService.PrescriptionExists(prescription.ID))
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

            var prescription = await _prescriptionService.GetPrescription(id);

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
            await _prescriptionService.DeletePrescription(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
