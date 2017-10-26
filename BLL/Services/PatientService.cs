using DAL.Data;
using DAL.Models;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Utils;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class PatientService : IPatientService
    {
        private readonly PrescriptorContext _context;
        public PatientService(PrescriptorContext context)
        {
            _context = context;
        }

        public IQueryable<Patient> SortPatients(string sortOrder, IQueryable<Patient> patients)
        {
            switch (sortOrder)
            {
                default:
                    patients = patients.OrderBy(p => p.ID);
                    break;
                case nameof(Support.PatientsSortOrder.PatientIdDesc):
                    patients = patients.OrderByDescending(p => p.ID);
                    break;
                case nameof(Support.PatientsSortOrder.PatientName):
                    patients = patients.OrderBy(p => p.Name);
                    break;
                case nameof(Support.PatientsSortOrder.PatientNameDesc):
                    patients = patients.OrderByDescending(p => p.Name);
                    break;
                case nameof(Support.PatientsSortOrder.PatientLastName):
                    patients = patients.OrderBy(p => p.LastName);
                    break;
                case nameof(Support.PatientsSortOrder.PatientLastNameDesc):
                    patients = patients.OrderByDescending(p => p.LastName);
                    break;
                case nameof(Support.PatientsSortOrder.PatientBirthDate):
                    patients = patients.OrderBy(p => p.BirthDate);
                    break;
                case nameof(Support.PatientsSortOrder.PatientBirthDateDesc):
                    patients = patients.OrderByDescending(p => p.BirthDate);
                    break;
                case nameof(Support.PatientsSortOrder.PatientPhoneNumber):
                    patients = patients.OrderBy(p => p.PhoneNumber);
                    break;
                case nameof(Support.PatientsSortOrder.PatientPhoneNumberDesc):
                    patients = patients.OrderByDescending(p => p.PhoneNumber);
                    break;
            }
            return patients;
        }

        public IQueryable<Patient> SearchPatients(string searchString, IQueryable<Patient> patients)
        {
            patients = patients.Where(p => p.ID.ToString() == searchString
                                           || p.ID.ToString().Contains(searchString)
                                           || (!string.IsNullOrEmpty(p.Name) && p.Name.Contains(searchString))
                                           || (!string.IsNullOrEmpty(p.LastName) && p.LastName.Contains(searchString))
                                           || (!string.IsNullOrEmpty(p.BirthDate.ToLongDateString()) &&
                                               p.BirthDate.ToShortDateString().Contains(searchString))
                                           || (!string.IsNullOrEmpty(p.PhoneNumber) && p.PhoneNumber.Contains(searchString)));
            return patients;
        }

        public IQueryable<Patient> GetPatients()
        {
            var patients = from p in _context.Patients
                           select p;
            return patients;
        }
        public async Task<Patient> GetPatient(int? id)
        {
            var patient = await _context.Patients.SingleOrDefaultAsync(m => m.ID == id);
            return patient;
        }

        public async Task AddPatient(Patient patient)
        {
            _context.Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatient(Patient patient)
        {
            _context.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePatient(int id)
        {
            var patient = await _context.Patients.SingleOrDefaultAsync(m => m.ID == id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }

        public bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.ID == id);
        }


    }
}
