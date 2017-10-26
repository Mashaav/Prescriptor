using System.Threading.Tasks;
using DAL.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using BLL.Utils;

namespace BLL.Services
{
    public class PrescriptionService : IPrescriptionService
    {

        private readonly PrescriptorContext _context;
        public PrescriptionService(PrescriptorContext context)
        {
            _context = context;
        }

        public IQueryable<Prescription> GetPrescriptions()
        {
            var prescriptions = from p in _context.Prescriptions.Include(p => p.Patient)
                select p;
            return prescriptions;
        }
        public IQueryable<Prescription> SearchPrescriptions(string searchString, IQueryable<Prescription> prescriptions, Dictionary<string, string> payments)
        {
            prescriptions = prescriptions.Where(p => p.ID.ToString() == searchString
                                                     || (!string.IsNullOrEmpty(p.DrugName) && p.DrugName.Contains(searchString))
                                                     || (!string.IsNullOrEmpty(p.PrescriptionCreationDate.ToLongDateString()) &&
                                                         p.PrescriptionCreationDate.ToShortDateString().Contains(searchString))
                                                     || (!string.IsNullOrEmpty(p.PatientID.ToString()) &&
                                                         p.PatientID.ToString().Contains(searchString))
                                                     || (!string.IsNullOrEmpty(p.Patient.Name) &&
                                                         p.Patient.Name.Contains(searchString))
                                                     || (!string.IsNullOrEmpty(p.Patient.LastName) &&
                                                         p.Patient.LastName.Contains(searchString))
                                                     || (!string.IsNullOrEmpty(p.PaymentMethod.ToString()) &&
                                                         payments[p.PaymentMethod.ToString()].Contains(searchString)));
            return prescriptions;
        }

        public IQueryable<Prescription> SortPrescriptions(string sortOrder, IQueryable<Prescription> prescriptions)
        {
            switch (sortOrder)
            {
                default:
                    prescriptions = prescriptions.OrderBy(p => p.ID);
                    break;
                case nameof(Support.PrescriptionsSortOrder.IdDesc):
                    prescriptions = prescriptions.OrderByDescending(p => p.ID);
                    break;
                case nameof(Support.PrescriptionsSortOrder.DrugName):
                    prescriptions = prescriptions.OrderBy(p => p.DrugName);
                    break;
                case nameof(Support.PrescriptionsSortOrder.DrugNameDesc):
                    prescriptions = prescriptions.OrderByDescending(p => p.DrugName);
                    break;
                case nameof(Support.PrescriptionsSortOrder.PrescriptionCreationDate):
                    prescriptions = prescriptions.OrderBy(p => p.PrescriptionCreationDate);
                    break;
                case nameof(Support.PrescriptionsSortOrder.PrescriptionCreationDateDesc):
                    prescriptions = prescriptions.OrderByDescending(p => p.PrescriptionCreationDate);
                    break;
                case nameof(Support.PrescriptionsSortOrder.PatientId):
                    prescriptions = prescriptions.OrderBy(p => p.Patient.ID);
                    break;
                case nameof(Support.PrescriptionsSortOrder.PatientIdDesc):
                    prescriptions = prescriptions.OrderByDescending(p => p.Patient.ID);
                    break;
                case nameof(Support.PrescriptionsSortOrder.PatientName):
                    prescriptions = prescriptions.OrderBy(p => p.Patient.Name);
                    break;
                case nameof(Support.PrescriptionsSortOrder.PatientNameDesc):
                    prescriptions = prescriptions.OrderByDescending(p => p.Patient.Name);
                    break;
                case nameof(Support.PrescriptionsSortOrder.PatientLastName):
                    prescriptions = prescriptions.OrderBy(p => p.Patient.LastName);
                    break;
                case nameof(Support.PrescriptionsSortOrder.PatientLastNameDesc):
                    prescriptions = prescriptions.OrderByDescending(p => p.Patient.LastName);
                    break;
                case nameof(Support.PrescriptionsSortOrder.PaymentMethod):
                    prescriptions = prescriptions.OrderBy(p => p.PaymentMethod);
                    break;
                case nameof(Support.PrescriptionsSortOrder.PaymentMethodDesc):
                    prescriptions = prescriptions.OrderByDescending(p => p.PaymentMethod);
                    break;
            }
            return prescriptions;
        }

        public async Task<Prescription> GetPrescription(int? id)
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.Patient)
                .SingleOrDefaultAsync(m => m.ID == id);
            return prescription;
        }

        public async Task<Prescription> EditGetPrescription(int? id)
        {
            var prescription = await _context.Prescriptions.SingleOrDefaultAsync(m => m.ID == id);
            return prescription;
        }

        public async Task EditUpdatePrescription(Prescription prescription)
        {
            _context.Update(prescription);
            await _context.SaveChangesAsync();
        }

        public async Task AddPrescription(Prescription prescription)
        {
            _context.Add(prescription);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePrescription(int id)
        {
            var prescription = await _context.Prescriptions.SingleOrDefaultAsync(m => m.ID == id);
            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
        }

        public bool PrescriptionExists(int id)
        {
            return _context.Prescriptions.Any(e => e.ID == id);
        }
    }
}
